using Controllers;
using Models;
using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using Views;

public enum ECGGeneratorState
{
    Idle, Generating, Done
}
public class ECGGeneratorController : MonoBehaviour
{
    public ECGGeneratorState state;
    private void Awake()
    {
        state = ECGGeneratorState.Idle;
    }
    public void GenerateECG()
    {
        StartCoroutine(_StartECGProcess());
    }

    private IEnumerator _StartECGProcess()
    {
        LoadingView.instance.ShowLoading(true);
        NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Alert, "Loading", "Generating ECG"));
        ECGGeneratorData data = new ECGGeneratorData();
        if (ECGGeneratorView.instance.isCustomValue())
        {
            data = ECGGeneratorView.instance.GetECGCustomData();
        }
        new Thread(delegate ()
        {
            _CallECGProcess(data);
        }).Start();
        yield return new WaitUntil(() => state == ECGGeneratorState.Done);
        state = ECGGeneratorState.Idle;
        LoadingView.instance.ShowLoading(false);
        NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Success, "Success", "ECG Generated"));
        NavigationViewController.instance.MoveToECGResulView();
        ECGResultController.instance.Init();
        UnityEngine.Debug.Log("Done Generating");
    }

    private void _CallECGProcess(ECGGeneratorData data)
    {
        state = ECGGeneratorState.Generating;


        try
        {

            UnityEngine.Debug.Log("Generating");


            // 1) Create Process Info
            var psi = new ProcessStartInfo();
            psi.FileName = @Config.PYTHON_LOC_GUY;

            // 2) Provide script and arguments
            var script = @Application.streamingAssetsPath + Config.ECG_SCRIPT_LOC;

            psi.Arguments = $"\"{script}\" \"{data.defaultGeneratorValues}\" \"{data.heartRate}\" \"{data.amp_pwav}\" \"{data.dist_pwav}\" \"{data.time_pwav}\" \"{data.amp_qwav}\" \"{data.dist_qwav}\" \"{data.amp_qrswav}\" \"{data.dist_qrswav}\" \"{data.amp_swav}\" \"{data.dist_swav}\" \"{data.amp_twav}\" \"{data.dist_twav}\" \"{data.time_twav}\" \"{data.amp_uwav}\" \"{data.dist_uwav}\"";

            // 3) Process configuration
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            // 4) Execute process and get output
            var errors = "";
            var results = "";

            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }

            // 5) Display output
            UnityEngine.Debug.Log("ERRORS:");
            UnityEngine.Debug.Log(errors);
            //UnityEngine.Debug.Log("\n");
            //UnityEngine.Debug.Log("Results:");
            //UnityEngine.Debug.Log(results);
            if (results.Contains(Config.EOF_ECG_GEN))
            {
                state = ECGGeneratorState.Done;
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e);
        }

    }

}


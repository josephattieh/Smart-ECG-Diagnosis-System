using UnityEngine;
using UnityEngine.UI;

public class FilterAllECGS : MonoBehaviour
{
    public static FilterAllECGS instance;
    public InputField date;
    public InputField diag;
    public InputField humanDiag;
    public Button apply;
    public Button cancel;

    private void Start()
    {
        apply.onClick.AddListener(delegate
        {
            AllECGView.instance.ClearContent();
            StartCoroutine(Requests.instance.SearchECG(date.text, diag.text, humanDiag.text, (res) =>
            {
                EcgRecords[] rec = res;
                for (int i = 0; i < rec.Length; i++)
                {
                    AllECGView.instance.AddECG(rec[i].patientId, rec[i].ecgId, "ok", rec[i].dateOfAcquisition, rec[i].timeOfAcquisition, rec[i].humanDiagnosis, res[i]);
                }
                gameObject.SetActive(false);
            }));
        });

        cancel.onClick.AddListener(delegate
        {
            AllECGView.instance.ClearContent();
            StartCoroutine(Requests.instance.SearchECG("", "", "", (res) =>
            {
                EcgRecords[] rec = res;
                for (int i = 0; i < rec.Length; i++)
                {
                    AllECGView.instance.AddECG(rec[i].patientId, rec[i].ecgId, "ok", rec[i].dateOfAcquisition, rec[i].timeOfAcquisition, rec[i].humanDiagnosis, res[i]);
                }
                gameObject.SetActive(false);
            }));
        });
    }


    private void Awake()
    {
        instance = this;

    }

    public void Clear()
    {
        date.text = "";
        diag.text = "";
        humanDiag.text = "";
    }
}

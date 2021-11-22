using UnityEngine;
using UnityEngine.UI;
using Views;

public class UnconfirmedECGView : MonoBehaviour
{
    public static UnconfirmedECGView instance;
    public GameObject content;
    public GameObject unconfirmedPrefab;

    public EcgRecords currentECGRecord;

    public Animator unconfirmedDiagAnim;
    public Animator ecgViewAnim;
    public GameObject ecgViewPanel;

    private void Awake()
    {
        instance = this;
    }
    public void ClearContent()
    {
        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void AddUnconfirmedECG(EcgRecords ecg, string patientName, string date, string time, string humanDiag)
    {

        //ClearContent();

        GameObject unDiag = Instantiate(unconfirmedPrefab);

        unDiag.name = "ECG";
        Text nameIF = unDiag.transform.Find("PatientName").GetComponent<Text>();
        Text dateIF = unDiag.transform.Find("Date").GetComponent<Text>();
        Text timeIF = unDiag.transform.Find("Time").GetComponent<Text>();
        Text humanDiagIF = unDiag.transform.Find("Human Diag").GetComponent<Text>();
        nameIF.text = patientName;
        StartCoroutine(Requests.instance.GetPatientByID(ecg.patientId, (resp) =>
          {
              nameIF.text = resp[0].name;
          }));
        dateIF.text = date;
        timeIF.text = time;
        humanDiagIF.text = humanDiag;

        unDiag.transform.SetParent(content.transform);
        unDiag.GetComponent<RectTransform>().localScale = Vector3.one;
        unDiag.GetComponent<RectTransform>().localPosition = new Vector3(unDiag.GetComponent<RectTransform>().localPosition.x, unDiag.GetComponent<RectTransform>().localPosition.y, 0);

        LUI_UIAnimManager anim = unDiag.GetComponent<LUI_UIAnimManager>();
        anim.oldAnimator = unconfirmedDiagAnim;
        anim.newAnimator = ecgViewAnim;
        anim.newPanel = ecgViewPanel;
        anim.animButton = unDiag.GetComponent<Button>();

        unDiag.GetComponent<Button>().onClick.AddListener(delegate
        {
            currentECGRecord = ecg;

            ECGView.instance.currentECGForPatient = ecg;
            ECGView.instance.currentECGID = ecg.ecgId;
            ECGView.instance.selectedPatientID = ecg.patientId;
            StartCoroutine(ECGView.instance.GenerateECGViewForUnconfirmed());
        });
        unDiag.GetComponent<RectTransform>().localPosition = new Vector3(unDiag.GetComponent<RectTransform>().localPosition.x, unDiag.GetComponent<RectTransform>().localPosition.y, 0);
    }
}

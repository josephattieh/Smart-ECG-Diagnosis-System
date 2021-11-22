using UnityEngine;
using UnityEngine.UI;
using Views;

public class AllECGView : MonoBehaviour
{
    public static AllECGView instance;
    public GameObject content;
    public GameObject ecgPrefab;
    public Animator oldAnim;
    public Animator ECGAnim;
    public GameObject ECGPanel;

    public string currentECGID = "";
    public EcgRecords currentECG;

    public Button filterBtn;
    public GameObject filterAllECGsPanel;

    private void Start()
    {
        filterBtn.onClick.AddListener(delegate
        {
            filterAllECGsPanel.SetActive(true);
            FilterAllECGS.instance.Clear();
        });
    }

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
    public void AddECG(string patientID, string id, string name, string date, string time, string diag, EcgRecords ecgRecord)
    {

        //ClearContent();

        GameObject patientlessSummary = Instantiate(ecgPrefab);

        patientlessSummary.name = "ECG";
        Text fullName = patientlessSummary.transform.Find("FullName").GetComponent<Text>();
        Text userName = patientlessSummary.transform.Find("UserName").GetComponent<Text>();
        Text prof = patientlessSummary.transform.Find("Role").GetComponent<Text>();
        Text roleDesc = patientlessSummary.transform.Find("RoleDesc").GetComponent<Text>();
        StartCoroutine(Requests.instance.GetPatientByID(patientID, (ress) =>
        {
            fullName.text = ress[0].name;
        }));

        userName.text = date;
        prof.text = time;
        roleDesc.text = diag;
        patientlessSummary.transform.SetParent(content.transform);
        patientlessSummary.GetComponent<RectTransform>().localScale = Vector3.one;
        patientlessSummary.GetComponent<RectTransform>().localPosition = new Vector3(patientlessSummary.GetComponent<RectTransform>().localPosition.x, patientlessSummary.GetComponent<RectTransform>().localPosition.y, 0);

        LUI_UIAnimManager anim = patientlessSummary.GetComponent<LUI_UIAnimManager>();
        anim.oldAnimator = oldAnim;
        anim.newAnimator = ECGAnim;
        anim.newPanel = ECGPanel;
        anim.animButton = patientlessSummary.GetComponent<Button>();

        patientlessSummary.GetComponent<Button>().onClick.AddListener(delegate
        {
            currentECGID = id;
            currentECG = ecgRecord;
            ECGView.instance.currentECGID = id;
            ECGView.instance.currentECGRecord = ecgRecord;
            ECGView.instance.currentECGForPatient = ecgRecord;
            // Add here ECG currentID
            // Generate Diagnosis here
            // Actually call all commands here but put them in the ECGView class
            StartCoroutine(ECGView.instance.GenerateDiagnosisNew(true, ecgRecord.patientId));


        });
        patientlessSummary.GetComponent<RectTransform>().localPosition = new Vector3(patientlessSummary.GetComponent<RectTransform>().localPosition.x, patientlessSummary.GetComponent<RectTransform>().localPosition.y, 0);
    }
}

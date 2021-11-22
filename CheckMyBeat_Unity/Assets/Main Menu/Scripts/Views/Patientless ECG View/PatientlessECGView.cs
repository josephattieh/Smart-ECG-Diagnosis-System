using UnityEngine;
using UnityEngine.UI;
using Views;

public class PatientlessECGView : MonoBehaviour
{
    public static PatientlessECGView instance;
    public GameObject content;
    public GameObject patientlessECGPrefab;
    public Animator patientlessAnim;
    public Animator ECGAnim;
    public GameObject ECGPanel;

    public Button filterButton;
    public GameObject filterPatientlessPanel;

    public string currentECGID = "";
    public EcgFetched currentECG;

    private void Start()
    {
        filterButton.onClick.AddListener(delegate
        {
            filterPatientlessPanel.SetActive(true);
            FilterPatientless.instance.Clear();
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
    public void AddPatientLessECG(string id, string date, string time, EcgFetched ecgFetched)
    {

        //ClearContent();

        GameObject patientlessSummary = Instantiate(patientlessECGPrefab);

        patientlessSummary.name = "Patientless ECG";
        Text fullName = patientlessSummary.transform.Find("FullName").GetComponent<Text>();
        Text userName = patientlessSummary.transform.Find("UserName").GetComponent<Text>();
        Text prof = patientlessSummary.transform.Find("Role").GetComponent<Text>();
        Text roleDesc = patientlessSummary.transform.Find("RoleDesc").GetComponent<Text>();
        fullName.text = "ECG";
        userName.text = date;
        prof.text = time;
        roleDesc.text = "";
        patientlessSummary.transform.SetParent(content.transform);
        patientlessSummary.GetComponent<RectTransform>().localScale = Vector3.one;
        patientlessSummary.GetComponent<RectTransform>().localPosition = new Vector3(patientlessSummary.GetComponent<RectTransform>().localPosition.x, patientlessSummary.GetComponent<RectTransform>().localPosition.y, 0);

        LUI_UIAnimManager anim = patientlessSummary.GetComponent<LUI_UIAnimManager>();
        anim.oldAnimator = patientlessAnim;
        anim.newAnimator = ECGAnim;
        anim.newPanel = ECGPanel;
        anim.animButton = patientlessSummary.GetComponent<Button>();

        patientlessSummary.GetComponent<Button>().onClick.AddListener(delegate
        {
            currentECGID = id;
            currentECG = ecgFetched;
            ECGView.instance.currentECGID = id;
            ECGView.instance.currentECG = ecgFetched;
            // Add here ECG currentID
            // Generate Diagnosis here
            // Actually call all commands here but put them in the ECGView class
            StartCoroutine(ECGView.instance.GenerateDiagnosis());
           

        });
        patientlessSummary.GetComponent<RectTransform>().localPosition = new Vector3(patientlessSummary.GetComponent<RectTransform>().localPosition.x, patientlessSummary.GetComponent<RectTransform>().localPosition.y, 0);
    }
}

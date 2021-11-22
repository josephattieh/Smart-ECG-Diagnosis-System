using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class AllPatientsView : MonoBehaviour
{
    public static AllPatientsView instance;
    public GameObject content;
    public GameObject patientPrefab;
    public InputField searchPatients;
    public Animator allPatientsAnim;
    public Animator patientPanelAnim;

    public GameObject patientPanel;

    public GameObject filterPatientsPanel;
    public Button filterButton;

    // Buttons related to patient panel
    public Button approve;
    public Button reject;

    public string currentUserID = "";

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        filterButton.onClick.AddListener(delegate
        {
            filterPatientsPanel.SetActive(true);
            FilterPatients.instance.Clear();
        });
        

        //searchPatients.onValueChanged.AddListener(delegate
        //{
        //    List<GameObject> patientSummaries = new List<GameObject>();

        //    foreach (Transform child in content.transform)
        //    {
        //        patientSummaries.Add(child.gameObject);
        //    }

        //    string searchMetric = searchPatients.text;

        //    foreach (GameObject go in patientSummaries)
        //    {
        //        Text fullName = go.transform.Find("FullName").GetComponent<Text>();
        //        if (fullName.text.Contains(searchMetric))
        //        {
        //            go.SetActive(true);
        //        }
        //        else
        //        {
        //            go.SetActive(false);
        //        }

        //        if (searchMetric.Length == 0)
        //        {
        //            go.SetActive(true);
        //        }

        //    }


        //});
    }

    public void ClearContent()
    {
        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public void AddPatient(string id, string name, string gender, string bloodType, string diseases)
    {

        //ClearContent();

        GameObject patientSummary = Instantiate(patientPrefab);

        patientSummary.name = "Auth Req";
        Text fullName = patientSummary.transform.Find("FullName").GetComponent<Text>();
        Text userName = patientSummary.transform.Find("UserName").GetComponent<Text>();
        Text prof = patientSummary.transform.Find("Role").GetComponent<Text>();
        Text roleDesc = patientSummary.transform.Find("RoleDesc").GetComponent<Text>();
        fullName.text = name;
        userName.text = gender;
        prof.text = bloodType;
        roleDesc.text = diseases;
        patientSummary.transform.SetParent(content.transform);
        patientSummary.GetComponent<RectTransform>().localScale = Vector3.one;
        patientSummary.GetComponent<RectTransform>().localPosition = new Vector3(patientSummary.GetComponent<RectTransform>().localPosition.x, patientSummary.GetComponent<RectTransform>().localPosition.y, 0);
        //Destroy(patientSummary.GetComponent<LUI_UIAnimManager>());
        LUI_UIAnimManager anim = patientSummary.GetComponent<LUI_UIAnimManager>();
        anim.oldAnimator = allPatientsAnim;
        anim.newAnimator = patientPanelAnim;
        anim.newPanel = patientPanel;
        anim.animButton = patientSummary.GetComponent<Button>();

        patientSummary.GetComponent<Button>().onClick.AddListener(delegate
        {
            currentUserID = id;
            PatientView.instance.selectedPatientID = id;
            PatientView.instance.PopulatePatient(name, bloodType, diseases, gender);
        });
        patientSummary.GetComponent<RectTransform>().localPosition = new Vector3(patientSummary.GetComponent<RectTransform>().localPosition.x, patientSummary.GetComponent<RectTransform>().localPosition.y, 0);
    }
}

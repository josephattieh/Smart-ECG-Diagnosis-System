using Models;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class AllPatientsViewECG : MonoBehaviour
{
    public static AllPatientsViewECG instance;
    public GameObject content;
    public GameObject patientPrefab;
    public Animator allPatientsAnim;
    public Animator ecgPanelAnim;
    public GameObject ecgPanel;


    public string currentUserID = "";

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
    public void AddPatientForECG(string id, string name, string gender, string bloodType, string diseases)
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
        LUI_UIAnimManager anim = patientSummary.GetComponent<LUI_UIAnimManager>();
        anim.oldAnimator = allPatientsAnim;
        anim.newAnimator = ecgPanelAnim;
        anim.newPanel = ecgPanel;
        anim.animButton = patientSummary.GetComponent<Button>();

        patientSummary.GetComponent<Button>().onClick.AddListener(delegate
        {
            currentUserID = id;
            NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Success, "Patient Selected", name));
            ECGView.instance.selectedPatientID = id;
        });
        patientSummary.GetComponent<RectTransform>().localPosition = new Vector3(patientSummary.GetComponent<RectTransform>().localPosition.x, patientSummary.GetComponent<RectTransform>().localPosition.y, 0);
    }
}

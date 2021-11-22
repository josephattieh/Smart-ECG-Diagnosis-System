using Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class PatientView : MonoBehaviour
{
    public InputField fullName;
    public CustomDropdown bloodType;
    public CustomDropdown diseases;
    public CustomDropdown gender;


    public GameObject content;
    public GameObject patientDiagPrefab;

    public Button back;
    public Button save;
    public Button delete;


    public string selectedPatientID = "";

    public static PatientView instance;

    public Animator patientAnim;
    public Animator ECGAnim;
    public GameObject ECGPanel;

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
        save.onClick.AddListener(delegate
        {
            StartCoroutine(Requests.instance.UpdatePatient(CurrentUserModel.instance.userInfo.username, selectedPatientID, gender.GetSelectedOption(), fullName.text, "pic", bloodType.GetSelectedOption(), new List<string>() { diseases.GetSelectedOption() }, (res) =>
                  {
                      NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Success, "Profile Update", res.status));
                  }));
        });

        delete.onClick.AddListener(delegate
        {
            StartCoroutine(Requests.instance.DeletePatient(CurrentUserModel.instance.userInfo.username, selectedPatientID, (re) =>
            {
                
                NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Alert, "Profile Delete", re.status));

                AllPatientsView.instance.ClearContent();
                StartCoroutine(Requests.instance.GetAllPatients((response) =>
                {
                    LoadingView.instance.ShowLoading(true);
                    Patient[] res = response;

                    for (int i = 0; i < res.Length; i++)
                    {
                        AllPatientsView.instance.AddPatient(res[i].patientId, res[i].name, res[i].gender, res[i].bloodType, res[i].diseases[0]);
                    }
                    LoadingView.instance.ShowLoading(false);
                }));

            }));

        });
        back.onClick.AddListener(delegate
        {
            AllPatientsView.instance.ClearContent();
            StartCoroutine(Requests.instance.GetAllPatients((response) =>
            {
                LoadingView.instance.ShowLoading(true);
                Patient[] res = response;

                for (int i = 0; i < res.Length; i++)
                {
                    AllPatientsView.instance.AddPatient(res[i].patientId, res[i].name, res[i].gender, res[i].bloodType, res[i].diseases[0]);
                }
                LoadingView.instance.ShowLoading(false);
            }));
        });

    }

    public void ClearScrollContent()
    {
        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }


    public void PopulatePatient(string name, string blood, string disease, string gend)
    {
        fullName.text = name;
        bloodType.SetSelectedOption(blood);
        diseases.SetSelectedOption(disease);
        gender.SetSelectedOption(gend);

        StartCoroutine(Requests.instance.GetPatientRecords(selectedPatientID, (res) =>
        {
            ClearScrollContent();
            foreach (EcgRecords rec in res)
            {
                string date = rec.dateOfAcquisition;
                string time = rec.timeOfAcquisition;
                AddPatientECG(time, date, rec);
            }
        }));
    }


    public void AddPatientECG(string time, string date, EcgRecords ecg)
    {

        //ClearContent();

        GameObject machineDiag = Instantiate(patientDiagPrefab);

        machineDiag.name = "Patient ECG";
        Text dateIF = machineDiag.transform.Find("Date").GetComponent<Text>();
        Text timeIF = machineDiag.transform.Find("Time").GetComponent<Text>();
        dateIF.text = date;
        timeIF.text = time;

        machineDiag.transform.SetParent(content.transform);
        machineDiag.GetComponent<RectTransform>().localScale = Vector3.one;
        machineDiag.GetComponent<RectTransform>().localPosition = new Vector3(machineDiag.GetComponent<RectTransform>().localPosition.x, machineDiag.GetComponent<RectTransform>().localPosition.y, 0);
        //Destroy(machineDiag.GetComponent<LUI_UIAnimManager>());

        LUI_UIAnimManager anim = machineDiag.GetComponent<LUI_UIAnimManager>();
        anim.oldAnimator = patientAnim;
        anim.newAnimator = ECGAnim;
        anim.newPanel = ECGPanel;
        anim.animButton = machineDiag.GetComponent<Button>();

        machineDiag.GetComponent<Button>().onClick.AddListener(delegate
        {
            ECGView.instance.currentECGID = ecg.ecgId;
            ECGView.instance.selectedPatientID = ecg.patientId;
            ECGView.instance.currentECGForPatient = ecg;
            // Add here ECG currentID
            // Generate Diagnosis here
            // Actually call all commands here but put them in the ECGView class
            StartCoroutine(ECGView.instance.GenerateDiagnosisNew(false,ecg.patientId));
        });


        machineDiag.GetComponent<RectTransform>().localPosition = new Vector3(machineDiag.GetComponent<RectTransform>().localPosition.x, machineDiag.GetComponent<RectTransform>().localPosition.y, 0);
    }
}

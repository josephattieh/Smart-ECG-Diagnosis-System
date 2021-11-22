using Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class ECGView : MonoBehaviour
{
    public static ECGView instance;
    public InputField dateAndTime;
    public InputField bpm;
    public InputField samplingFreq;
    public InputField humanDiagnosis;
    public Button associateToPatient;

    public Button applyChanges;
    public Button cancel;

    public GameObject content;
    public GameObject machineDiagPrefab;

    public string currentECGID;
    public EcgFetched currentECG;
    public EcgRecords currentECGForPatient;
    public string selectedPatientID = "";
    public string oldSelectedPatientID = "";
    public Features features;
    public EcgRecords currentECGRecord;


    public Animator patientlessECGAnim;
    public GameObject patientlessPanel;

    public Animator patientViewAnimator;
    public GameObject patientViewPanel;

    public Animator unconfirmedAnimator;
    public GameObject unconfirmedPanel;

    public Animator allECGsAnimator;
    public GameObject allECGsPanel;

    public Button updateDiag;

    public EcgRecords ecgToUse;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        HandleSaveButton();
        HandleUpdateDiagnosis();
    }

    public IEnumerator GenerateDiagnosis()
    {
        selectedPatientID = "";
        oldSelectedPatientID = "";
        LoadingView.instance.ShowLoading(true);
        ECGView.instance.associateToPatient.enabled = true;
        // Generate Diagnosis, once everything is done, then populate view.

        cancel.GetComponent<LUI_UIAnimManager>().newAnimator = patientlessECGAnim;
        cancel.GetComponent<LUI_UIAnimManager>().newPanel = patientlessPanel;


        applyChanges.GetComponent<LUI_UIAnimManager>().newAnimator = patientlessECGAnim;
        applyChanges.GetComponent<LUI_UIAnimManager>().newPanel = patientlessPanel;




        int RRint = int.Parse(currentECG.numberOfRRIntervals);
        yield return StartCoroutine(Requests.instance.GenerateFirstDiagnosis(currentECG.data.OfType<float>().ToList(), RRint, int.Parse(currentECG.fs), (res) =>
          {
              NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Success, "AI DIAGNOSIS", "Success"));
              currentECGRecord = res;
              ecgToUse = currentECGRecord;
              features = new Features();
              features.p = res.pExtracted;
              features.q = res.qExtracted;
              features.r = res.rExtracted;
              features.s = res.sExtracted;
              features.t = res.tExtracted;

              StartCoroutine(drawECGGraph(features));
              StartCoroutine(PopulateView());
              ClearScrollContent();
              List<string> diag = currentECGRecord.machineDiagnosisKey.OfType<string>().ToList();
              List<float> proba = currentECGRecord.machineDiagnosisProba.OfType<float>().ToList();
              List<bool> approved = currentECGRecord.machineDiagnosisBoolean.OfType<bool>().ToList();
              for (int i = 0; i < diag.Count; i++)
              {
                  AddMachineDiagnostic(i, diag[i], "" + ((int)proba[i]) + "%", approved[i]);
              }
              bpm.text = currentECGRecord.bpm;
              humanDiagnosis.text = currentECGRecord.humanDiagnosis;
          }));


    }


    public IEnumerator GenerateDiagnosisNew(bool fromAllECG = false, string patientID = null)
    {
        oldSelectedPatientID = "";
        LoadingView.instance.ShowLoading(true);
        ECGView.instance.associateToPatient.enabled = false;
        Debug.Log("From ALL ECG: " + fromAllECG + "\t PatientID = " + patientID);
        if (patientID == "P0001")
        {
            oldSelectedPatientID = "P0001";
            selectedPatientID = "P0001";
            ECGView.instance.associateToPatient.enabled = true;
        }

        // Generate Diagnosis, once everything is done, then populate view.

        cancel.GetComponent<LUI_UIAnimManager>().newAnimator = patientViewAnimator;
        cancel.GetComponent<LUI_UIAnimManager>().newPanel = patientViewPanel;
        applyChanges.GetComponent<LUI_UIAnimManager>().newAnimator = patientViewAnimator;
        applyChanges.GetComponent<LUI_UIAnimManager>().newPanel = patientViewPanel;

        if (fromAllECG)
        {
            cancel.GetComponent<LUI_UIAnimManager>().newAnimator = allECGsAnimator;
            cancel.GetComponent<LUI_UIAnimManager>().newPanel = allECGsPanel;
            applyChanges.GetComponent<LUI_UIAnimManager>().newAnimator = allECGsAnimator;
            applyChanges.GetComponent<LUI_UIAnimManager>().newPanel = allECGsPanel;
        }

        int RRint = int.Parse(currentECGForPatient.numberOfRRIntervals);
        List<float> dt = currentECGForPatient.data.OfType<float>().ToList();
        int fs = int.Parse(currentECGForPatient.fs);
        List<int> p = currentECGForPatient.pExtracted.OfType<int>().ToList();
        List<int> q = currentECGForPatient.qExtracted.OfType<int>().ToList();
        List<int> r = currentECGForPatient.rExtracted.OfType<int>().ToList();
        List<int> s = currentECGForPatient.sExtracted.OfType<int>().ToList();
        List<int> t = currentECGForPatient.tExtracted.OfType<int>().ToList();
        //yield return StartCoroutine(Requests.instance.UpdateDiagnosis(p, q, r, s, t, currentECGID, (res) =>
        //      {
        NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Success, "AI DIAGNOSIS", "Success"));
        EcgRecords res = currentECGForPatient;
        currentECGRecord = res;
        ecgToUse = currentECGForPatient;
        features = new Features();
        features.p = res.pExtracted;
        features.q = res.qExtracted;
        features.r = res.rExtracted;
        features.s = res.sExtracted;
        features.t = res.tExtracted;

        StartCoroutine(drawECGGraphNew(features));
        StartCoroutine(PopulateViewNew());
        ClearScrollContent();
        List<string> diag = currentECGForPatient.machineDiagnosisKey.OfType<string>().ToList();
        List<float> proba = currentECGForPatient.machineDiagnosisProba.OfType<float>().ToList();
        List<bool> approved = currentECGForPatient.machineDiagnosisBoolean.OfType<bool>().ToList();
        for (int i = 0; i < diag.Count; i++)
        {
            AddMachineDiagnostic(i, diag[i], "" + ((int)proba[i]) + "%", approved[i]);
        }
        bpm.text = currentECGRecord.bpm;
        humanDiagnosis.text = currentECGForPatient.humanDiagnosis;
        //}));
        yield return null;

    }


    public IEnumerator GenerateECGViewForUnconfirmed()
    {
        oldSelectedPatientID = "";
        LoadingView.instance.ShowLoading(true);

        ECGView.instance.associateToPatient.enabled = false;
        // Generate Diagnosis, once everything is done, then populate view.

        cancel.GetComponent<LUI_UIAnimManager>().newAnimator = unconfirmedAnimator;
        cancel.GetComponent<LUI_UIAnimManager>().newPanel = unconfirmedPanel;
        applyChanges.GetComponent<LUI_UIAnimManager>().newAnimator = unconfirmedAnimator;
        applyChanges.GetComponent<LUI_UIAnimManager>().newPanel = unconfirmedPanel;



        int RRint = int.Parse(currentECGForPatient.numberOfRRIntervals);
        List<float> dt = currentECGForPatient.data.OfType<float>().ToList();
        int fs = int.Parse(currentECGForPatient.fs);
        List<int> p = currentECGForPatient.pExtracted.OfType<int>().ToList();
        List<int> q = currentECGForPatient.qExtracted.OfType<int>().ToList();
        List<int> r = currentECGForPatient.rExtracted.OfType<int>().ToList();
        List<int> s = currentECGForPatient.sExtracted.OfType<int>().ToList();
        List<int> t = currentECGForPatient.tExtracted.OfType<int>().ToList();

        NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Success, "AI DIAGNOSIS", "Success"));
        currentECGRecord = currentECGForPatient;
        EcgRecords res = currentECGRecord;
        features = new Features();
        features.p = res.pExtracted;
        features.q = res.qExtracted;
        features.r = res.rExtracted;
        features.s = res.sExtracted;
        features.t = res.tExtracted;

        StartCoroutine(drawECGGraphNew(features));
        StartCoroutine(PopulateViewNew());
        ClearScrollContent();
        List<string> diag = currentECGForPatient.machineDiagnosisKey.OfType<string>().ToList();
        List<float> proba = currentECGForPatient.machineDiagnosisProba.OfType<float>().ToList();
        List<bool> approved = currentECGForPatient.machineDiagnosisBoolean.OfType<bool>().ToList();
        for (int i = 0; i < diag.Count; i++)
        {
            AddMachineDiagnostic(i, diag[i], "" + ((int)proba[i]) + "%", approved[i]);
        }
        bpm.text = currentECGRecord.bpm;
        humanDiagnosis.text = currentECGForPatient.humanDiagnosis;
        yield return null;
        LoadingView.instance.ShowLoading(false);


    }



    public IEnumerator PopulateView()
    {
        bool isMedicalProf = (CurrentUserModel.instance.userInfo.role.ToUpper() == Config.PROFESSIONAL_NAME ? true : false);
        humanDiagnosis.interactable = isMedicalProf;



        dateAndTime.text = currentECG.dateOfAcquisition + " at " + currentECG.timeOfAcquisition;
        bpm.text = "Beats per minute";
        samplingFreq.text = currentECG.fs;
        yield return null;

    }

    public IEnumerator PopulateViewNew()
    {
        bool isMedicalProf = (CurrentUserModel.instance.userInfo.role.ToUpper() == Config.PROFESSIONAL_NAME ? true : false);
        humanDiagnosis.interactable = isMedicalProf;



        dateAndTime.text = currentECGForPatient.dateOfAcquisition + " at " + currentECGForPatient.timeOfAcquisition;
        bpm.text = "Beats per minute";
        samplingFreq.text = currentECGForPatient.fs;
        yield return null;

    }

    private IEnumerator drawECGGraph(Features f)
    {

        //ECGInfo info = ECGInfo.CreateFromJSON();
        List<float> lst = currentECG.data.OfType<float>().ToList(); // this isn't going to be fast.

        GraphController.instance.ShowGraph(lst, f, -1, (int _i) => "" + (_i + 1), (float _f) => "" + _f.ToString("0.00"));
        LoadingView.instance.ShowLoading(false);
        yield return null;
    }

    private IEnumerator drawECGGraphNew(Features f)
    {
        //LoadingView.instance.ShowLoading(true);
        //yield return new WaitForSeconds(1f);
        //LoadingView.instance.ShowLoading(false);
        //ECGInfo info = ECGInfo.CreateFromJSON();
        List<float> lst = currentECGForPatient.data.OfType<float>().ToList(); // this isn't going to be fast.

        GraphController.instance.ShowGraph(lst, f, -1, (int _i) => "" + (_i + 1), (float _f) => "" + _f.ToString("0.00"));
        LoadingView.instance.ShowLoading(false);
        yield return null;
    }

    public void ClearScrollContent()
    {
        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public void AddMachineDiagnostic(int index, string diagnostic, string proba, bool approved)
    {

        //ClearContent();

        GameObject machineDiag = Instantiate(machineDiagPrefab);

        machineDiag.name = "Machine Diag";
        Text diag = machineDiag.transform.Find("Diagnosis").GetComponent<Text>();
        Text probab = machineDiag.transform.Find("Proba").GetComponent<Text>();
        Toggle approve = machineDiag.transform.Find("ApprovedToggle").GetComponent<Toggle>();
        diag.text = diagnostic;
        probab.text = proba;
        approve.isOn = approved;

        machineDiag.transform.SetParent(content.transform);
        machineDiag.GetComponent<RectTransform>().localScale = Vector3.one;
        machineDiag.GetComponent<RectTransform>().localPosition = new Vector3(machineDiag.GetComponent<RectTransform>().localPosition.x, machineDiag.GetComponent<RectTransform>().localPosition.y, 0);
        Destroy(machineDiag.GetComponent<LUI_UIAnimManager>());
        if (CurrentUserModel.instance.userInfo.role != Config.PROFESSIONAL_NAME)
        {
            machineDiag.GetComponent<Button>().interactable = false;
        }
        else
        {
            machineDiag.GetComponent<Button>().interactable = true;
        }
        Debug.Log(CurrentUserModel.instance.userInfo.role);
        machineDiag.GetComponent<Button>().onClick.AddListener(delegate
        {
            Debug.Log(CurrentUserModel.instance.userInfo.role);
            approve.isOn = !approve.isOn;
            currentECGRecord.machineDiagnosisBoolean[index] = approve.isOn;
        });


        machineDiag.GetComponent<RectTransform>().localPosition = new Vector3(machineDiag.GetComponent<RectTransform>().localPosition.x, machineDiag.GetComponent<RectTransform>().localPosition.y, 0);
    }

    public void HandleSaveButton()
    {

        applyChanges.onClick.AddListener(delegate
        {

            if (selectedPatientID == "")
            {
                string dummyPatientID = "P0001";
                StartCoroutine(Requests.instance.AssociateECGToPatient(currentECGID, dummyPatientID, CurrentUserModel.instance.userInfo.username, (res) =>
                {
                    string idToPass = currentECGID;
                    string beatsPerM = bpm.text;
                    List<int> pFeatures = features.p.OfType<int>().ToList();
                    List<int> qFeatures = features.q.OfType<int>().ToList();
                    List<int> rFeatures = features.r.OfType<int>().ToList();
                    List<int> sFeatures = features.s.OfType<int>().ToList();
                    List<int> tFeatures = features.t.OfType<int>().ToList();
                    List<string> diag = currentECGRecord.machineDiagnosisKey.OfType<string>().ToList();
                    List<float> proba = currentECGRecord.machineDiagnosisProba.OfType<float>().ToList();
                    List<bool> approved = currentECGRecord.machineDiagnosisBoolean.OfType<bool>().ToList();
                    string humanDiag = humanDiagnosis.text;
                    StartCoroutine(Requests.instance.UpdatePatientRecord(CurrentUserModel.instance.userInfo.username, idToPass, beatsPerM, pFeatures, qFeatures, rFeatures, sFeatures, tFeatures, diag, proba, approved, humanDiag, (resp) =>
                    {
                        currentECGID = "";
                        selectedPatientID = "";
                        NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Alert, "Save", resp.status));
                        StartCoroutine(Requests.instance.GetAllPatientlessECGs((response) =>
                        {
                            LoadingView.instance.ShowLoading(true);
                            EcgFetched[] respo = response;

                            for (int i = 0; i < respo.Length; i++)
                            {
                                PatientlessECGView.instance.AddPatientLessECG(respo[i].ecgId, respo[i].dateOfAcquisition, respo[i].timeOfAcquisition, respo[i]);
                            }
                            LoadingView.instance.ShowLoading(false);
                        }));

                        StartCoroutine(Requests.instance.GetAllPatientRecords((ress) =>
                        {
                            EcgRecords[] rec = ress;
                            for (int i = 0; i < rec.Length; i++)
                            {
                                if (rec[i].humanDiagnosis == "")
                                {
                                    UnconfirmedECGView.instance.AddUnconfirmedECG(rec[i], "Patient", rec[i].dateOfAcquisition, rec[i].timeOfAcquisition, "");
                                }
                            }
                            LoadingView.instance.ShowLoading(false);
                        }));
                    }));




                }));
                NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Success, "SUCCESS", "ASSOCIATED TO DUMMY PATIENT"));
            }
            else
            {

                LoadingView.instance.ShowLoading(true);
                // if from patientless
                PatientlessECGView.instance.ClearContent();

                //if from patientView


                //if from practitioner
                UnconfirmedECGView.instance.ClearContent();

                Debug.Log("Associating with: " + selectedPatientID);
                if (oldSelectedPatientID == "P0001" && selectedPatientID != "P0001")
                {

                    string idToPass = currentECGID;
                    string beatsPerM = bpm.text;
                    List<int> pFeatures = features.p.OfType<int>().ToList();
                    List<int> qFeatures = features.q.OfType<int>().ToList();
                    List<int> rFeatures = features.r.OfType<int>().ToList();
                    List<int> sFeatures = features.s.OfType<int>().ToList();
                    List<int> tFeatures = features.t.OfType<int>().ToList();
                    List<string> diag = currentECGRecord.machineDiagnosisKey.OfType<string>().ToList();
                    List<float> proba = currentECGRecord.machineDiagnosisProba.OfType<float>().ToList();
                    List<bool> approved = currentECGRecord.machineDiagnosisBoolean.OfType<bool>().ToList();
                    string humanDiag = humanDiagnosis.text;
                    StartCoroutine(Requests.instance.UpdatePatientRecordPatient(idToPass, selectedPatientID, (resp) =>
                     {
                         currentECGID = "";
                         selectedPatientID = "";
                         oldSelectedPatientID = "";
                         NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Alert, "Save", resp.status));
                         StartCoroutine(Requests.instance.GetAllPatientlessECGs((response) =>
                         {
                             LoadingView.instance.ShowLoading(true);
                             EcgFetched[] respo = response;

                             for (int i = 0; i < respo.Length; i++)
                             {
                                 PatientlessECGView.instance.AddPatientLessECG(respo[i].ecgId, respo[i].dateOfAcquisition, respo[i].timeOfAcquisition, respo[i]);
                             }
                             LoadingView.instance.ShowLoading(false);
                         }));

                         StartCoroutine(Requests.instance.GetAllPatientRecords((ress) =>
                         {
                             EcgRecords[] rec = ress;
                             for (int i = 0; i < rec.Length; i++)
                             {
                                 if (rec[i].humanDiagnosis == "")
                                 {
                                     UnconfirmedECGView.instance.AddUnconfirmedECG(rec[i], "Patient", rec[i].dateOfAcquisition, rec[i].timeOfAcquisition, "");
                                 }
                             }
                             LoadingView.instance.ShowLoading(false);
                         }));
                     }));

                }
                if (oldSelectedPatientID == "")
                {
                    StartCoroutine(Requests.instance.AssociateECGToPatient(currentECGID, selectedPatientID, CurrentUserModel.instance.userInfo.username, (res) =>
                    {
                        string idToPass = currentECGID;
                        string beatsPerM = bpm.text;
                        List<int> pFeatures = features.p.OfType<int>().ToList();
                        List<int> qFeatures = features.q.OfType<int>().ToList();
                        List<int> rFeatures = features.r.OfType<int>().ToList();
                        List<int> sFeatures = features.s.OfType<int>().ToList();
                        List<int> tFeatures = features.t.OfType<int>().ToList();
                        List<string> diag = currentECGRecord.machineDiagnosisKey.OfType<string>().ToList();
                        List<float> proba = currentECGRecord.machineDiagnosisProba.OfType<float>().ToList();
                        List<bool> approved = currentECGRecord.machineDiagnosisBoolean.OfType<bool>().ToList();
                        string humanDiag = humanDiagnosis.text;
                        StartCoroutine(Requests.instance.UpdatePatientRecord(CurrentUserModel.instance.userInfo.username, idToPass, beatsPerM, pFeatures, qFeatures, rFeatures, sFeatures, tFeatures, diag, proba, approved, humanDiag, (resp) =>
                        {
                            currentECGID = "";
                            selectedPatientID = "";
                            NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Alert, "Save", resp.status));
                            StartCoroutine(Requests.instance.GetAllPatientlessECGs((response) =>
                            {
                                LoadingView.instance.ShowLoading(true);
                                EcgFetched[] respo = response;

                                for (int i = 0; i < respo.Length; i++)
                                {
                                    PatientlessECGView.instance.AddPatientLessECG(respo[i].ecgId, respo[i].dateOfAcquisition, respo[i].timeOfAcquisition, respo[i]);
                                }
                                LoadingView.instance.ShowLoading(false);
                            }));

                            StartCoroutine(Requests.instance.GetAllPatientRecords((ress) =>
                            {
                                EcgRecords[] rec = ress;
                                for (int i = 0; i < rec.Length; i++)
                                {
                                    if (rec[i].humanDiagnosis == "")
                                    {
                                        UnconfirmedECGView.instance.AddUnconfirmedECG(rec[i], "Patient", rec[i].dateOfAcquisition, rec[i].timeOfAcquisition, "");
                                    }
                                }
                                LoadingView.instance.ShowLoading(false);
                            }));
                        }));




                    }));
                }
            }
            LoadingView.instance.ShowLoading(false);
        });

    }

    public void HandleUpdateDiagnosis()
    {
        updateDiag.onClick.AddListener(delegate
        {
            // int RRint = int.Parse(currentECGForPatient.numberOfRRIntervals);
            //  List<float> dt = currentECGForPatient.data.OfType<float>().ToList();
            //int fs = int.Parse(currentECGForPatient.fs);
            List<int> p = features.p.OfType<int>().ToList();
            List<int> q = features.q.OfType<int>().ToList();
            List<int> r = features.r.OfType<int>().ToList();
            List<int> s = features.s.OfType<int>().ToList();
            List<int> t = features.t.OfType<int>().ToList();
            StartCoroutine(Requests.instance.UpdateDiagnosis(p, q, r, s, t, currentECGID, (res) =>
                  {
                      NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Success, "AI DIAGNOSIS", "Success"));

                      currentECGForPatient = res;
                      currentECGRecord = res;
                      currentECGForPatient.data = res.data;
                      features = new Features();
                      features.p = res.pExtracted;
                      features.q = res.qExtracted;
                      features.r = res.rExtracted;
                      features.s = res.sExtracted;
                      features.t = res.tExtracted;

                      StartCoroutine(drawECGGraphNew(features));
                      StartCoroutine(PopulateViewNew());
                      ClearScrollContent();
                      List<string> diag = currentECGForPatient.machineDiagnosisKey.OfType<string>().ToList();
                      List<float> proba = currentECGForPatient.machineDiagnosisProba.OfType<float>().ToList();
                      List<bool> approved = currentECGForPatient.machineDiagnosisBoolean.OfType<bool>().ToList();
                      for (int i = 0; i < diag.Count; i++)
                      {
                          AddMachineDiagnostic(i, diag[i], "" + ((int)proba[i]) + "%", approved[i]);
                      }
                      bpm.text = currentECGRecord.bpm;
                      humanDiagnosis.text = currentECGForPatient.humanDiagnosis;
                  }));
        });
    }
}

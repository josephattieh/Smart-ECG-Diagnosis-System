using System;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class WorkflowController : MonoBehaviour
{
    public static WorkflowController instance;
    public GameObject authReqPanel;
    public GameObject allUsersPanel;
    public GameObject confirmDiagnosis;
    public GameObject viewAllUsersPanel;
    public GameObject allECGsPanel;
    public Button settingsBtn;
    public Button allPatientsButton;
    public Button patientlessECGButton;
    public Button associatePatient;
    public Button exitAppButton;
    public Button historyButton;
    public Button statsButton;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        populateProfileInSettings();
        handleAdminAuth();
        handleAdminAllUsers();
        handleGetAllUsers();
        handleGetAllECGS();
        handleGetHistory();
        handleGetStats();
        GetAllPatientsHandler();
        GetAllPatientlessECGRecords();
        GetAllPatientsECGHandler();
        GetAllUnconfirmedECGHandler();


        exitAppButton.onClick.AddListener(delegate
        {
            Debug.Log("Kbasneha");
            Application.Quit();
        });
    }

    private void populateProfileInSettings()
    {
        settingsBtn.onClick.AddListener(() =>
        {

            SettingsView.instance.PopulateView();
        });
    }

    private void GetAllUnconfirmedECGHandler()
    {
        confirmDiagnosis.GetComponentInChildren<Button>().onClick.AddListener(delegate
        {
            UnconfirmedECGView.instance.ClearContent();
            StartCoroutine(Requests.instance.GetAllPatientRecords((res) =>
            {
                EcgRecords[] rec = res;
                for (int i = 0; i < rec.Length; i++)
                {
                    if (rec[i].humanDiagnosis == "")
                    {
                        UnconfirmedECGView.instance.AddUnconfirmedECG(rec[i], "Patient", rec[i].dateOfAcquisition, rec[i].timeOfAcquisition, "");
                    }
                }
            }));
        });
    }

    private void handleGetAllECGS()
    {
        allECGsPanel.GetComponentInChildren<Button>().onClick.AddListener(delegate
        {
            AllECGView.instance.ClearContent();
            StartCoroutine(Requests.instance.SearchECG("", "", "", (res) =>
               {
                   EcgRecords[] rec = res;
                   for (int i = 0; i < rec.Length; i++)
                   {
                       AllECGView.instance.AddECG(rec[i].patientId, rec[i].ecgId, "ok", rec[i].dateOfAcquisition, rec[i].timeOfAcquisition, rec[i].humanDiagnosis, res[i]);
                   }
               }));
        });
    }


    private void handleAdminAuth()
    {
        authReqPanel.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            AuthReqView.instance.ClearContent();
            StartCoroutine(Requests.instance.GetAllUserRequests((response) =>
            {
                LoadingView.instance.ShowLoading(true);
                AccountRequests[] res = response;

                for (int i = 0; i < res.Length; i++)
                {
                    AuthReqView.instance.AddReq(res[i].name, res[i].username, res[i].role, res[i].roleDescription);
                }
                LoadingView.instance.ShowLoading(false);
            }));
        });
    }

    private void handleGetStats()
    {
        statsButton.onClick.AddListener(delegate
        {
            StartCoroutine(Requests.instance.GetStatsImage(1, (res) =>
            {
                byte[] data = Convert.FromBase64String(res.image_bytes);

                Texture2D tex = new Texture2D(350, 300);
                tex.LoadImage(data);
                
                StatsView.instance.stats1.texture = tex;

            }));

            StartCoroutine(Requests.instance.GetStatsImage(2, (res) =>
            {
                byte[] data = Convert.FromBase64String(res.image_bytes);

                Texture2D tex = new Texture2D(350, 300);
                tex.LoadImage(data);

                StatsView.instance.stats2.texture = tex;

            }));

            StartCoroutine(Requests.instance.GetStatsImage(3, (res) =>
            {
                
                byte[] data = Convert.FromBase64String(res.image_bytes);

                Texture2D tex = new Texture2D(350, 300);
                tex.LoadImage(data);

                StatsView.instance.stats3.texture = tex;

            }));

            StartCoroutine(Requests.instance.GetStatsImage(4, (res) =>
            {
                byte[] data = Convert.FromBase64String(res.image_bytes);

                Texture2D tex = new Texture2D(350, 300);
                tex.LoadImage(data);

                StatsView.instance.stats4.texture = tex;

            }));

            StartCoroutine(Requests.instance.GetStatsImage(5, (res) =>
            {
                byte[] data = Convert.FromBase64String(res.image_bytes);

                Texture2D tex = new Texture2D(350, 300);
                tex.LoadImage(data);

                StatsView.instance.stats5.texture = tex;

            }));

            StartCoroutine(Requests.instance.GetStatsImage(6, (res) =>
            {
                byte[] data = Convert.FromBase64String(res.image_bytes);

                Texture2D tex = new Texture2D(350, 300);
                tex.LoadImage(data);

                StatsView.instance.stats6.texture = tex;

            }));
        });
    }

    public void GetAllPatientsHandler()
    {
        allPatientsButton.onClick.AddListener(() =>
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

    private void GetAllPatientsECGHandler()
    {
        associatePatient.onClick.AddListener(() =>
        {
            AllPatientsViewECG.instance.ClearContent();
            StartCoroutine(Requests.instance.GetAllPatients((response) =>
            {
                LoadingView.instance.ShowLoading(true);
                Patient[] res = response;

                for (int i = 0; i < res.Length; i++)
                {
                    AllPatientsViewECG.instance.AddPatientForECG(res[i].patientId, res[i].name, res[i].gender, res[i].bloodType, res[i].diseases[0]);
                }
                LoadingView.instance.ShowLoading(false);
            }));
        });
    }

    private void handleGetAllUsers()
    {
        viewAllUsersPanel.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            ViewAllUsersView.instance.ClearContent();
            StartCoroutine(Requests.instance.GetAllUserAccounts((response) =>
            {
                AccountRequests[] res = response;
                LoadingView.instance.ShowLoading(true);

                for (int i = 0; i < response.Length; i++)
                {
                    ViewAllUsersView.instance.AddUser(res[i].name, res[i].username, res[i].role, res[i].email);
                }

                LoadingView.instance.ShowLoading(false);
            }));
        });
    }

    private void handleAdminAllUsers()
    {
        allUsersPanel.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            AllUsersView.instance.ClearContent();
            StartCoroutine(Requests.instance.GetUsersRoleChange((response) =>
            {
                LoadingView.instance.ShowLoading(true);

                for (int i = 0; i < response.Length; i++)
                {
                    string newRole = response[i].newRole;
                    StartCoroutine(Requests.instance.GetAccountDetails(response[i].username, (res) =>
                    {

                        AllUsersView.instance.AddUser(res[0].name, res[0].username, res[0].role, newRole);
                    }));
                }

                LoadingView.instance.ShowLoading(false);
            }));
        });
    }

    private void GetAllPatientlessECGRecords()
    {
        patientlessECGButton.onClick.AddListener(() =>
        {
            PatientlessECGView.instance.ClearContent();
            StartCoroutine(Requests.instance.GetAllPatientlessECGs((response) =>
            {
                LoadingView.instance.ShowLoading(true);
                EcgFetched[] res = response;

                for (int i = 0; i < res.Length; i++)
                {
                    PatientlessECGView.instance.AddPatientLessECG(res[i].ecgId, res[i].dateOfAcquisition, res[i].timeOfAcquisition, res[i]);
                }
                LoadingView.instance.ShowLoading(false);
            }));
        });
    }

    private void handleGetHistory()
    {
        historyButton.onClick.AddListener(delegate
        {
            HistoryView.instance.ClearContent();
            LoadingView.instance.ShowLoading(true);
            StartCoroutine(Requests.instance.GetHistory(CurrentUserModel.instance.userInfo.username, (res) =>
            {
                for (int i = 0; i < res.Length; i++)
                {
                    HistoryView.instance.AddHistory(res[i].user, res[i].day, res[i].time, res[i].message);
                }
            }));
            LoadingView.instance.ShowLoading(false);
        });
    }
}

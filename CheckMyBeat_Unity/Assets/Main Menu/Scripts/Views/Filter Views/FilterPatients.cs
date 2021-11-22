using UnityEngine;
using UnityEngine.UI;
using Views;

public class FilterPatients : MonoBehaviour
{
    public static FilterPatients instance;
    public InputField fullName;
    public CustomDropdown diseases;
    public CustomDropdown gender;
    public CustomDropdown bloodType;
    public Button apply;
    public Button cancel;

    private void Awake()
    {
        instance = this;
    }

    public void Clear()
    {
        fullName.text = "";
        gender.SetSelectedOption("OTHER");
        diseases.SetSelectedOption("");
        bloodType.SetSelectedOption("");
    }

    private void Start()
    {
        apply.onClick.AddListener(delegate
        {
            StartCoroutine(Requests.instance.SearchPatients("", fullName.text, gender.GetSelectedOption(), bloodType.GetSelectedOption(), diseases.GetSelectedOption(), (response) =>
                {
                    AllPatientsView.instance.ClearContent();

                    LoadingView.instance.ShowLoading(true);
                    Patient[] res = response;

                    for (int i = 0; i < res.Length; i++)
                    {
                        AllPatientsView.instance.AddPatient(res[i].patientId, res[i].name, res[i].gender, res[i].bloodType, res[i].diseases[0]);
                    }
                    LoadingView.instance.ShowLoading(false);
                    gameObject.SetActive(false);
                }));
        });

        cancel.onClick.AddListener(delegate
        {
            StartCoroutine(Requests.instance.SearchPatients("", "", "", "", "", (response) =>
            {
                AllPatientsView.instance.ClearContent();

                LoadingView.instance.ShowLoading(true);
                Patient[] res = response;

                for (int i = 0; i < res.Length; i++)
                {
                    AllPatientsView.instance.AddPatient(res[i].patientId, res[i].name, res[i].gender, res[i].bloodType, res[i].diseases[0]);
                }
                LoadingView.instance.ShowLoading(false);
                gameObject.SetActive(false);
            }));
        });
    }
}

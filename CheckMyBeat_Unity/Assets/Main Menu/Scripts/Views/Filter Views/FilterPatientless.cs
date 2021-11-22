using UnityEngine;
using UnityEngine.UI;
using Views;

public class FilterPatientless : MonoBehaviour
{
    public static FilterPatientless instance;
    public InputField date;
    public Button apply;
    public Button cancel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        apply.onClick.AddListener(delegate
        {
            PatientlessECGView.instance.ClearContent();
            StartCoroutine(Requests.instance.SearchPatientlessECG(date.text, (response) =>
             {
                 LoadingView.instance.ShowLoading(true);
                 EcgFetched[] res = response;

                 for (int i = 0; i < res.Length; i++)
                 {
                     PatientlessECGView.instance.AddPatientLessECG(res[i].ecgId, res[i].dateOfAcquisition, res[i].timeOfAcquisition, res[i]);
                 }
                 LoadingView.instance.ShowLoading(false);
                 gameObject.SetActive(false);
             }));
        });

        cancel.onClick.AddListener(delegate
        {
            PatientlessECGView.instance.ClearContent();
            StartCoroutine(Requests.instance.SearchPatientlessECG("", (response) =>
            {
                LoadingView.instance.ShowLoading(true);
                EcgFetched[] res = response;

                for (int i = 0; i < res.Length; i++)
                {
                    PatientlessECGView.instance.AddPatientLessECG(res[i].ecgId, res[i].dateOfAcquisition, res[i].timeOfAcquisition, res[i]);
                }
                LoadingView.instance.ShowLoading(false);
                gameObject.SetActive(false);
            }));

        });
    }

    public void Clear()
    {
        date.text = "";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class FilterUserRequests : MonoBehaviour
{
    public static FilterUserRequests instance;
    public InputField fullname;
    public InputField username;
    public InputField email;
    public InputField dob;
    public CustomDropdown gender;
    public CustomDropdown profession;
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
            AuthReqView.instance.ClearContent();
            StartCoroutine(Requests.instance.SearchUserAccountRequests(username.text, email.text, fullname.text, gender.GetSelectedOption(), dob.text, profession.GetSelectedOption(), (response) =>
            {
                AccountRequests[] res = response;
                LoadingView.instance.ShowLoading(true);

                for (int i = 0; i < response.Length; i++)
                {
                    AuthReqView.instance.AddReq(res[i].name, res[i].username, res[i].role, res[i].roleDescription);
                }

                LoadingView.instance.ShowLoading(false);
                FilterUserRequests.instance.gameObject.SetActive(false);
            }));

        });


        cancel.onClick.AddListener(delegate
        {
            AuthReqView.instance.ClearContent();
            StartCoroutine(Requests.instance.SearchUserAccountRequests("", "", "", "", "", "", (response) =>
            {
                AccountRequests[] res = response;
                LoadingView.instance.ShowLoading(true);

                for (int i = 0; i < response.Length; i++)
                {
                    AuthReqView.instance.AddReq(res[i].name, res[i].username, res[i].role, res[i].roleDescription);
                }

                LoadingView.instance.ShowLoading(false);
                AuthReqView.instance.gameObject.SetActive(false);

            }));

        });

    }

    public void Clear()
    {
        fullname.text = "";
        username.text = "";
        email.text = "";
        dob.text = "";
        gender.SetSelectedOption("OTHER");
        profession.SetSelectedOption("OPERATOR");
    }

}

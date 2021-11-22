using UnityEngine;
using UnityEngine.UI;
using Views;

public class FilterAllUsers : MonoBehaviour
{
    public static FilterAllUsers instance;
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
            ViewAllUsersView.instance.ClearContent();
            StartCoroutine(Requests.instance.SearchUsers(username.text, email.text, fullname.text, gender.GetSelectedOption(), dob.text, profession.GetSelectedOption(), (response) =>
            {
                AccountRequests[] res = response;
                LoadingView.instance.ShowLoading(true);

                for (int i = 0; i < response.Length; i++)
                {
                    ViewAllUsersView.instance.AddUser(res[i].name, res[i].username, res[i].role, res[i].email);
                }

                LoadingView.instance.ShowLoading(false);
                FilterAllUsers.instance.gameObject.SetActive(false);
            }));
          
        });


        cancel.onClick.AddListener(delegate
        {
            ViewAllUsersView.instance.ClearContent();
            StartCoroutine(Requests.instance.SearchUsers("", "", "", "", "", "", (response) =>
            {
                AccountRequests[] res = response;
                LoadingView.instance.ShowLoading(true);

                for (int i = 0; i < response.Length; i++)
                {
                    ViewAllUsersView.instance.AddUser(res[i].name, res[i].username, res[i].role, res[i].email);
                }

                LoadingView.instance.ShowLoading(false);
                FilterAllUsers.instance.gameObject.SetActive(false);

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
        gender.SetSelectedOption("OPERATOR");
    }

}

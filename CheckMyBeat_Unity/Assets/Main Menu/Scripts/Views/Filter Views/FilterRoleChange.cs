using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class FilterRoleChange : MonoBehaviour
{
    public static FilterRoleChange instance;
    public InputField username;
    public CustomDropdown newRole;
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
            AllUsersView.instance.ClearContent();
            StartCoroutine(Requests.instance.SearchRoleChangeRequests(username.text, newRole.GetSelectedOption(), "", (response) =>
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
                   StartCoroutine(turnOffFilter());
               }));
        });

        cancel.onClick.AddListener(delegate
        {
            AllUsersView.instance.ClearContent();
            StartCoroutine(Requests.instance.SearchRoleChangeRequests("", "", "", (response) =>
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
                StartCoroutine(turnOffFilter());
            }));
        });
    }

    private IEnumerator turnOffFilter()
    {
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);
    }

    public void Clear()
    {
        username.text = "";
        newRole.SetSelectedOption("");
    }
}

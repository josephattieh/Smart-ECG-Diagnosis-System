using Models;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class AllUsersView : MonoBehaviour
{
    public static AllUsersView instance;
    public GameObject content;
    public GameObject userObj;
    public Animator allUsersAnim;
    public Animator userAnim;
    public GameObject userPanel;

    public InputField fullNameIF;
    public InputField usernameIF;
    public InputField oldRoleIF;
    public InputField newRoleIF;

    public Button apply;
    public Button deny;
    public Button cancel;

    public Button filterButton;
    public GameObject filterRoleChangePanel;

    public string currentUser = "";


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
    private void Start()
    {
        SaveUserRoleHandler();
        DenyUserRoleHandler();
        filterButton.onClick.AddListener(delegate
        {
            filterRoleChangePanel.SetActive(true);
            FilterRoleChange.instance.Clear();
        });
    }

    public void AddUser(string name, string user, string role, string roleDes)
    {
        GameObject userObject = Instantiate(userObj);

        userObject.name = "Auth Req";
        Text fullName = userObject.transform.Find("FullName").GetComponent<Text>();
        Text userName = userObject.transform.Find("UserName").GetComponent<Text>();
        Text prof = userObject.transform.Find("Role").GetComponent<Text>();
        Text roleDesc = userObject.transform.Find("RoleDesc").GetComponent<Text>();
        fullName.text = name;
        userName.text = user;
        prof.text = role;
        roleDesc.text = roleDes;
        userObject.transform.SetParent(content.transform);
        userObject.GetComponent<RectTransform>().localScale = Vector3.one;
        userObject.GetComponent<RectTransform>().localPosition = new Vector3(userObject.GetComponent<RectTransform>().localPosition.x, userObject.GetComponent<RectTransform>().localPosition.y, 0);
        LUI_UIAnimManager anim = userObject.GetComponent<LUI_UIAnimManager>();
        anim.oldAnimator = allUsersAnim;
        anim.newAnimator = userAnim;
        anim.newPanel = userPanel;
        anim.animButton = userObject.GetComponent<Button>();
        userObject.GetComponent<Button>().onClick.AddListener(delegate
        {
            currentUser = user;
            fullNameIF.text = name;
            usernameIF.text = user;
            oldRoleIF.text = role;
            newRoleIF.text = roleDes;
        });
        userObject.GetComponent<RectTransform>().localPosition = new Vector3(userObject.GetComponent<RectTransform>().localPosition.x, userObject.GetComponent<RectTransform>().localPosition.y, 0);
    }

    private void SaveUserRoleHandler()
    {

        apply.onClick.AddListener(delegate
        {
            StartCoroutine(Requests.instance.ApproveRoleChange(currentUser, CurrentUserModel.instance.userInfo.username, (resp) =>
              {
                  NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Alert, "Approved Change", resp.status));
                  AllUsersView.instance.ClearContent();
                  StartCoroutine(Requests.instance.GetAllUserRequests((response) =>
                  {
                      LoadingView.instance.ShowLoading(true);
                      AccountRequests[] res = response;

                      for (int i = 0; i < res.Length; i++)
                      {
                          AllUsersView.instance.AddUser(res[i].name, res[i].username, res[i].role, res[i].roleDescription);
                      }
                      LoadingView.instance.ShowLoading(false);
                  }));
              }));
        });
    }
    private void DenyUserRoleHandler()
    {

        deny.onClick.AddListener(delegate
        {
            StartCoroutine(Requests.instance.DenyRoleChange(currentUser, CurrentUserModel.instance.userInfo.username, (resp) =>
            {
                NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Alert, "Denied Change", resp.status));
                AllUsersView.instance.ClearContent();
                StartCoroutine(Requests.instance.GetAllUserRequests((response) =>
                {
                    LoadingView.instance.ShowLoading(true);
                    AccountRequests[] res = response;

                    for (int i = 0; i < res.Length; i++)
                    {
                        AllUsersView.instance.AddUser(res[i].name, res[i].username, res[i].role, res[i].roleDescription);
                    }
                    LoadingView.instance.ShowLoading(false);
                }));
            }));
        });
    }
}

using Models;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class AuthReqView : MonoBehaviour
{
    public static AuthReqView instance;
    public GameObject content;
    public GameObject reqObj;
    public Animator authReqAnim;
    public Animator decisionAnim;
    public GameObject decisionPanel;
    public Button approve;
    public Button reject;
    public Button filterBtn;
    public GameObject filterUserReqsPanel;

    public string currentUser = "";

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ApproveButtonHandler();
        RejectButtonHandler();
        filterBtn.onClick.AddListener(delegate
        {
            filterUserReqsPanel.SetActive(true);
            FilterUserRequests.instance.Clear();
        });
    }

    public void ClearContent()
    {
        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public void AddReq(string name, string user, string role, string roleDes)
    {

        //ClearContent();

        GameObject authReq = Instantiate(reqObj);

        authReq.name = "Auth Req";
        Text fullName = authReq.transform.Find("FullName").GetComponent<Text>();
        Text userName = authReq.transform.Find("UserName").GetComponent<Text>();
        Text prof = authReq.transform.Find("Role").GetComponent<Text>();
        Text roleDesc = authReq.transform.Find("RoleDesc").GetComponent<Text>();
        fullName.text = name;
        userName.text = user;
        prof.text = role;
        roleDesc.text = roleDes;
        authReq.transform.SetParent(content.transform);
        authReq.GetComponent<RectTransform>().localScale = Vector3.one;
        authReq.GetComponent<RectTransform>().localPosition = new Vector3(authReq.GetComponent<RectTransform>().localPosition.x, authReq.GetComponent<RectTransform>().localPosition.y, 0);
        LUI_UIAnimManager anim = authReq.GetComponent<LUI_UIAnimManager>();
        anim.oldAnimator = decisionAnim;
        anim.newAnimator = decisionAnim;
        anim.newPanel = decisionPanel;
        anim.animButton = authReq.GetComponent<Button>();
        authReq.GetComponent<Button>().onClick.AddListener(delegate
        {
            currentUser = user;
        });


        authReq.GetComponent<RectTransform>().localPosition = new Vector3(authReq.GetComponent<RectTransform>().localPosition.x, authReq.GetComponent<RectTransform>().localPosition.y, 0);
    }

    private void ApproveButtonHandler()
    {
        approve.onClick.AddListener(delegate
        {

            StartCoroutine(Requests.instance.ApproveUserCreated(CurrentUserModel.instance.userInfo.username,currentUser, (resp) =>
            {
                Status stat = resp;
                NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Success, "Approved", stat.status));

                StartCoroutine(Requests.instance.GetAllUserRequests((response) =>
                {
                    LoadingView.instance.ShowLoading(true);
                    AccountRequests[] res = response;
                    AuthReqView.instance.ClearContent();
                    for (int i = 0; i < res.Length; i++)
                    {
                        Debug.Log("Name: " + res[i].name);
                        AuthReqView.instance.AddReq(res[i].name, res[i].username, res[i].role, res[i].roleDescription);
                    }
                    LoadingView.instance.ShowLoading(false);
                }));
            }));

        });
    }

    private void RejectButtonHandler()
    {
        reject.onClick.AddListener(delegate
        {

            StartCoroutine(Requests.instance.RejectUserCreated(CurrentUserModel.instance.userInfo.username, currentUser, "not specified", (resp) =>
            {
                Status stat = resp;
                NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Success, "Rejected", stat.status));
                StartCoroutine(Requests.instance.GetAllUserRequests((response) =>
                {
                    LoadingView.instance.ShowLoading(true);
                    AccountRequests[] res = response;
                    AuthReqView.instance.ClearContent();
                    for (int i = 0; i < res.Length; i++)
                    {
                        Debug.Log("Name: " + res[i].name);
                        AuthReqView.instance.AddReq(res[i].name, res[i].username, res[i].role, res[i].roleDescription);
                    }
                    LoadingView.instance.ShowLoading(false);
                }));
            }));

        });
    }
}

using Models;
using UnityEngine;
using Views;

namespace Controllers
{
    public class LoginController : MonoBehaviour
    {
        public static LoginController instance;

        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            LoginView.instance.signIn.onClick.AddListener(delegate
            {
                SignIn();
            });
            LoginView.instance.signUp.onClick.AddListener(delegate
            {
                NavigationViewController.instance.ChangeFromLoginToSignUp();
            });
            LoginView.instance.signOut.onClick.AddListener(delegate
            {
                // Animation is done using animator
                NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.LockClosed, "Success", "Signed Out"));
            });

        }
        public void SignIn()
        {
            LoadingView.instance.ShowLoading(true);
            StartCoroutine(Requests.instance.SignIn(LoginView.instance.username.text, LoginView.instance.password.text, (response) =>
             {
                 AccountRequests res = response;
                 NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Alert, "Status", res.status));
                 if (res.username.Length > 0)
                 {
                     NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.LockOpen, "Success", "Welcome Back, " + res.name + " !"));
                     NavigationViewController.instance.NavigateFromLoginToMainPanel();
                     updateUser(new UserModel("", res.name, res.username, res.dateOfBirth, res.roleDescription, res.role, res.password, res.gender.ToUpper(), null));
                 }
                 LoadingView.instance.ShowLoading(false);
             }));
        }

        public void updateUser(UserModel model)
        {
            CurrentUserModel.instance.updateCurrentUser(model);
            WorkflowController.instance.authReqPanel.SetActive(CurrentUserModel.instance.userInfo.role.ToUpper() == Config.ADMIN_NAME ? true : false);
            WorkflowController.instance.allUsersPanel.SetActive(CurrentUserModel.instance.userInfo.role.ToUpper() == Config.ADMIN_NAME ? true : false);
            WorkflowController.instance.confirmDiagnosis.SetActive(CurrentUserModel.instance.userInfo.role.ToUpper() == Config.PROFESSIONAL_NAME ? true : false);
        }
    }
}
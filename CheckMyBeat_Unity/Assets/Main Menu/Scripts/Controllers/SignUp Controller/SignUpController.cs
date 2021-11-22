using UnityEngine;
using Views;

namespace Controllers
{
    public class SignUpController : MonoBehaviour
    {
        public static SignUpController instance;

        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            SignUpView.instance.signUp.onClick.AddListener(delegate
            {
                SignUp();
            });

            SignUpView.instance.back.onClick.AddListener(delegate
            {
                NavigationViewController.instance.ChangeFromSignUpToLogin();
            });
            SignUpView.instance.signUp.onClick.AddListener(delegate
            {
                NavigationViewController.instance.ChangeFromSignUpToLogin();
            });
        }

        public void SignUp()
        {
            // Fill the model first
            // Do the DB stuff here.
        }

    }
}
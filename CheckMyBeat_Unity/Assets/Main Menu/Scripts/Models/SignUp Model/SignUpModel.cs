using System;
using System.Globalization;
using UnityEngine;
using Views;

namespace Models
{
    public class SignUpModel : MonoBehaviour
    {
        public static SignUpModel instance;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            SignUpView.instance.username.onValueChanged.AddListener(delegate
            {
                StartCoroutine(Requests.instance.ValidateUsername(SignUpView.instance.username.text, (response) =>
                {
                    SignUpView.instance.usernameFeedback.text = response.status;
                }));
            });

            SignUpView.instance.email.onValueChanged.AddListener(delegate
            {
                StartCoroutine(Requests.instance.ValidateEmail(SignUpView.instance.email.text, (response) =>
                {
                    SignUpView.instance.emailFeedback.text = response.status;
                }));
            });

            SignUpView.instance.signUp.onClick.AddListener(delegate
            {
                SignUpView view = SignUpView.instance;
                string errors = "";
                if (view.username.text.Length < 2)
                {
                    errors += "Username can not be less than 2 characters.\n";
                }
                if (view.usernameFeedback.text.ToLower().Contains("invalid"))
                {
                    errors += "Username already exists.\n";
                }
                if (view.password.text.Length < 3)
                {
                    errors += "Password can not be less than 3 characters.\n";
                }
                if (view.fullName.text.Length < 3)
                {
                    errors += "Full Name can not be less than 3 characters.\n";
                }
                if (view.customProfessionDropdown.GetComponent<CustomDropdown>().GetSelectedOption() == null)
                {
                    errors += "Specify a profession.\n";
                }
                if (view.customGenderDropdown.GetComponent<CustomDropdown>().GetSelectedOption() == "MALE")
                {
                    errors += "Specify a gender.\n";
                }
                if (view.emailFeedback.text.ToLower().Contains("invalid"))
                {
                    errors += "Enter a valid email.\n";
                }
                DateTime dt;
                bool isValid = DateTime.TryParseExact(view.dateOfBirth.text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
                if (!isValid)
                {
                    errors += "Date of Birth should be DD/MM/YYYY \n";
                }
                if (errors.Length > 0)
                {
                    NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Alert, "Error", "Kindly fix your login details"));
                }
                else
                {
                    StartCoroutine(Requests.instance.SignUp(view.username.text, view.password.text, view.email.text, view.fullName.text, view.customGenderDropdown.GetComponent<CustomDropdown>().GetSelectedOption(), view.dateOfBirth.text, view.customProfessionDropdown.GetComponent<CustomDropdown>().GetSelectedOption(), view.description.text, "picture test", (response) =>
                        {
                            string status = response.status;
                            NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Alert, "Status", status));
                        }));
                }
            });
        }

        public bool ValidateInput()
        {
            return true;
        }

    }
}


using UnityEngine;
using Views;

namespace Models
{
    public class LoginModel : MonoBehaviour
    {
        public static LoginModel instance;

        private void Awake()
        {
            instance = this;
        }

        public bool ValidateInput()
        {

            if (LoginView.instance.username.text.Length < 3)
            {
                return false;
            }
            if (LoginView.instance.password.text.Length < 6)
            {
                return false;
            }
            //if (LoginView.instance.customDropDown.GetComponent<CustomDropdown>().GetSelectedOption() == null)
            //{
            //    return false;
            //}
            return true;
        }

    }
}

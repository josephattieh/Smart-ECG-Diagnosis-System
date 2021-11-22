using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class LoginView : MonoBehaviour
    {
        public static LoginView instance;
        public InputField username;
        public InputField password;
        public Button signIn;
        public Button signUp;
        public Button signOut;

        private void Awake()
        {
            instance = this;
        }

        public void Clear()
        {
            username.text = "";
            password.text = "";
        }
    }
}

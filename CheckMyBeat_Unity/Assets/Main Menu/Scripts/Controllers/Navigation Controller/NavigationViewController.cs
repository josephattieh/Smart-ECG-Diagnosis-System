using UnityEngine;
namespace Views
{
    public class NavigationViewController : MonoBehaviour
    {

        [Header("Panel Animators")]
        public Animator mainPageAnim;
        public Animator loginPanelAnim;
        public GameObject loginPanel;
        public GameObject signUpPanel;
        
        [Header("Panels Associated with ECG with their resp. Animators")]
        public GameObject ecgPanel;
        public Animator ecgPanelAnim;
        public GameObject ecgTypeSelectPanel;
        public Animator ecgTypeSelectAnim;
        public GameObject ecgGeneratorPanel;
        public Animator ecgGeneratorAnim;
        public GameObject ecgResultPanel;
        public Animator ecgResultAnim;


        public static NavigationViewController instance;


        public void MoveToECGResulView()
        {
            ecgGeneratorAnim.Play("Panel Closing");
            ecgTypeSelectAnim.Play("Panel Closing");
            ecgResultPanel.SetActive(true);
            ecgResultAnim.Play("Panel Opening Standart");
        }

        public void CancelECGResulView()
        {
            ecgResultAnim.Play("Panel Closing");
            ecgPanelAnim.Play("Panel Fading FG");

        }

        public void NavigateFromLoginToMainPanel()
        {
            loginPanelAnim.SetBool("hideLogin", true);
            loginPanelAnim.SetBool("showLogin", false);
            mainPageAnim.SetBool("hideLogin", true);
        }
        public void NavigateToLogin()
        {
            loginPanelAnim.SetBool("hideLogin", false);
            loginPanelAnim.SetBool("showLogin", true);
            mainPageAnim.SetBool("hideLogin", false);
        }
        public void ChangeFromLoginToSignUp()
        {
            SignUpView.instance.Clear();
            loginPanelAnim.enabled = false;
            loginPanel.GetComponent<CanvasGroup>().alpha = 0;
            signUpPanel.GetComponent<CanvasGroup>().alpha = 1;
            signUpPanel.GetComponent<CanvasGroup>().interactable = true;
            signUpPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        public void ChangeFromSignUpToLogin()
        {
            loginPanelAnim.enabled = true;
            loginPanel.GetComponent<CanvasGroup>().alpha = 1;
            signUpPanel.GetComponent<CanvasGroup>().alpha = 0;
            signUpPanel.GetComponent<CanvasGroup>().interactable = false;
            signUpPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void NavigateFromSpashToLoginScreen()
        {
            loginPanelAnim.SetBool("showLogin", true);
        }

        public void NavigateToPatientsScreen()
        {

        }


        private void Awake()
        {
            instance = this;
        }
    }
}

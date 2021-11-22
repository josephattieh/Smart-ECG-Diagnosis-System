using UnityEngine;
using UnityEngine.UI;

public class SignUpView : MonoBehaviour
{
    private Sprite baseImageSprite;

    public static SignUpView instance;
    public Image profilePic;
    public InputField fullName;
    public InputField username;
    public InputField password;
    public InputField email;
    public GameObject customGenderDropdown;
    public GameObject customProfessionDropdown;
    public InputField description;
    public InputField dateOfBirth;
    public Button signUp;
    public Button back;
    public Text usernameFeedback;
    public Text emailFeedback;

    private void Awake()
    {
        instance = this;
        baseImageSprite = profilePic.sprite;

    }

    public void Clear()
    {
        fullName.text = "";
        username.text = "";
        password.text = "";
        dateOfBirth.text = "";
        description.text = "";
        email.text = "";
        profilePic.sprite = baseImageSprite;
        usernameFeedback.text = "";
        emailFeedback.text = "";
        //customGenderDropdown.GetComponent<CustomDropdown>().SetSelectedOption("Male");
        //customProfessionDropdown.GetComponent<CustomDropdown>().SetSelectedOption("Admi");

    }
}

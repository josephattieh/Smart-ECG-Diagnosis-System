using Models;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class SettingsView : MonoBehaviour
{
    public static SettingsView instance;
    public InputField fullName;
    public InputField oldPassword;
    public InputField username;
    public InputField newPassword;
    public GameObject profession;
    public GameObject gender;
    public InputField dob;
    public InputField extraInf;
    public Button apply;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ApplyButtonHandle();
    }

    public void PopulateView()
    {
        fullName.text = CurrentUserModel.instance.userInfo.fullName;
        oldPassword.text = CurrentUserModel.instance.userInfo.password;
        username.text = CurrentUserModel.instance.userInfo.username;
        newPassword.text = "";
        profession.GetComponent<CustomDropdown>().SetSelectedOption(CurrentUserModel.instance.userInfo.role.ToUpper());
        gender.GetComponent<CustomDropdown>().SetSelectedOption(CurrentUserModel.instance.userInfo.gender.ToUpper());
        dob.text = CurrentUserModel.instance.userInfo.dateOfBirth;
        extraInf.text = CurrentUserModel.instance.userInfo.roleDescription;
    }

    private void ApplyButtonHandle()
    {

        apply.onClick.AddListener(delegate
        {
            if (CurrentUserModel.instance.userInfo.username != "admin")
            {

                CustomDropdown role = profession.GetComponent<CustomDropdown>();
                if (role.GetSelectedOption().ToUpper() != CurrentUserModel.instance.userInfo.role.ToUpper())
                {
                    StartCoroutine(Requests.instance.RequestRoleChange(CurrentUserModel.instance.userInfo.username, role.GetSelectedOption().ToUpper(), "not specified", (resp) =>
                      {
                          NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Alert, "Change Role", resp.status));
                      }));
                }
                string newPass = "";
                if (newPassword.text != "")
                {
                    newPass = newPassword.text;
                }
                else
                {
                    newPass = CurrentUserModel.instance.userInfo.password;
                }
                UserModel user = CurrentUserModel.instance.userInfo;
                string newGender = gender.GetComponent<CustomDropdown>().GetSelectedOption().ToUpper();

                StartCoroutine(Requests.instance.UpdateAccountDetails(user.username, newPass, "enter email", user.fullName, newGender, dob.text, role.GetSelectedOption().ToUpper(), extraInf.text, "picture", (res) =>
                        {
                            NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Alert, "Profile Update", res.status));
                        }));
                CurrentUserModel.instance.userInfo.gender = newGender;
                CurrentUserModel.instance.userInfo.password = newPass;
                CurrentUserModel.instance.userInfo.roleDescription = extraInf.text;
                CurrentUserModel.instance.userInfo.dateOfBirth = dob.text;
            }
        });
    }
}

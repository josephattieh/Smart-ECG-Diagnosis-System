using UnityEngine;
using Views;

public enum UserType
{
    Administrator,
    Operator,
    Professional,
    Other
}
public class CurrentUserModel : MonoBehaviour
{
    public static CurrentUserModel instance;
    public UserModel userInfo;

    private void Awake()
    {
        instance = this;
    }

    public void ClearUser()
    {
        this.userInfo = null;
    }

    public void updateCurrentUser(UserModel newUserInfo)
    {
        this.userInfo = newUserInfo;
    }

    public void SignOut()
    {
        Debug.Log("Sign Out");
        ClearUser();
        LoginView.instance.Clear();
        NavigationViewController.instance.NavigateToLogin();
    }

}

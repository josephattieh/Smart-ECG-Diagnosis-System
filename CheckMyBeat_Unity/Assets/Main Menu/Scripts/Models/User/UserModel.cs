using UnityEngine;

public class UserModel
{
    public string id;
    public string fullName;
    public string username;
    public string dateOfBirth;
    public string role;
    public string roleDescription;
    public Sprite profilePic;
    public string password;
    public string gender;

    public UserModel(string id, string fullName, string username, string dob, string desc, string role, string password, string gender, Sprite profilePic)
    {
        this.id = id;
        this.fullName = fullName;
        this.username = username;
        this.dateOfBirth = dob;
        this.role = role;
        this.roleDescription = desc;
        this.password = password;
        this.gender = gender;
        this.profilePic = profilePic;
    }
    // Just for test
    public UserModel()
    {
        this.id = "1";
        this.fullName = "Guy Abi Hanna";
        this.dateOfBirth = "17/08/1997";
        this.role = Config.ADMIN_NAME;
        this.roleDescription = "Test";
        this.password = "12345";
        this.username = "guyabihanna";
        this.gender = "Male";
        this.profilePic = null;
    }

}

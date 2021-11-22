using UnityEngine;
using UnityEngine.UI;
using Views;

public class ViewAllUsersView : MonoBehaviour
{
    public static ViewAllUsersView instance;
    
    public GameObject content;
    public GameObject userObj;
    public Button filterBtn;
    public GameObject filterAllUsersPanel;
    
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        filterBtn.onClick.AddListener(delegate
        {
            filterAllUsersPanel.SetActive(true);
            FilterAllUsers.instance.Clear();
        });
    }
    public void ClearContent()
    {
        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
   
    public void AddUser(string name, string user, string role, string roleDes)
    {
        GameObject userObject = Instantiate(userObj);

        userObject.name = "User";
        Text fullName = userObject.transform.Find("FullName").GetComponent<Text>();
        Text userName = userObject.transform.Find("UserName").GetComponent<Text>();
        Text prof = userObject.transform.Find("Role").GetComponent<Text>();
        Text roleDesc = userObject.transform.Find("RoleDesc").GetComponent<Text>();
        fullName.text = name;
        userName.text = user;
        prof.text = role;
        roleDesc.text = roleDes;
        userObject.transform.SetParent(content.transform);
        userObject.GetComponent<RectTransform>().localScale = Vector3.one;
        userObject.GetComponent<RectTransform>().localPosition = new Vector3(userObject.GetComponent<RectTransform>().localPosition.x, userObject.GetComponent<RectTransform>().localPosition.y, 0);
        Destroy(userObject.GetComponent<LUI_UIAnimManager>());
        
        userObject.GetComponent<RectTransform>().localPosition = new Vector3(userObject.GetComponent<RectTransform>().localPosition.x, userObject.GetComponent<RectTransform>().localPosition.y, 0);
    }
    

}

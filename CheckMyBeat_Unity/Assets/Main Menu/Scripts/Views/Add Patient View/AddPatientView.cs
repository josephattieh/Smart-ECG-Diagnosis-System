using Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class AddPatientView : MonoBehaviour
{
    public static AddPatientView instance;
    public InputField fullName;
    public CustomDropdown bloodType;
    public CustomDropdown diseases;
    public CustomDropdown gender;
    public Button back;
    public Button apply;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        back.onClick.AddListener(delegate
        {
            Clear();
        });
        ApplyButtonHandler();
    }
    public void Clear()
    {
        fullName.text = "";
        bloodType.SetSelectedOption("Unidentified");
        diseases.SetSelectedOption("NO RISKS");
        gender.SetSelectedOption("Other");
    }

    public void ApplyButtonHandler()
    {
        apply.onClick.AddListener(delegate
        {
            string name = fullName.text;
            string blood = bloodType.GetSelectedOption().ToUpper();
            string dis = diseases.GetSelectedOption();
            string gen = gender.GetSelectedOption().ToUpper();
            StartCoroutine(Requests.instance.AddPatient(CurrentUserModel.instance.userInfo.username,gen, name, "picture", blood, new List<string>() { dis,"unknown disease" }, (resp) =>
             {
                 NotificationView.instance.ShowNotification(new NotificationModel(NotificationSprite.Success, "Add Patient", resp.status));
             }));
            Clear();
        });
    }

}

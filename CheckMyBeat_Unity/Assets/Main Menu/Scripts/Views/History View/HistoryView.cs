using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class HistoryView : MonoBehaviour
{
    public static HistoryView instance;
    public GameObject content;
    public GameObject historyPrefab;
  
    private void Awake()
    {
        instance = this;
    }
    public void ClearContent()
    {
        foreach (Transform child in content.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public void AddHistory(string user, string date, string time, string message)
    {

        //ClearContent();

        GameObject historySummary = Instantiate(historyPrefab);

        historySummary.name = "History";
        Text fullName = historySummary.transform.Find("FullName").GetComponent<Text>();
        Text userName = historySummary.transform.Find("UserName").GetComponent<Text>();
        Text prof = historySummary.transform.Find("Role").GetComponent<Text>();
        Text roleDesc = historySummary.transform.Find("RoleDesc").GetComponent<Text>();
        fullName.text = user;
        userName.text = date;
        prof.text = time;
        roleDesc.text = message;
        historySummary.transform.SetParent(content.transform);
        historySummary.GetComponent<RectTransform>().localScale = Vector3.one;
        historySummary.GetComponent<RectTransform>().localPosition = new Vector3(historySummary.GetComponent<RectTransform>().localPosition.x, historySummary.GetComponent<RectTransform>().localPosition.y, 0);

        Destroy(historySummary.GetComponent<LUI_UIAnimManager>());
        historySummary.GetComponent<RectTransform>().localPosition = new Vector3(historySummary.GetComponent<RectTransform>().localPosition.x, historySummary.GetComponent<RectTransform>().localPosition.y, 0);
    }
}

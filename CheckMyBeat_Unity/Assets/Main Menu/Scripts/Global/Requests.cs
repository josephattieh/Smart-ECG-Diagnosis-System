using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Requests : MonoBehaviour
{
    public static Requests instance;

    public void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //List<string> diseases = new List<string>()
        //{
        //    "disease1","disease2"};

        //Debug.Log(formatArray(diseases));
        // StartCoroutine(GetRequest("http://127.0.0.1:5000/users/request/list"));
    }
    private string formatArray(List<string> arr)
    {
        string str = "[ ";
        for (int i = 0; i < arr.Count; i++)
        {
            str += "\"" + arr[i] + "\"";
            if (i != arr.Count - 1)
            {
                str += " ,";
            }
        }
        str += " ]";
        return str;
    }
    private string formatDataArray(List<int> arr)
    {
        string str = "[ ";
        for (int i = 0; i < arr.Count; i++)
        {
            str += "" + arr[i] + "";
            if (i != arr.Count - 1)
            {
                str += " ,";
            }
        }
        str += " ]";
        return str;
    }
    private string formatDataFloatArray(List<float> arr)
    {
        string str = "[ ";
        for (int i = 0; i < arr.Count; i++)
        {
            str += "" + arr[i] + "";
            if (i != arr.Count - 1)
            {
                str += " ,";
            }
        }
        str += " ]";
        return str;
    }
    private string formatDataBoolArray(List<bool> arr)
    {
        string str = "[ ";
        for (int i = 0; i < arr.Count; i++)
        {
            str += "\"" + arr[i] + "\"";
            if (i != arr.Count - 1)
            {
                str += " ,";
            }
        }
        str += " ]";
        return str;
    }


    public IEnumerator ValidateUsername(string username, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/username";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"username\":\"" + username + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator ValidateEmail(string email, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/email";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"email\":\"" + email + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator SignIn(string username, string password, Action<AccountRequests> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/authenticate";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"username\":\"" + username + "\",";
        json += "\"password\":\"" + password + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            AccountRequests response = JsonConvert.DeserializeObject<AccountRequests>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }
    public IEnumerator GetAllUserRequests(Action<AccountRequests[]> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/request/list";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            AccountRequests[] response = JsonConvert.DeserializeObject<AccountRequests[]>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }


    public IEnumerator GetAllUserAccounts(Action<AccountRequests[]> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/account/list";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            AccountRequests[] response = JsonConvert.DeserializeObject<AccountRequests[]>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator GetUsersRoleChange(Action<RoleChangeRequest[]> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/rolechange/list";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            RoleChangeRequest[] response = JsonConvert.DeserializeObject<RoleChangeRequest[]>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator GetPatientByID(string id, Action<Patient[]> callback = null)
    {
        string url = "http://127.0.0.1:5000/patients/get";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"patientId\":\"" + id + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Patient[] response = JsonConvert.DeserializeObject<Patient[]>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator GetAllPatients(Action<Patient[]> callback = null)
    {
        string url = "http://127.0.0.1:5000/patients/list";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Patient[] response = JsonConvert.DeserializeObject<Patient[]>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator GetAllPatientlessECGs(Action<EcgFetched[]> callback = null)
    {
        string url = "http://127.0.0.1:5000/ecgs/patientless/list";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("Cache-Control", "no-cache");
        uwr.SetRequestHeader("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
        uwr.SetRequestHeader("Postman-Token", "163476fc-5220-d299-7b91-8fbe5fa24cad");
        uwr.SetRequestHeader("Accept", "*/*");
        uwr.SetRequestHeader("Accept-Encoding", "gzip, deflate, sdch");
        uwr.SetRequestHeader("Accept-Language", "en-US,en;q=0.8,ja;q=0.6");

        //Send the request then wait here until it returns

        yield return uwr.SendWebRequest();


        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            StartCoroutine(GetAllPatientlessECGs(callback));
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            EcgFetched[] response = JsonConvert.DeserializeObject<EcgFetched[]>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator GetAllPatientRecords(Action<EcgRecords[]> callback = null)
    {
        string url = "http://127.0.0.1:5000/ecgs/patientrecords/list";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            try
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
                EcgRecords[] response = JsonConvert.DeserializeObject<EcgRecords[]>(uwr.downloadHandler.text);
                callback?.Invoke(response);
            }
            catch (Exception e)
            {
                StartCoroutine(GetAllPatientRecords(callback));
            }
        }
    }

    public IEnumerator EmptyDatabase(Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/empty";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator SignUp(string username, string password, string email, string fullName, string gender, string dob, string role, string roleDesc, string pic, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/request/add";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"username\":\"" + username + "\",";
        json += "\"password\":\"" + password + "\",";
        json += "\"email\":\"" + email + "\",";
        json += "\"name\":\"" + fullName + "\",";
        json += "\"gender\":\"" + gender + "\",";
        json += "\"dateOfBirth\":\"" + dob + "\",";
        json += "\"role\":\"" + role + "\",";
        json += "\"roleDescription\":\"" + roleDesc + "\",";
        json += "\"picture\":\"" + pic + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator RejectUserCreated(string currentUser, string username, string reason, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/request/reject";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"username\":\"" + username + "\",";
        json += "\"reasonRejected\":\"" + reason + "\",";
        json += "\"myUsername\":\"" + currentUser + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator UpdateDiagnosis(List<int> p, List<int> q, List<int> r, List<int> s, List<int> t, string ecgID, Action<EcgRecords> callback = null)
    {
        string url = "http://127.0.0.1:5000/ecg/patientrecords/getdiagnosis/features";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "";
        json += "{\"pExtracted\":" + formatDataArray(p) + ",";
        json += "\"qExtracted\":" + formatDataArray(q) + ",";
        json += "\"rExtracted\":" + formatDataArray(r) + ",";
        json += "\"sExtracted\":" + formatDataArray(s) + ",";
        json += "\"tExtracted\":" + formatDataArray(t) + ",";
        json += "\"ecgId\":\"" + ecgID + "\"}";
        Debug.Log(json);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            EcgRecords response = JsonConvert.DeserializeObject<EcgRecords>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }


    public IEnumerator GenerateFirstDiagnosis(List<float> data, int numOfRR, int fs, Action<EcgRecords> callback = null)
    {
        string url = "http://127.0.0.1:5000/ecg/patientrecords/getdiagnosis/nofeatures";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"data\":" + formatDataFloatArray(data) + ",";
        json += "\"numberOfRRIntervals\":" + numOfRR + ",";
        json += "\"fs\":" + fs + "}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            EcgRecords response = JsonConvert.DeserializeObject<EcgRecords>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator ApproveUserCreated(string currentUser, string username, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/request/approve";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"username\":\"" + username + "\",";
        json += "\"myUsername\":\"" + currentUser + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator GetAccountDetails(string username, Action<AccountRequests[]> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/account/get";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"username\":\"" + username + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            AccountRequests[] response = JsonConvert.DeserializeObject<AccountRequests[]>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator UpdateAccountDetails(string username, string password, string email, string fullName, string gender, string dob, string role, string roleDesc, string pic, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/account/update";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"username\":\"" + username + "\",";
        json += "\"password\":\"" + password + "\",";
        json += "\"email\":\"" + email + "\",";
        json += "\"name\":\"" + fullName + "\",";
        json += "\"gender\":\"" + gender + "\",";
        json += "\"dateOfBirth\":\"" + dob + "\",";
        json += "\"role\":\"" + role + "\",";
        json += "\"roleDescription\":\"" + roleDesc + "\",";
        json += "\"picture\":\"" + pic + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator RequestRoleChange(string username, string newRole, string reason, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/rolechange/request";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"username\":\"" + username + "\",";
        json += "\"newRole\":\"" + newRole + "\",";
        json += "\"reason\":\"" + reason + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator DenyRoleChange(string regUser, string adminUser, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/rolechange/reject";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"username\":\"" + regUser + "\",";
        json += "\"reason\":\"" + "not specified" + "\",";
        json += "\"myUsername\":\"" + adminUser + "\"}";
        Debug.Log(json);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator ApproveRoleChange(string regUser, string adminUser, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/rolechange/approve";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"username\":\"" + regUser + "\",";
        json += "\"myUsername\":\"" + adminUser + "\"}";
        Debug.Log("SENT:");
        Debug.Log(json);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator AddPatient(string currentUser, string gender, string fullName, string pic, string bloodType, List<string> diseases, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/patients/add";
        var uwr = new UnityWebRequest(url, "POST");
        string id = Guid.NewGuid().ToString("N");
        string json = "{\"patientId\":\"" + id + "\",";
        json += "\"gender\":\"" + gender + "\",";
        json += "\"name\":\"" + fullName + "\",";
        json += "\"picture\":\"" + pic + "\",";
        json += "\"bloodType\":\"" + bloodType + "\",";
        json += "\"diseases\":" + formatArray(diseases) + ",";
        json += "\"myUsername\":\"" + currentUser + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator UpdatePatient(string currentUser, string id, string gender, string fullName, string pic, string bloodType, List<string> diseases, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/patients/update";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"patientId\":\"" + id + "\",";
        json += "\"gender\":\"" + gender + "\",";
        json += "\"name\":\"" + fullName + "\",";
        json += "\"picture\":\"" + pic + "\",";
        json += "\"bloodType\":\"" + bloodType + "\",";
        json += "\"diseases\":" + formatArray(diseases) + ",";
        json += "\"myUsername\":\"" + currentUser + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator DeletePatient(string currentUser, string id, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/patients/delete";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"patientId\":\"" + id + "\",";
        json += "\"myUsername\":\"" + currentUser + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator AddAnonymousECG(List<int> data, string fs, string picture, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/ecgs/patientless/add";
        var uwr = new UnityWebRequest(url, "POST");
        string id = Guid.NewGuid().ToString("N");
        string json = "{\"ecgId\":\"" + id + "\",";
        json += "\"data\":\"" + formatDataArray(data) + "\",";
        json += "\"fs\":\"" + fs + "\",";
        json += "\"picture\":\"" + picture + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }
    public IEnumerator GetPatientlessECGbyID(string id, Action<EcgFetched> callback = null)
    {
        string url = "http://127.0.0.1:5000/ecgs/patientless/get";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"ecgId\":\"" + id + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            EcgFetched response = JsonConvert.DeserializeObject<EcgFetched>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator DeletePatientlessECGbyID(string id, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/ecgs/patientless/delete";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"ecgId\":\"" + id + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator GetPatientRecords(string patientId, Action<EcgRecords[]> callback = null)
    {
        string url = "http://127.0.0.1:5000/ecgs/patientrecords/get/patient";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"patientId\":\"" + patientId + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            EcgRecords[] response = JsonConvert.DeserializeObject<EcgRecords[]>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator GetECGbyID(string id, Action<EcgRecords> callback = null)
    {
        string url = "http://127.0.0.1:5000/ecgs/patientrecords/get/ecg";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"ecgId\":\"" + id + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            EcgRecords response = JsonConvert.DeserializeObject<EcgRecords>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator DeleteECGbyID(string currentUser, string id, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/ecgs/patientrecords/delete";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"ecgId\":\"" + id + "\",";
        json += "\"myUsername\":\"" + currentUser + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }
    public IEnumerator AssociateECGToPatient(string ecgId, string patientID, string myUsername, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/ecgs/patientrecords/setpatient";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"ecgId\":\"" + ecgId + "\",";
        json += "\"patientId\":\"" + patientID + "\",";
        json += "\"myUsername\":\"" + myUsername + "\"}";
        Debug.Log(json);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator SearchUsers(string username, string email, string fullName, string gender, string dateOfBirth, string role, Action<AccountRequests[]> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/account/search/wild";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"username\":\"" + username + "\",";
        json += "\"email\":\"" + email + "\",";
        json += "\"name\":\"" + fullName + "\",";
        json += "\"gender\":\"" + gender + "\",";
        json += "\"dateOfBirth\":\"" + dateOfBirth + "\",";
        json += "\"role\":\"" + role + "\"}";

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            AccountRequests[] response = JsonConvert.DeserializeObject<AccountRequests[]>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
        Debug.Log("Okay");
    }

    public IEnumerator SearchUserAccountRequests(string username, string email, string fullName, string gender, string dateOfBirth, string role, Action<AccountRequests[]> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/request/search/wild";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"username\":\"" + username + "\",";
        json += "\"email\":\"" + email + "\",";
        json += "\"name\":\"" + fullName + "\",";
        json += "\"gender\":\"" + gender + "\",";
        json += "\"dateOfBirth\":\"" + dateOfBirth + "\",";
        json += "\"role\":\"" + role + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            AccountRequests[] response = JsonConvert.DeserializeObject<AccountRequests[]>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator SearchPatients(string patientID, string fullName, string gender, string bloodType, string diseases, Action<Patient[]> callback = null)
    {
        string url = "http://127.0.0.1:5000/patients/search/wild";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"patientId\":\"" + patientID + "\",";
        json += "\"gender\":\"" + gender + "\",";
        json += "\"name\":\"" + fullName + "\",";
        json += "\"bloodType\":\"" + bloodType + "\",";
        json += "\"diseases\":\"" + diseases + "\"}";
        Debug.Log("JSON is:");
        Debug.Log(json);
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Patient[] response = JsonConvert.DeserializeObject<Patient[]>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator SearchRoleChangeRequests(string username, string newRole, string reason, Action<RoleChangeRequest[]> callback = null)
    {
        string url = "http://127.0.0.1:5000/users/rolechange/search/wild";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"username\":\"" + username + "\",";
        json += "\"newRole\":\"" + newRole + "\",";
        json += "\"reason\":\"" + reason + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            RoleChangeRequest[] response = JsonConvert.DeserializeObject<RoleChangeRequest[]>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator SearchPatientlessECG(string date, Action<EcgFetched[]> callback = null)
    {
        string url = "http://127.0.0.1:5000/ecgs/patientless/search/wild";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"dateOfAcquisition\":\"" + date + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            EcgFetched[] response = JsonConvert.DeserializeObject<EcgFetched[]>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator SearchECG(string date, string diag, string humanDiag, Action<EcgRecords[]> callback = null)
    {
        string url = "http://127.0.0.1:5000/ecgs/patientrecords/search/wild";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"dateOfAcquisition\":\"" + date + "\",";
        json += "\"machineDiagnosisKey\":\"" + diag + "\",";
        json += "\"humanDiagnosis\":\"" + humanDiag + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            EcgRecords[] response = JsonConvert.DeserializeObject<EcgRecords[]>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator GetHistory(string username, Action<History[]> callback = null)
    {
        string url = "http://127.0.0.1:5000/history";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"username\":\"" + username + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            History[] response = JsonConvert.DeserializeObject<History[]>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }


    public IEnumerator UpdatePatientRecord(string currentUser, string ecgId, string bpm, List<int> p, List<int> q, List<int> r, List<int> s, List<int> t, List<string> machineDiag, List<float> machinePro, List<bool> machineBool, string humanDiag, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/ecgs/patientrecords/update";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"ecgId\":\"" + ecgId + "\",";
        json += "\"bpm\":\"" + bpm + "\",";
        json += "\"pExtracted\":" + formatDataArray(p) + ",";
        json += "\"qExtracted\":" + formatDataArray(q) + ",";
        json += "\"rExtracted\":" + formatDataArray(r) + ",";
        json += "\"sExtracted\":" + formatDataArray(s) + ",";
        json += "\"tExtracted\":" + formatDataArray(t) + ",";
        json += "\"machineDiagnosisKey\":" + formatArray(machineDiag) + ",";
        json += "\"machineDiagnosisProba\":" + formatDataFloatArray(machinePro) + ",";
        json += "\"machineDiagnosisBoolean\":" + formatDataBoolArray(machineBool) + ",";
        json += "\"humanDiagnosis\":\"" + humanDiag + "\",";
        json += "\"myUsername\":\"" + currentUser + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator UpdatePatientRecordPatient(string ecgId, string patientId, Action<Status> callback = null)
    {
        string url = "http://127.0.0.1:5000/ecgs/patientrecords/update/patient";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"ecgId\":\"" + ecgId + "\",";
        json += "\"patientId\":\"" + patientId + "\"}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("JSON is:");
        Debug.Log(json);
        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            Status response = JsonConvert.DeserializeObject<Status>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

    public IEnumerator GetStatsImage(int number, Action<ImageString> callback = null)
    {
        string url = "http://127.0.0.1:5000/statistics/figure/get";
        var uwr = new UnityWebRequest(url, "POST");
        string json = "{\"number\":" + number + "}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        Debug.Log("JSON is:");
        Debug.Log(json);
        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            ImageString response = JsonConvert.DeserializeObject<ImageString>(uwr.downloadHandler.text);
            callback?.Invoke(response);
        }
    }

}

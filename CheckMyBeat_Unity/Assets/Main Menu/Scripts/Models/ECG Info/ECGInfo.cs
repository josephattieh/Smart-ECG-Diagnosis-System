using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ECGInfo
{
    public string bpm;
    public string[] diagnosis;
    public float[] data;
    public int fs;
    public Features features;

    
    public static ECGInfo CreateFromJSON()
    {
        string path = @"C:\Users\dell\Downloads\data.json";
        string readText = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<ECGInfo>(readText);
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}
public class Features
{
    public int[] p;
    public int[] q;
    public int[] r;
    public int[] s;
    public int[] t;
}

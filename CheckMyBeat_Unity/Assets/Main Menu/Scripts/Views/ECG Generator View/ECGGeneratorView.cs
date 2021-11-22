using Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class ECGGeneratorView : MonoBehaviour
    {
        public List<GameObject> ecgInputFields;
        public GameObject ecgDefaultToggle;
        public static ECGGeneratorView instance;
        private bool isCustomValues;
        private void Awake()
        {
            instance = this;
            // We always start by using Custom Values
            isCustomValues = true;
            ecgDefaultToggle.GetComponent<Toggle>().onValueChanged.AddListener(delegate
            {
                isCustomValues = !isCustomValues;
                foreach (GameObject inputField in ecgInputFields)
                {
                    inputField.GetComponent<Animator>().enabled = isCustomValues;
                    inputField.GetComponent<InputField>().interactable = isCustomValues;
                }
            });
        }
        public bool isCustomValue()
        {
            return isCustomValues;
        }

        public ECGGeneratorData GetECGCustomData()
        {
            List<float> ecgValues = new List<float>();
            Debug.Log("input filed count: " + ecgInputFields.Count);
            for (int i = 0; i < ecgInputFields.Count; i++)
            {
                Debug.Log("i: " + i);
                ecgValues.Add(float.Parse(ecgInputFields[i].GetComponent<InputField>().text));
            }
            return new ECGGeneratorData(ecgValues[0], ecgValues[1], ecgValues[2], ecgValues[3], ecgValues[4], ecgValues[5], ecgValues[6], ecgValues[7], ecgValues[8], ecgValues[9], ecgValues[10], ecgValues[11], ecgValues[12], ecgValues[13], ecgValues[14], ecgValues[15]);
        }

    }
}
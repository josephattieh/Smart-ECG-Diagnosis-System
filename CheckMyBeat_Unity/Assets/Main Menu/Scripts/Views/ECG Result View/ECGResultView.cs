using Models;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class ECGResultView : MonoBehaviour
    {
        public static ECGResultView instance;
        public RawImage ecgImage;

        private void Awake()
        {
            instance = this;
        }

        public void CancelButtonClick()
        {
            NavigationViewController.instance.CancelECGResulView();
        }

        public void UpdateECGParams()
        {
            ecgImage.texture = ECGResultModel.instance.LoadECGTexture();
        }
    }
}
using UnityEngine;
using Views;

namespace Controllers
{
    public class ECGResultController : MonoBehaviour
    {
        public static ECGResultController instance;

        private void Awake()
        {
            instance = this;
        }

        public void Init()
        {
            // This is called whenever a new ECG Result is calculated
            ECGResultView.instance.UpdateECGParams();
        }
    }
}
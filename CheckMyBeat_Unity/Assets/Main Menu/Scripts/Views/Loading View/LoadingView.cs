using UnityEngine;
namespace Views
{
    public class LoadingView : MonoBehaviour
    {
        public static LoadingView instance;
        private CanvasGroup _canvasGroup;
        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            _canvasGroup = gameObject.GetComponent<CanvasGroup>();
        }

        public void ShowLoading(bool show)
        {
            _canvasGroup.alpha = show ? 1 : 0;
            _canvasGroup.interactable = show;
            _canvasGroup.blocksRaycasts = show;
        }

    }
}
using Models;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum NotificationSprite
{
    LockOpen, LockClosed, Success, Alert
}
namespace Views
{

    public class NotificationView : MonoBehaviour
    {
        [Header("Notification Prefab")]
        #region Prefab
        public Image notifImage;
        public Text notifTitle;
        public Text notifDescription;
        public Animator notifAnimator;
        #endregion


        [Header("Notification Images")]
        #region Notification Images
        public Sprite lockOpen;
        public Sprite lockClosed;
        public Sprite success;
        public Sprite alert;
        #endregion

        private bool _showingNotification;
        private Queue notificationQueue;

        public static NotificationView instance;

        private void Awake()
        {
            instance = this;
            _showingNotification = false;
            notificationQueue = new Queue();
        }

        public void ShowNotification(NotificationModel notifModel)
        {

            _ScheduleNotification(notifModel);
            if (notificationQueue.Count == 1)
            {
                StartCoroutine(_StartNotifications());
            }
            else
            {

            }

        }

        private void _ScheduleNotification(NotificationModel notifModel)
        {
            notificationQueue.Enqueue(notifModel);
        }

        private IEnumerator _StartNotifications()
        {

            yield return new WaitUntil(() => !_showingNotification);
            NotificationModel notifModel = (NotificationModel)notificationQueue.Dequeue();
            _showingNotification = true;
            notifImage.sprite = _GenerateSpriteFromType(notifModel.imageType);
            notifTitle.text = notifModel.title;
            notifDescription.text = notifModel.description;
            notifAnimator.enabled = true;
            notifAnimator.Play("Slipping Effect", -1, 0);
            StartCoroutine(_TurnOffCurrentNotification());
        }

        private IEnumerator _TurnOffCurrentNotification()
        {
            yield return new WaitForSeconds(notifAnimator.GetCurrentAnimatorStateInfo(0).length + notifAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            notifAnimator.enabled = false;
            notifImage.sprite = _GenerateSpriteFromType(NotificationSprite.Alert);
            notifTitle.text = "Notification";
            notifDescription.text = "";
            _showingNotification = false;
            yield return new WaitForSeconds(0.12f);
            if (notificationQueue.Count > 0)
            {
                StartCoroutine(_StartNotifications());
            }
        }

        private Sprite _GenerateSpriteFromType(NotificationSprite type)
        {
            switch (type)
            {
                case NotificationSprite.LockOpen:
                    return lockOpen;

                case NotificationSprite.LockClosed:
                    return lockClosed;

                case NotificationSprite.Success:
                    return success;

                case NotificationSprite.Alert:
                    return alert;

                default:
                    return success;

            }
        }

    }
}

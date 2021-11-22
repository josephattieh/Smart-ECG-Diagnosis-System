using System.Collections;
using UnityEngine;

namespace Views
{
    public class SplashScreenManager : MonoBehaviour
    {

        [Header("ANIMATORS")]
        public Animator splashAnimator;


        private void Start()
        {
            splashAnimator.GetComponent<Animator>();

            StartCoroutine(_DoneSplash());

        }

        private IEnumerator _DoneSplash()
        {
            yield return new WaitForSeconds(splashAnimator.GetCurrentAnimatorStateInfo(0).length + splashAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            NavigationViewController.instance.NavigateFromSpashToLoginScreen();
            gameObject.SetActive(false);
        }

    }
}

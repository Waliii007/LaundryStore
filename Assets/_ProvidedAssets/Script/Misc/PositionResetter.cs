using System;
using System.Collections;
using DG.Tweening;
using GameAnalyticsSDK.Setup;
using Invector.vCharacterController;
using Unity.VisualScripting;
using UnityEngine;

namespace LaundaryMan
{
    public class PositionResetter : MonoBehaviour
    {
        public GameObject playerPoint;
        public GameObject lookatPoint;
        public GameObject Tutorial;
        public Animator tutorialAnimator;
        public GameObject tutorialimage;
        public bool isDropper;

        private void OnEnable()
        {
        //    this.gameObject.SetActive(false);
            StartCoroutine(checkifTutorialComplete());
        }

        public IEnumerator checkifTutorialComplete()
        {

            tutorialAnimator.enabled = true;
            yield return new WaitUntil(() => ReferenceManager.Instance.GameData.isTutorialCompleted);
           this.gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") &&
                other.TryGetComponent(out vThirdPersonInput tpi) && !tpi.IsMoving())
            {
                other.transform.DOMove(playerPoint.transform.position,
                    1);
                other.transform.DODynamicLookAt(lookatPoint.transform.position,
                    1);
            }
        }
    }
}
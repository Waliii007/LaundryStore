using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LaundaryMan
{
    public class LoadingSystem : MonoBehaviour
    {
        private static readonly int DoorOpen = Animator.StringToHash("DoorOpen");
        public Animator doorAnimator;
        public static LoadingSystem instance;

        private void OnEnable()
        {
            instance = this;
            StartLoading("LaundryScene");
            DontDestroyOnLoad(this.gameObject);
        }

        void StartLoading(string sceneName)
        {
            SceneLoaders.SetActive(true);
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        IEnumerator LoadSceneCoroutine(string sceneName)
        {
            doorAnimator.SetBool(DoorOpen, true);
            yield return new WaitForSeconds(1.5f);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            while (asyncLoad is { isDone: false })
            {
                yield return null;
            }

            doorAnimator.SetBool(DoorOpen, false);
            DOVirtual.DelayedCall(1, () => { SceneLoaders.SetActive(false); });
        }

        public GameObject SceneLoaders;
    }
}
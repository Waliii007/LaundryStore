using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace LaundaryMan
{
    public class TutorialHandler : MonoBehaviour
    {
        public GameObject[] tasks;
        public int indexTask;
        public TutorialTask prevTask;
        public TutorialTask currentTask;

        private void Awake()
        {
            ReferenceManager.Instance.taskHandler.OnTaskUpdatedAction += TaskCompleted;
        }

        public void UnSubscribe()
        {
            ReferenceManager.Instance.taskHandler.OnTaskUpdatedAction -= TaskCompleted;
        }

        private void OnEnable()
        {
            if (!ReferenceManager.Instance.GameData.isTutorialCompleted)
            {
                TaskHandler(TutorialTask.UnlockLaundry);
            }
            else
            {
                UnSubscribe();
            }
        }

        public void TaskHandler(TutorialTask newTask)
        {
            prevTask = currentTask;
            currentTask = newTask;
            tasks[(int)prevTask].gameObject.SetActive(false);
            tasks[(int)currentTask].gameObject.SetActive(true);
        }

        public void TaskCompleted()
        {
            DOVirtual.DelayedCall(1, () =>
                ReferenceManager.Instance.tutorialHandler.TaskHandler(ReferenceManager.Instance.tutorialHandler
                    .currentTask + 1));
        }


        private void OnDisable()
        {
            ReferenceManager.Instance.taskHandler.OnTaskUpdatedAction -= TaskCompleted;
        }
    }
}

public enum TutorialTask
{
    UnlockLaundry,
    PickLaundry,
    DropLaundry,
    PickCleanLaundry,
    DropToPressLaundry,
    CheckOut,
    CollectCash
}
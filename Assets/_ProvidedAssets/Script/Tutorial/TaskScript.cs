using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace LaundaryMan
{
    public class TaskScript : MonoBehaviour
    {
        public string tutorialObjective;
        public Transform navmeshTarget;

        private void OnEnable()
        {
            ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.ObjectivePanel);
            ReferenceManager.Instance.objectivePanel.ShowObjective(tutorialObjective);

            TaskComplete();
            ReferenceManager.Instance.playerStackManager.pathDraw.destination = navmeshTarget;
            if (taskObject) taskObject.SetActive(true);
            
        }

        public GameObject taskObject;

        void TaskComplete()
        {

            switch (ReferenceManager.Instance.tutorialHandler.currentTask)
            {
                case TutorialTask.UnlockLaundry:

                    break;
                case TutorialTask.PickLaundry:
                    StartCoroutine(PickLaundary());
                 

                    break;
                case TutorialTask.DropLaundry:
                    StartCoroutine(DropLaundary());
                  

                    break;
                case TutorialTask.PickCleanLaundry:
                    StartCoroutine(PickLaundary());
                    break;
                case TutorialTask.DropToPressLaundry:
                    StartCoroutine(DropLaundary());
                    break;
                case TutorialTask.CheckOut:

                    break;
                case TutorialTask.CollectCash:
                    break;
            }

        }

        IEnumerator PickLaundary()
        {
            yield return new WaitUntil(() =>
                ReferenceManager.Instance.playerStackManager.ClothStack.Count >=
                ReferenceManager.CustomerAmountOfClothForTutorial);

            DOVirtual.DelayedCall(1, () =>
                ReferenceManager.Instance.tutorialHandler.TaskHandler(ReferenceManager.Instance.tutorialHandler
                    .currentTask + 1));
            if (taskObject) taskObject.gameObject.SetActive(false);

        }

        IEnumerator DropLaundary()
        {
            yield return new WaitUntil(() =>
                ReferenceManager.Instance.playerStackManager.ClothStack.Count <= 0);
            DOVirtual.DelayedCall(1, () =>
                ReferenceManager.Instance.tutorialHandler.TaskHandler(ReferenceManager.Instance.tutorialHandler
                    .currentTask + 1));
            if (taskObject) taskObject.gameObject.SetActive(false);

        }
    }
}
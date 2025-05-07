using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace LaundaryMan
{
    public class TaskScript : MonoBehaviour
    {
        [TextArea(2,6)]
        public string tutorialObjective;
        public Transform navmeshTarget;

        private void OnEnable()
        {
            ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.TutorialObjective);
            ReferenceManager.Instance.tutorialObjectivePanel.ShowObjective(tutorialObjective);

            TaskComplete();
            ReferenceManager.Instance.playerStackManager.pathDraw.destination = navmeshTarget;
            if (taskObject) taskObject.SetActive(true);
            TaskStart();
        }

        public GameObject taskObject;
        

        void TaskStart()
        {
            switch (ReferenceManager.Instance.tutorialHandler.currentTask)
            {
                case TutorialTask.UnlockLaundry:
                    break;
                case TutorialTask.PickLaundry:
                    break;
                case TutorialTask.DropLaundry:
                    break;
                case TutorialTask.PickCleanLaundry:
                    break;
                case TutorialTask.DropToPressLaundry:
                    break;
                case TutorialTask.CheckOut:
                    break;
                case TutorialTask.CollectCash:
                    break;
                case TutorialTask.RefillBuy:
                    break;
                case TutorialTask.RinseBuy:
                   
                    break;
                case TutorialTask.RefillDetergent:
                    WashingMachineDropper.totalDetergent = 0;
                    WashingMachineDropper.EmptyForTutorial();
                    ReferenceManager.Instance.playerStackManager.pathDraw.destination = navmeshTarget;
                    break;
                case TutorialTask.RefillRinse:
                    PressBasketHandler.EmptyForTutorial();
                    ReferenceManager.Instance.playerStackManager.pathDraw.destination = navmeshTarget;
                    PressBasketHandler.totalRin = 0;
                    ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(false);
                    break;
            }
        }

        public WashingMachineDropper WashingMachineDropper;
        public PressBasketHandler PressBasketHandler;

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
                case TutorialTask.RefillRinse:
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
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
        public GameObject[] someTriggerToOff;
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
        public void CompleteTutorial()
        {
            var reference = ReferenceManager.Instance;
            reference.tutorialHandler.TaskCompleted();

            reference.GameData.isTutorialCompleted = true;
            reference.taskHandler.HrHandler();
            reference.canvasManager.CanvasStateChanger(CanvasStates.ObjectivePanel);
            reference.objectivePanel.ShowObjective("Tutorial Complete");
            reference.playerStackManager.pathDraw.gameObject.SetActive(false);
            reference.playerStackManager.pathDraw.destination = null;
            foreach (var some in someTriggerToOff)
            {
                some.gameObject.SetActive(true);   
            }
            ReferenceManager.Instance.queueSystem.OnTutorialCompleted();
            reference.SaveGameDataObserver();
            StartCoroutine(washingMachineDropper.CheckCanvasStateRoutine());

        }
        public void TaskCompleted()
        {
            ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(false);
            DOVirtual.DelayedCall(.2f, () =>
                ReferenceManager.Instance.tutorialHandler.TaskHandler(ReferenceManager.Instance.tutorialHandler
                    .currentTask + 1));
            if (tasks[(int)currentTask].TryGetComponent<TaskScript>(out var taskScript) && taskScript.taskObject)
            {
                taskScript.taskObject.SetActive(false);
            }
        }
        public WashingMachineDropper washingMachineDropper;

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
    CollectCash,
    RefillBuy,
    RinseBuy,
    RefillDetergent,
    RefillRinse,
    
}
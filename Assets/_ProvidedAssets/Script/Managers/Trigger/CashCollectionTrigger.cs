using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace LaundaryMan
{
    public class CashCollectionTrigger : MonoBehaviour
    {
        public List<CashScript> cashStack = new();
        public Transform cashStackPointer;
        private bool isCollecting = false;
        public CheckoutHandler checkoutHandler;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isCollecting = true;
                StartCoroutine(CollectCash());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isCollecting = false;
            }
        }

        public GameObject triggerToOn;
        public bool once;

        private IEnumerator CollectCash()
        {
            if (!ReferenceManager.Instance.GameData.isTutorialCompleted && !once)
            {
                once = true;
                ReferenceManager.Instance.tutorialHandler.TaskCompleted();

                triggerToOn.SetActive(true);
            }

            while (isCollecting && cashStack.Count > 0)
            {
                CashScript cash = cashStack[0];
                cashStack.RemoveAt(0);

                if (cash)
                {
                    Vector3 targetPosition = cashStackPointer.position + new Vector3(0, cashStack.Count * -0.05f, 0);
                    cash.transform.DOMove(targetPosition, 0.42f).OnComplete(() =>
                    {
                        // ReferenceManager.Instance.GameData.playerCash += 50;

                        cash.transform.SetParent(cashStackPointer);
                        Destroy(cash.gameObject, 0.1f);
                    });

                    yield return new WaitForSeconds(0.1f);
                    ReferenceManager.Instance.SaveGameDataObserver();
                }
            }

            // Move reset logic outside the loop
            checkoutHandler.cashStackCount = 0;
            checkoutHandler.changeInY = 0.1f; // Reset properly to avoid floating cash
            ReferenceManager.Instance.GameData.playerCash +=
                ReferenceManager.Instance.snackbarManager.tipCash;
            ReferenceManager.Instance.snackbarManager.tipCash = 0;
            SaveAndLoadSystem.Instance.SaveGame();
        }

        private void CompleteTutorial()
        {
            var reference = ReferenceManager.Instance;
            reference.tutorialHandler.TaskCompleted();

            reference.GameData.isTutorialCompleted = true;
            reference.taskHandler.HrHandler();
            reference.canvasManager.CanvasStateChanger(CanvasStates.TutorialObjective);
            reference.tutorialObjectivePanel.ShowObjective("Tutorial Complete");
            reference.playerStackManager.pathDraw.gameObject.SetActive(false);
            reference.playerStackManager.pathDraw.destination = null;
        }
    }
}
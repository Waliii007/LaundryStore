using System;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class MachineCanvasManager : MonoBehaviour
    {
        public MachineCanvasStates prev;
        public MachineCanvasStates currentState;
        public Image detergentTrackingImage;

        private void OnEnable()
        {
            CanvasStateChanger(MachineCanvasStates.Full);
            DetergentImageChange(DetergentType.Green);
        }

        public WashingMachineDropper myDropper;


        public void MachineRefillNeeded()
        {
            index = myDropper.myIndex;
            ReferenceManager.Instance.detergentItemUI.index = index;
            CanvasStateChanger(MachineCanvasStates.RefillNeeded);
        }

        public Image detargentImage;
        public Sprite[] detargentSprites;

        public void DetergentImageChange(DetergentType detargentuse)
        {
            detargentImage.preserveAspect = true;

            detargentImage.sprite = detargentSprites[(int)detargentuse];
        }

        public void OnClickRefillButton()
        {
            ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.RefillDetergent);
        }

        public void CanvasStateChanger(MachineCanvasStates newState)
        {
            prev = currentState;
            currentState = newState;
            canvasStates[(int)prev].SetActive(false);
            canvasStates[(int)currentState].SetActive(true);
            switch (newState)
            {
                case MachineCanvasStates.Full:
                    refillTrigger.gameObject.SetActive(false);
                    if (!ReferenceManager.Instance.GameData.isTutorialCompleted)
                        ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(false);
                    break;
                case MachineCanvasStates.RefillNeeded:
                    if (once)
                        ReferenceManager.Instance.notificationHandler.ShowNotification(
                            $"Machine {index} detergent needs to be refilled");
                    refillTrigger.gameObject.SetActive(true);
                    break;
            }
        }

        public bool once;
        public GameObject refillTrigger;
        private int index;
        public WaitForSeconds waitForTextUpdate = new WaitForSeconds(.01f);
        [SerializeField] private GameObject[] canvasStates;
    }
}

public enum MachineCanvasStates
{
    Full,
    RefillNeeded
}
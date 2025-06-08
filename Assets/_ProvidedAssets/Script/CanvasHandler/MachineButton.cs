using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class MachineButton : MonoBehaviour
    {
        public CanvasStates canvasState;
        public Image buttonImage;
        public WashingMachineDropper washingMachineDropper;
        public PressBasketHandler pressingClothPickingHandler;
        public RinseMachineCanvas rinManager;
        public GameObject tutorialPanel;
        public CoffeeType coffeeType;
        public CoffeeMachineCanvas coffeeManager;
        public bool isFromTrigger;
        private void OnEnable()
        {
            //tutorialPanel.SetActive(!ReferenceManager.Instance.GameData.isTutorialCompleted);
            if (!ReferenceManager.Instance.GameData.isTutorialCompleted)
            {
                DOVirtual.DelayedCall(1, TriggerState);
buttonImage.enabled = false;
                // TriggerState();
            }
            else
            {
buttonImage.enabled = true;
                
            }
        }

        public void ChangeSprite(Sprite newSprite)
        {
            buttonImage.sprite = newSprite;
        }

        public MachineRefillTrigger.TypeMachine typeMachine;

        private void OnDisable()
        {
            tutorialPanel.SetActive(false);

        }

        public void TriggerState()
        {
            
            switch (typeMachine)
            {
                case MachineRefillTrigger.TypeMachine.Machine:
                    ReferenceManager.Instance.detergentItemUI.index = washingMachineDropper.myIndex;
                    ReferenceManager.Instance.canvasManager.CanvasStateChanger(canvasState);

                    break;
                case MachineRefillTrigger.TypeMachine.Rinse:
                    ReferenceManager.Instance.rinseItemUI.index = pressingClothPickingHandler.myIndex;
                    ReferenceManager.Instance.canvasManager.CanvasStateChanger(canvasState);
                    break;
                
                case MachineRefillTrigger.TypeMachine.Coffee:
                    ReferenceManager.Instance.canvasManager.CanvasStateChanger(canvasState);
                    break;
            }
            this.gameObject.SetActive(false);

        }
    }
}
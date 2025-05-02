using System;
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

        public void ChangeSprite(Sprite newSprite)
        {
            buttonImage.sprite = newSprite;
        }

        public MachineRefillTrigger.TypeMachine typeMachine;

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
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
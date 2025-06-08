using System;
using UnityEngine;

namespace LaundaryMan
{
    public class MachineRefillTrigger : MonoBehaviour
    {
        public TypeMachine machienType;
        public MachineCanvasManager machineManager;
        public RinseMachineCanvas rinManager;
        public WashingMachineDropper washingMachineDropper;
        public Sprite sprite;
        public CanvasStates canvasState;

        public enum TypeMachine
        {
            Machine,
            Rinse,
            Coffee
        }

        public GameObject refill;
        public PressBasketHandler pressingClothPickingHandler;
        private bool once;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player") ||
                other.gameObject.layer != LayerMask.NameToLayer("Player"))
                return;

            if (!ReferenceManager.Instance.GameData.isTutorialCompleted && !once)
            {
                
                once = true;


                ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(true);

                switch (machienType)
                {
                    case TypeMachine.Machine:
                        ReferenceManager.Instance.canvasManager.machineButton.typeMachine = machienType;
                        ReferenceManager.Instance.canvasManager.machineButton.ChangeSprite(sprite);
                        ReferenceManager.Instance.canvasManager.machineButton.canvasState = canvasState;
                        ReferenceManager.Instance.canvasManager.machineButton.washingMachineDropper =
                            this.washingMachineDropper;
                        ReferenceManager.Instance.detergentPurchase.isDetergentFromTrigger = true;
                        this.gameObject.SetActive(false);

                        break;
                    case TypeMachine.Rinse:
                        ReferenceManager.Instance.canvasManager.machineButton.typeMachine = machienType;
                        ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(true);
                        ReferenceManager.Instance.rinsePurchase.isDetergentFromTrigger = true;
                        ReferenceManager.Instance.canvasManager.machineButton.ChangeSprite(sprite);
                        ReferenceManager.Instance.canvasManager.machineButton.canvasState = canvasState;
                        ReferenceManager.Instance.canvasManager.machineButton.rinManager = rinManager;
                        ReferenceManager.Instance.canvasManager.machineButton.rinManager = rinManager;
                        ReferenceManager.Instance.canvasManager.machineButton.pressingClothPickingHandler =
                            pressingClothPickingHandler;

                        break;
                }
            }
        }

        public bool onceA;

        private void OnTriggerStay(Collider other)
        {
            if (ReferenceManager.Instance.GameData.isTutorialCompleted)
            {
                if (!other.gameObject.CompareTag("Player") ||
                    other.gameObject.layer != LayerMask.NameToLayer("Player"))
                    return;

                if (!onceA)
                {
                    onceA = true;
                    ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(true);
                    switch (machienType)
                    {
                        case TypeMachine.Machine:
                            ReferenceManager.Instance.detergentPurchase.isDetergentFromTrigger = true;

                            break;
                        case TypeMachine.Rinse:
                            ReferenceManager.Instance.rinsePurchase.isDetergentFromTrigger = true;

                            break;
                    }
                }

                ;

                switch (machienType)
                {
                    case TypeMachine.Machine:
                        ReferenceManager.Instance.canvasManager.machineButton.typeMachine = machienType;


                        ReferenceManager.Instance.canvasManager.machineButton.ChangeSprite(sprite);
                        ReferenceManager.Instance.canvasManager.machineButton.canvasState = canvasState;
                        ReferenceManager.Instance.canvasManager.machineButton.washingMachineDropper =
                            this.washingMachineDropper;


                        break;
                    case TypeMachine.Rinse:
                        ReferenceManager.Instance.canvasManager.machineButton.typeMachine = machienType;

                        ReferenceManager.Instance.canvasManager.machineButton.ChangeSprite(sprite);
                        ReferenceManager.Instance.canvasManager.machineButton.canvasState = canvasState;
                        ReferenceManager.Instance.canvasManager.machineButton.rinManager = rinManager;
                        ReferenceManager.Instance.canvasManager.machineButton.rinManager = rinManager;
                        ReferenceManager.Instance.canvasManager.machineButton.pressingClothPickingHandler =
                            pressingClothPickingHandler;

                        break;
                }
            }
        }

        private void OnDisable()
        {
            onceA = false;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player") ||
                other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            
            ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(false);
            //ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(false);
            onceA = false;
        }

        public void ChangeState()
        {
            machineManager.MachineRefillNeeded();
        }
    }
}
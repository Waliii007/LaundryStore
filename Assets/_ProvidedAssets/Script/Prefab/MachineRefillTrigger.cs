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
            Rinse
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
                        this.gameObject.SetActive(false);

                        break;
                    case TypeMachine.Rinse:
                        ReferenceManager.Instance.canvasManager.machineButton.typeMachine = machienType;
                        ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(true);

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

        private void OnTriggerStay(Collider other)
        {
            if (ReferenceManager.Instance.GameData.isTutorialCompleted)

            {
                if (!other.gameObject.CompareTag("Player") ||
                    other.gameObject.layer != LayerMask.NameToLayer("Player"))
                    return;


                ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(true);

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
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player") ||
                other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(false);
            //ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(false);
        }

        public void ChangeState()
        {
            machineManager.MachineRefillNeeded();
        }
    }
}
using UnityEngine;

namespace LaundaryMan
{
    public class CoffeeRefillTrigger : MonoBehaviour
    {
       public CoffeeType coffeeType;
        public Sprite sprite;
        public CanvasStates canvasState;

        public CoffeeMachineCanvas coffeeManager;
   

         
       

        private bool once;
        private bool onceA;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player") ||
                other.gameObject.layer != LayerMask.NameToLayer("Player"))
                return;

            if (!ReferenceManager.Instance.GameData.isTutorialCompleted && !once)
            {
                once = true;

                var button = ReferenceManager.Instance.canvasManager.machineButton;
                button.gameObject.SetActive(true);
                button.typeMachine = MachineRefillTrigger.TypeMachine.Coffee;
                button.ChangeSprite(sprite);
                button.canvasState = canvasState;

                // Assign coffee-specific components
                button.coffeeType = coffeeType;
                button.coffeeManager = coffeeManager;
               
                //coffeePurchase.isFromTrigger = true;

                this.gameObject.SetActive(false);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.CompareTag("Player") ||
                other.gameObject.layer != LayerMask.NameToLayer("Player"))
                return;

            if (ReferenceManager.Instance.GameData.isTutorialCompleted)
            {
                var button = ReferenceManager.Instance.canvasManager.machineButton;
                button.gameObject.SetActive(true);

                if (!onceA)
                {
                    onceA = true;
                }

                button.typeMachine = MachineRefillTrigger.TypeMachine.Coffee;
                button.ChangeSprite(sprite);
                button.canvasState = canvasState;
                button.coffeeType = coffeeType;
                button.coffeeManager = coffeeManager;
               // button.pressingClothPickingHandler = pressingClothPickingHandler;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player") ||
                other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

            ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(false);
            onceA = false;
        }

        private void OnDisable()
        {
            onceA = false;
        }

        public void ChangeState()
        {
            coffeeManager?.CoffeeRefillNeeded();
        }
    
    }
}

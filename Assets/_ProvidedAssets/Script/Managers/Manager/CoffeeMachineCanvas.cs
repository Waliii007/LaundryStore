using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class CoffeeMachineCanvas : MonoBehaviour
    {
        public CoffeeCanvasStates prev;
        public CoffeeCanvasStates currentState;

        private void OnEnable()
        {
            CanvasStateChanger(CoffeeCanvasStates.Full);
            CoffeeImageChange(CoffeeType.GloriaPants);
        }

        public Image coffeeImage;
        public Sprite[] coffeeSprites; // Make sure array matches CoffeeType enum order

        public int machineIndex;
        public bool showOnce = true;

        public GameObject refillTrigger;

        [SerializeField] private GameObject[] canvasStates; // Assign in inspector

        public void CoffeeRefillNeeded()
        {
            machineIndex = ReferenceManager.Instance.selectedMachineIndex;
            ReferenceManager.Instance.coffeeItemUI.index = machineIndex;
            CanvasStateChanger(CoffeeCanvasStates.RefillNeeded);
        }

        public void CoffeeImageChange(CoffeeType type)
        {
            coffeeImage.preserveAspect = true;
            coffeeImage.sprite = coffeeSprites[(int)type];
        }

        public void OnClickRefillButton()
        {
            ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.CoffeeUse);
        }

        public void CanvasStateChanger(CoffeeCanvasStates newState)
        {
            prev = currentState;
            currentState = newState;

            canvasStates[(int)prev].SetActive(false);
            canvasStates[(int)currentState].SetActive(true);

            switch (newState)
            {
                case CoffeeCanvasStates.Full:
                    refillTrigger.SetActive(false);
                    if (!ReferenceManager.Instance.GameData.isTutorialCompleted)
                        ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(false);
                    break;

                case CoffeeCanvasStates.RefillNeeded:
                    if (showOnce)
                        ReferenceManager.Instance.notificationHandler.ShowNotification(
                            $"Machine {machineIndex} coffee needs to be refilled");
                    refillTrigger.SetActive(true);
                    break;
            }
        }
    }
}

public enum CoffeeCanvasStates
{
    Full,
    RefillNeeded
}
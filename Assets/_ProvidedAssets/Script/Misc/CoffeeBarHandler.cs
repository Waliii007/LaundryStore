using System;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class CoffeeBarHandler : MonoBehaviour
    {
        public Image coffeeSlider;
        public CoffeeConsumeTrigger coffeeConsumeTrigger;

        private int maxServes = 0;
        private int remainingServes = 0;
        public CoffeeType currentCoffeeType;
        public CoffeeMachineCanvas coffeeCanvasManager;
        public GameObject consumeTrigger;
        public GameObject refilTrigger;

        private void OnEnable()
        {
            UseCoffee(CoffeeType.GloriaPants);
        }

        public void UseCoffee(CoffeeType type)
        {
            currentCoffeeType = type;
            maxServes = GetMaxServeCount(type);
            print(maxServes);
            remainingServes = maxServes;
            UpdateUI();
        }

        public void ConsumeServe(int customers)
        {
            if (remainingServes > 0 && customers >= 1)
            {
                ReferenceManager.Instance.playerStackManager.CupsOnAndOff();
                ReferenceManager.playerHasTheCoffee = true;
                remainingServes = Mathf.Max(0, remainingServes - customers);
                
                UpdateUI();
            }
            else if (customers > 0)
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("Not enough coffee beans in machine.");
            }
        }

        public bool HasEnoughCoffee(int customers)
        {
            return remainingServes >= customers;
        }

        private int GetMaxServeCount(CoffeeType type)
        {
            switch (type)
            {
                case CoffeeType.GloriaPants: return 75;
                case CoffeeType.StarDucks: return 125;
                case CoffeeType.SimBortan: return 250;
                default: return 0;
            }
        }

        private void UpdateUI()
        {
            if (coffeeSlider.fillAmount <= 0)
            {
                coffeeSlider.fillAmount = 0;
                coffeeCanvasManager.CanvasStateChanger(CoffeeCanvasStates.RefillNeeded);
                refilTrigger.gameObject.SetActive(true);
                return;
            }

            print(remainingServes);
            coffeeSlider.fillAmount = (float)remainingServes / maxServes;
            consumeTrigger.gameObject.SetActive(true);
            if (remainingServes == 0)
            {
                coffeeCanvasManager.CanvasStateChanger(CoffeeCanvasStates.RefillNeeded);
            }
            else if ((remainingServes > 0))
            {
                coffeeCanvasManager.CanvasStateChanger(CoffeeCanvasStates.Full);
            }
        }
    }
}
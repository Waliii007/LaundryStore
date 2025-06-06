using System;
using Unity.Entities.Hybrid.Baking;
using UnityEngine;

namespace LaundaryMan
{
    public class CoffeeConsumeTrigger : MonoBehaviour
    {
        public int
            serveIndex =
                1; // Index to define how many customers this trigger serves (e.g., 1 cup = 1 customer or any custom logic)

        public CoffeeBarHandler coffeeBar;

        private void OnEnable()
        {
            serveIndex = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!ReferenceManager.Instance.GameData.isTutorialCompleted)
                return;
            if (ReferenceManager.Instance.playerStackManager.ClothStack.Count > 0)
                return;
            if (ReferenceManager.Instance.checkoutHandler.isCoffeeServed())
                return;
            if (ReferenceManager.playerHasTheCoffee)
                return;
            if (other.CompareTag("Player"))
            {
                // You can change this mapping logic as needed
                print(serveIndex);
                int customersToServe = GetCustomerCountFromIndex(serveIndex);

                if (coffeeBar != null)
                {
                    if (coffeeBar.HasEnoughCoffee(customersToServe))
                    {
                        coffeeBar.ConsumeServe(customersToServe);
                        Debug.Log($"Consumed {customersToServe} coffee serves.");
                    }
                    else
                    {
                        Debug.Log("Not enough coffee to serve customers.");
                    }
                }
                else
                {
                    Debug.LogWarning("CoffeeBarHandler reference not set!");
                }
            }
        }

        private int GetCustomerCountFromIndex(int index)
        {
            // Customize this mapping however you want
            switch (index)
            {
                case 0: return 0;
                case 1: return 5;
                case 2: return 10;
                case 3: return 15;
            }

            return 0;
        }
    }
}
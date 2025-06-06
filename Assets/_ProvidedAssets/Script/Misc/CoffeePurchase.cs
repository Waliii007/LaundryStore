using UnityEngine;

namespace LaundaryMan
{
    public class CoffeePurchase : MonoBehaviour
    {
        public int gloriaPrice = 100;
        public int starPrice = 200;
        public int simPrice = 300;

        public void OnBuyCoffee(CoffeeType type)
        {
            int price = GetPrice(type);
            if (ReferenceManager.Instance.GameData.playerCash >= price)
            {
                AddCoffee(type, 1);
                ReferenceManager.Instance.GameData.playerCash -= price;
                ReferenceManager.Instance.notificationHandler.ShowNotification($"Purchased 1 {type}");
                UpdateCoffeeUI();
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("Not enough cash");
            }
        }

        public void OnBuyAdsCoffee(CoffeeType type)
        {
            TssAdsManager._Instance.ShowRewardedAd((() =>
            {
                AddCoffee(type, 1);
                ReferenceManager.Instance.notificationHandler.ShowNotification($"Purchased 1 {type}");
                UpdateCoffeeUI();
            }), "OnBuyAdsCoffee" + type);
        }

        public void OnClickBackButton()
        {
            ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.MainControls);
        }

        public void OnUseCoffee(CoffeeType type, int index)
        {
            if (UseCoffee(type))
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification($"Used {type}");
                UpdateCoffeeUI();
                // Add custom logic if needed
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("No coffee available.");
            }
        }

        private int GetPrice(CoffeeType type) => type switch
        {
            CoffeeType.GloriaPants => gloriaPrice,
            CoffeeType.StarDucks => starPrice,
            CoffeeType.SimBortan => simPrice,
            _ => 0
        };

        public void AddCoffee(CoffeeType type, int amount)
        {
            var g = ReferenceManager.Instance.GameData.gameEconomy;
            switch (type)
            {
                case CoffeeType.GloriaPants: g.gloriaPantsCount += amount; break;
                case CoffeeType.StarDucks: g.starPantsCount += amount; break;
                case CoffeeType.SimBortan: g.simHortanCount += amount; break;
            }
        }

        public bool UseCoffee(CoffeeType type)
        {
            var g = ReferenceManager.Instance.GameData.gameEconomy;
            switch (type)
            {
                case CoffeeType.GloriaPants when g.gloriaPantsCount > 0:
                    g.gloriaPantsCount--;
                    ReferenceManager.Instance.coffeeBarHandler.UseCoffee(type);

                    return true;
                case CoffeeType.StarDucks when g.starPantsCount > 0:
                    g.starPantsCount--;
                    ReferenceManager.Instance.coffeeBarHandler.UseCoffee(type);
                    return true;
                case CoffeeType.SimBortan when g.simHortanCount > 0:
                    g.simHortanCount--;
                    ReferenceManager.Instance.coffeeBarHandler.UseCoffee(type);
                    return true;
            }

            return false;
        }

        public int GetCoffeeCount(CoffeeType type)
        {
            var g = ReferenceManager.Instance.GameData.gameEconomy;
            return type switch
            {
                CoffeeType.GloriaPants => g.gloriaPantsCount,
                CoffeeType.StarDucks => g.starPantsCount,
                CoffeeType.SimBortan => g.simHortanCount,
                _ => 0
            };
        }

        public void OnBuyGloriaPants() => OnBuyCoffee(CoffeeType.GloriaPants);
        public void OnBuyStarPants() => OnBuyCoffee(CoffeeType.StarDucks);
        public void OnBuySimHortan() => OnBuyCoffee(CoffeeType.SimBortan);

        private void UpdateCoffeeUI()

        {
            var ui = ReferenceManager.Instance.coffeeItemUI;
            if (ui) ui.UpdateCoffeeUI();
        }
    }
}

public enum CoffeeType
{
    GloriaPants,
    StarDucks,
    SimBortan
}
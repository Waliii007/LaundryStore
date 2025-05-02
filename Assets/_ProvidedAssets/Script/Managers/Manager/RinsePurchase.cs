using UnityEngine;

namespace LaundaryMan
{
    public enum RinseType
    {
        Green = 0, // 250 rinse amount
        Blue = 1, // 400 rinse amount
        Red = 2 // 500 rinse amount
    }

    public class RinsePurchase : MonoBehaviour
    {
        public PressBasketHandler[] washingMachineDropper;

        public void OnClickCrossButton()
        {
            ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.MainControls);
        }

        #region Buy Buttons

        public void OnBuyGreenButton()
        {
            int price = 1000;
            if (ReferenceManager.Instance.GameData.playerCash >= price)
            {
                AddRinse(RinseType.Green, 1);
                ReferenceManager.Instance.GameData.playerCash -= price;
                ReferenceManager.Instance.notificationHandler.ShowNotification("Purchased 1 Green Rinse");
                UpdateRinseUI();
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("Not enough cash for Green Rinse");
            }
        }

        public void OnBuyBlueButton()
        {
            int price = 2000;
            if (ReferenceManager.Instance.GameData.playerCash >= price)
            {
                AddRinse(RinseType.Blue, 1);
                ReferenceManager.Instance.GameData.playerCash -= price;
                ReferenceManager.Instance.notificationHandler.ShowNotification("Purchased 1 Blue Rinse");
                UpdateRinseUI();
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("Not enough cash for Blue Rinse");
            }
        }

        public void OnBuyRedButton()
        {
            int price = 3000;
            if (ReferenceManager.Instance.GameData.playerCash >= price)
            {
                AddRinse(RinseType.Red, 1);
                ReferenceManager.Instance.GameData.playerCash -= price;
                ReferenceManager.Instance.notificationHandler.ShowNotification("Purchased 1 Red Rinse");
                UpdateRinseUI();
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("Not enough cash for Red Rinse");
            }
        }


        public void OnBuyGreenButtonAds()
        {
            int price = 1000;
            TssAdsManager._Instance.ShowRewardedAd(() =>
            {
                AddRinse(RinseType.Green, 1);
                ReferenceManager.Instance.notificationHandler.ShowNotification("Purchased 1 Green Rinse");
                UpdateRinseUI();
            }, "OnBuyGreenButton");
        }

        public void OnBuyBlueButtonAds()
        {
            int price = 2000;
            TssAdsManager._Instance.ShowRewardedAd(() =>
            {
                AddRinse(RinseType.Green, 1);
                ReferenceManager.Instance.notificationHandler.ShowNotification("Purchased 1 Green Rinse");
                UpdateRinseUI();
            }, "OnBuyBlueButton");
        }

        public void OnBuyRedButtonAds()
        {
            int price = 3000;
            TssAdsManager._Instance.ShowRewardedAd(() =>
            {
                AddRinse(RinseType.Red, 1);
                ReferenceManager.Instance.notificationHandler.ShowNotification("Purchased 1 Red Rinse");
                UpdateRinseUI();
            }, "OnBuyRedButton");
        }

        #endregion

        #region Use Buttons

        public void OnUseGreenButton(int machineIndex)
        {
            if (UseRinse(RinseType.Green,machineIndex))
            {
                washingMachineDropper[machineIndex].RefillRin(250);
                UpdateRinseUI();
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("No Green Rinse available.");
            }
        }

        public void OnUseBlueButton(int machineIndex)
        {
            if (UseRinse(RinseType.Blue,machineIndex))
            {
                washingMachineDropper[machineIndex].RefillRin(400);
                UpdateRinseUI();
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("No Blue Rinse available.");
            }
        }

        public void OnUseRedButton(int machineIndex)
        {
            if (UseRinse(RinseType.Red,machineIndex))
            {
                washingMachineDropper[machineIndex].RefillRin(500);
                UpdateRinseUI();
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("No Red Rinse available.");
            }
        }

        #endregion

        #region Rinse Logic

        public void AddRinse(RinseType type, int amount)
        {
            switch (type)
            {
                case RinseType.Green:
                    ReferenceManager.Instance.GameData.gameEconomy.remainingRinseBottleGreen += amount;
                    break;
                case RinseType.Blue:
                    ReferenceManager.Instance.GameData.gameEconomy.remainingRinseBottleBlue += amount;
                    break;
                case RinseType.Red:
                    ReferenceManager.Instance.GameData.gameEconomy.remainingRinseBottleRed += amount;
                    break;
            }
        }

        public bool UseRinse(RinseType type, int index)
        {
            if (!HasRinse(type, index)) return false;

            switch (type)
            {
                case RinseType.Green:
                    ReferenceManager.Instance.GameData.gameEconomy.remainingRinseBottleGreen--;
                    break;
                case RinseType.Blue:
                    ReferenceManager.Instance.GameData.gameEconomy.remainingRinseBottleBlue--;
                    break;
                case RinseType.Red:
                    ReferenceManager.Instance.GameData.gameEconomy.remainingRinseBottleRed--;
                    break;
            }

            return true;
        }

        public bool HasRinse(RinseType type, int index)
        {
            return GetRinseCount(type) > 0 && washingMachineDropper[index];
        }

        public int GetRinseCount(RinseType type)
        {
            switch (type)
            {
                case RinseType.Green:
                    return ReferenceManager.Instance.GameData.gameEconomy.remainingRinseBottleGreen;
                case RinseType.Blue:
                    return ReferenceManager.Instance.GameData.gameEconomy.remainingRinseBottleBlue;
                case RinseType.Red:
                    return ReferenceManager.Instance.GameData.gameEconomy.remainingRinseBottleRed;
                default:
                    return 0;
            }
        }

        #endregion

        private void UpdateRinseUI()
        {
            var ui = ReferenceManager.Instance.rinseItemUI;
            if (ui != null)
                ui.UpdateRinseUI();
        }
    }
}
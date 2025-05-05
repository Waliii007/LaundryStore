using System;
using UnityEngine;

namespace LaundaryMan
{
    public enum DetergentType
    {
        Green = 0, // 250 it will wash 50 clothes  
        Blue = 1, // 400 it will wash 80  
        Red = 2 // 500 it will wash 100 
    }

    public class DetergentPurchase : MonoBehaviour
    {
        public WashingMachineDropper[] washingMachineDropper;
        public GameObject tutorialPanel;

        private void OnEnable()
        {
            tutorialPanel.SetActive(!ReferenceManager.Instance.GameData.isTutorialCompleted);
        }

        public void OnClickCrossButton()
        {
            ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.MainControls);
        }

        #region Buy Buttons

        public void OnBuyGreenButton()
        {
            int price = 150;
            if (ReferenceManager.Instance.GameData.playerCash >= price)
            {
                AddDetergent(DetergentType.Green, 1);
                ReferenceManager.Instance.GameData.playerCash -= price;
                ReferenceManager.Instance.notificationHandler.ShowNotification("Purchased 1 Green Detergent");
                UpdateDetergentUI();
                if (!ReferenceManager.Instance.GameData.isTutorialCompleted)
                {
                    ReferenceManager.Instance.tutorialHandler.TaskCompleted();
                }
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("Not enough cash for Green Detergent");
            }
        }

        public void OnBuyBlueButton()
        {
            int price = 200;
            if (ReferenceManager.Instance.GameData.playerCash >= price)
            {
                AddDetergent(DetergentType.Blue, 1);
                ReferenceManager.Instance.GameData.playerCash -= price;
                ReferenceManager.Instance.notificationHandler.ShowNotification("Purchased 1 Blue Detergent");
                UpdateDetergentUI();
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("Not enough cash for Blue Detergent");
            }
        }

        public void OnBuyRedButton()
        {
            int price = 500;
            if (ReferenceManager.Instance.GameData.playerCash >= price)
            {
                AddDetergent(DetergentType.Red, 1);
                ReferenceManager.Instance.GameData.playerCash -= price;
                ReferenceManager.Instance.notificationHandler.ShowNotification("Purchased 1 Red Detergent");
                UpdateDetergentUI();
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("Not enough cash for Red Detergent");
            }
        }

        #endregion

        #region Use Buttons

        public void OnBuyGreenButtonAds()
        {
            TssAdsManager._Instance.ShowRewardedAd(() =>
            {
                AddDetergent(DetergentType.Green, 1);
                ReferenceManager.Instance.notificationHandler.ShowNotification("Purchased 1 Green Rinse");
                UpdateDetergentUI();
            }, "OnBuyGreenButton");
        }

        public void OnBuyBlueButtonAds()
        {
            TssAdsManager._Instance.ShowRewardedAd(() =>
            {
                AddDetergent(DetergentType.Blue, 1);
                ReferenceManager.Instance.notificationHandler.ShowNotification("Purchased 1 Green Rinse");
                UpdateDetergentUI();
            }, "OnBuyBlueButton");
        }

        public void OnBuyRedButtonAds()
        {
            TssAdsManager._Instance.ShowRewardedAd(() =>
            {
                AddDetergent(DetergentType.Red, 1);
                ReferenceManager.Instance.notificationHandler.ShowNotification("Purchased 1 Red Rinse");
                UpdateDetergentUI();
            }, "OnBuyRedButton");
        }

        public void OnUseGreenButton(int machineIndex)
        {
            if (UseDetergent(DetergentType.Green, machineIndex))
            {
                washingMachineDropper[machineIndex].Refill(250);
                UpdateDetergentUI();
                ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.MainControls);
                ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(false);
                washingMachineDropper[machineIndex].machineCanvasManager.DetergentImageChange(DetergentType.Green);
            }
            else
            {
                ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.DetergentPurchase);

                ReferenceManager.Instance.notificationHandler.ShowNotification("No Green Detergent available.");
            }
        }

        public void OnUseBlueButton(int machineIndex)
        {
            if (UseDetergent(DetergentType.Blue, machineIndex))
            {
                washingMachineDropper[machineIndex].Refill(400);
                UpdateDetergentUI();
                ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.MainControls);
                ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(false);
                washingMachineDropper[machineIndex].machineCanvasManager.DetergentImageChange(DetergentType.Blue);
            }
            else
            {
                ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.DetergentPurchase);

                ReferenceManager.Instance.notificationHandler.ShowNotification("No Blue Detergent available.");
            }
        }

        public void OnUseRedButton(int machineIndex)
        {
            if (UseDetergent(DetergentType.Red, machineIndex))
            {
                washingMachineDropper[machineIndex].Refill(500);
                ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.MainControls);
                ReferenceManager.Instance.canvasManager.machineButton.gameObject.SetActive(false);
                washingMachineDropper[machineIndex].machineCanvasManager.DetergentImageChange(DetergentType.Red);
                UpdateDetergentUI();
            }
            else
            {
                ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.DetergentPurchase);

                ReferenceManager.Instance.notificationHandler.ShowNotification("No Red Detergent available.");
            }
        }

        #endregion

        #region Detergent Logic

        public void AddDetergent(DetergentType type, int amount)
        {
            switch (type)
            {
                case DetergentType.Green:
                    ReferenceManager.Instance.GameData.gameEconomy.remainingDetergentBottleGreen += amount;
                    break;
                case DetergentType.Blue:
                    ReferenceManager.Instance.GameData.gameEconomy.remainingDetergentBottleBlue += amount;
                    break;
                case DetergentType.Red:
                    ReferenceManager.Instance.GameData.gameEconomy.remainingDetergentBottleRed += amount;
                    break;
            }
        }

        public bool UseDetergent(DetergentType type, int index)
        {
            if (!HasDetergent(type, index)) return false;

            switch (type)
            {
                case DetergentType.Green:
                    ReferenceManager.Instance.GameData.gameEconomy.remainingDetergentBottleGreen--;
                    break;
                case DetergentType.Blue:
                    ReferenceManager.Instance.GameData.gameEconomy.remainingDetergentBottleBlue--;
                    break;
                case DetergentType.Red:
                    ReferenceManager.Instance.GameData.gameEconomy.remainingDetergentBottleRed--;
                    break;
            }

            return true;
        }

        public bool HasDetergent(DetergentType type, int refillIndex)
        {
            print(refillIndex);
            return GetDetergentCount(type) > 0 && washingMachineDropper[refillIndex].totalDetergent <= 0;
        }

        public int GetDetergentCount(DetergentType type)
        {
            switch (type)
            {
                case DetergentType.Green:
                    return ReferenceManager.Instance.GameData.gameEconomy.remainingDetergentBottleGreen;
                case DetergentType.Blue:
                    return ReferenceManager.Instance.GameData.gameEconomy.remainingDetergentBottleBlue;
                case DetergentType.Red:
                    return ReferenceManager.Instance.GameData.gameEconomy.remainingDetergentBottleRed;
                default:
                    return 0;
            }
        }

        #endregion

        private void UpdateDetergentUI()
        {
            var ui = ReferenceManager.Instance.detergentItemUI;
            if (ui)
                ui.UpdateDetergentUI();
        }
    }
}
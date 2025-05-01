using System;
using UnityEngine;

namespace LaundaryMan
{
    public class RefillPanelEventListener : MonoBehaviour
    {
        public WashingMachineDropper[] washingMachineDropper;
        public PressBasketHandler[] pressMachine;

        public GameObject[] washingMachinePanel;
        public GameObject[] detargentUpgrade;
        public GameObject[] rinUpgrade;
        public GameObject[] rinRefill;
        public GameObject[] maxedOutImages; // For detergent
        public GameObject[] buttonPanel; // For detergent

        public int[] detergentCapacities = new int[3];
        public int[] detergentMaxCapacities = new int[3];

        public int[] rinCapacities = new int[3];
        public int[] rinMaxCapacities = new int[3];

        public GameObject[] rinMaxedOutImages;
        public GameObject[] rinButtonPanel;

        public int costPer10Upgrade = 100;
        public int costPer3Upgrade = 40;
        public int refillCost = 50;

        private void OnEnable()
        {
            LoadUpgrades();

            for (int i = 0; i < washingMachineDropper.Length; i++)
            {
                bool isUnlocked = i < ReferenceManager.Instance.GameData.unlockedMachine;

                washingMachinePanel[i].SetActive(isUnlocked);
                detargentUpgrade[i].SetActive(isUnlocked);
                rinUpgrade[i].SetActive(isUnlocked);
                rinRefill[i].SetActive(isUnlocked);

                if (isUnlocked && maxedOutImages.Length > i)
                {
                    bool isMaxed = detergentCapacities[i] >= detergentMaxCapacities[i];
                    maxedOutImages[i].SetActive(isMaxed);
                    buttonPanel[i].SetActive(!isMaxed);
                }

                if (isUnlocked && rinMaxedOutImages.Length > i)
                {
                    bool isRinMaxed = rinCapacities[i] >= rinMaxCapacities[i];
                    rinMaxedOutImages[i].SetActive(isRinMaxed);
                    rinButtonPanel[i].SetActive(!isRinMaxed);
                }
            }
        }

        private void LoadUpgrades()
        {
            detergentCapacities[0] = ReferenceManager.Instance.GameData.gameEconomy.detergentCapacity1;
            detergentCapacities[1] = ReferenceManager.Instance.GameData.gameEconomy.detergentCapacity2;
            detergentCapacities[2] = ReferenceManager.Instance.GameData.gameEconomy.detergentCapacity3;

            rinCapacities[0] = ReferenceManager.Instance.GameData.gameEconomy.rin1Capacity;
            rinCapacities[1] = ReferenceManager.Instance.GameData.gameEconomy.rin2Capacity;
            rinCapacities[2] = ReferenceManager.Instance.GameData.gameEconomy.rin3Capacity;
        }


        private void SaveDetergentUpgrade(int index)
        {
            switch (index)
            {
                case 0:
                    ReferenceManager.Instance.GameData.gameEconomy.detergentCapacity1 = detergentCapacities[0];
                    washingMachineDropper[0].InitDetergent();
                    break;
                case 1:
                    ReferenceManager.Instance.GameData.gameEconomy.detergentCapacity2 = detergentCapacities[1];
                    washingMachineDropper[1].InitDetergent();
                    break;
                case 2:
                    ReferenceManager.Instance.GameData.gameEconomy.detergentCapacity3 = detergentCapacities[2];
                    washingMachineDropper[2].InitDetergent();
                    break;
            }
        }

        private void SaveRinUpgrade(int index)
        {
            switch (index)
            {
                case 0: ReferenceManager.Instance.GameData.gameEconomy.rin1Capacity = rinCapacities[0]; break;
                case 1: ReferenceManager.Instance.GameData.gameEconomy.rin2Capacity = rinCapacities[1]; break;
                case 2: ReferenceManager.Instance.GameData.gameEconomy.rin3Capacity = rinCapacities[2]; break;
            }
        }

        // ---------- UI Button Calls for Detergent Upgrades ----------

        public void OnUpgradeDetergent_Machine1_By10() =>
            UpgradeDetergentCapacity(0, washingMachineDropper[0].refillAmount);

        public void OnUpgradeDetergent_Machine1_By10_Ads() =>
            TssAdsManager._Instance.ShowRewardedAd(OnUpgradeDetergent_Machine1_By10,
                "OnUpgradeDetergent_Machine1_By10");

        public void OnUpgradeDetergent_Machine2_By10() =>
            UpgradeDetergentCapacity(1, washingMachineDropper[0].refillAmount);

        public void OnUpgradeDetergent_Machine2_By10_Ads() =>
            TssAdsManager._Instance.ShowRewardedAd(OnUpgradeDetergent_Machine2_By10,
                "OnUpgradeDetergent_Machine2_By10");

        public void OnUpgradeDetergent_Machine3_By10() =>
            UpgradeDetergentCapacity(2, washingMachineDropper[0].refillAmount);

        public void OnUpgradeDetergent_Machine3_By10_Ads() =>
            TssAdsManager._Instance.ShowRewardedAd(OnUpgradeDetergent_Machine3_By10,
                "OnUpgradeDetergent_Machine3_By10");
        // ---------- UI Button Calls for Detergent Upgrades ----------


        // ---------- UI Button Calls for Rin Upgrades ----------

        public void OnUpgradeRin_Machine1_By10() => UpgradeRinCapacity(0, 10);

        public void OnUpgradeRin_Machine1_By10_Add() =>
            TssAdsManager._Instance.ShowRewardedAd(OnUpgradeRin_Machine1_By10,
                "OnUpgradeRin_Machine1_By10");

        public void OnUpgradeRin_Machine2_By10() => UpgradeRinCapacity(1, 10);

        public void OnUpgradeRin_Machine2_By10_Add() =>
            TssAdsManager._Instance.ShowRewardedAd(OnUpgradeRin_Machine2_By10,
                "OnUpgradeRin_Machine1_By10");

        public void OnUpgradeRin_Machine3_By10() => UpgradeRinCapacity(2, 10);

        public void OnUpgradeRin_Machine3_By10_Add() =>
            TssAdsManager._Instance.ShowRewardedAd(OnUpgradeRin_Machine3_By10,
                "OnUpgradeRin_Machine1_By10");
        // ---------- UI Button Calls for Rin Upgrades ----------

        public void UpgradeDetergentCapacity(int index, int amount)
        {
            if (index >= detergentCapacities.Length) return;

            int cost = amount == 10 ? costPer10Upgrade : costPer3Upgrade;

            if (ReferenceManager.Instance.GameData.playerCash >= cost)
            {
                if (detergentCapacities[index] >= detergentMaxCapacities[index])
                {
                    ShowToast("Already at max detergent.");
                    return;
                }

                detergentCapacities[index] += 10;
                ReferenceManager.Instance.GameData.playerCash -= cost;

                if (detergentCapacities[index] > detergentMaxCapacities[index])
                    detergentCapacities[index] = detergentMaxCapacities[index];

                SaveDetergentUpgrade(index);

                if (detergentCapacities[index] >= detergentMaxCapacities[index])
                    maxedOutImages[index].SetActive(true);

                ShowToast($"Detergent +{amount} upgraded.");
            }
            else
            {
                ShowToast("Not enough cash.");
            }
        }

        public void UpgradeRinCapacity(int index, int amount)
        {
            if (index >= rinCapacities.Length) return;

            int cost = amount == 10 ? costPer10Upgrade : costPer3Upgrade;

            if (ReferenceManager.Instance.GameData.playerCash >= cost)
            {
                if (rinCapacities[index] >= rinMaxCapacities[index])
                {
                    ShowToast("Already at max rinse.");
                    return;
                }

                rinCapacities[index] += amount;
                ReferenceManager.Instance.GameData.playerCash -= cost;

                if (rinCapacities[index] > rinMaxCapacities[index])
                {
                    buttonPanel[index].SetActive(false);

                    rinCapacities[index] = rinMaxCapacities[index];
                }

                SaveRinUpgrade(index);

                if (rinCapacities[index] >= rinMaxCapacities[index])
                {
                    rinButtonPanel[index].SetActive(false);
                    rinMaxedOutImages[index].SetActive(true);
                }

                ShowToast($"Rinse +{amount} upgraded.");
            }
            else
            {
                ShowToast("Not enough cash.");
            }
        }

        public void TryRefillMachine(int index)
        {
            if (ReferenceManager.Instance.GameData.playerCash >= refillCost)
            {
                ReferenceManager.Instance.GameData.playerCash -= refillCost;
                washingMachineDropper[index].Refill();
                ShowToast("Machine refilled.");
            }
            else
            {
                ShowToast("Not enough cash. Watch ad.");
            }
        }

        public void TryRefillRins(int index)
        {
            if (ReferenceManager.Instance.GameData.playerCash >= refillCost)
            {
                ReferenceManager.Instance.GameData.playerCash -= refillCost;
                pressMachine[index].RefillRin();
                ShowToast("Rinse refilled.");
            }
            else
            {
                ShowToast("Not enough cash. Watch ad.");
            }
        }

        public void Machine1Refill() => TryRefillMachine(0);
        public void Machine2Refill() => TryRefillMachine(1);
        public void Machine3Refill() => TryRefillMachine(2);

        public void Machine1RefillWithAdds() =>
            TssAdsManager._Instance.ShowRewardedAd(() => TryRefillMachine(0), "RefillMachine1Detergent");

        public void Machine2RefillWithAdds() =>
            TssAdsManager._Instance.ShowRewardedAd(() => TryRefillMachine(1), "RefillMachine1Detergent");

        public void Machine3RefillWithAdds() =>
            TssAdsManager._Instance.ShowRewardedAd(() => TryRefillMachine(2), "RefillMachine1Detergent");

        public void Rins1Refill() => TryRefillRins(0);
        public void Rins2Refill() => TryRefillRins(1);
        public void Rins3Refill() => TryRefillRins(2);

        public void Rin1RefillWithAdds() =>
            TssAdsManager._Instance.ShowRewardedAd(() => TryRefillMachine(0), "RefillMachine1Detergent");

        public void Rin2RefillWithAdds() =>
            TssAdsManager._Instance.ShowRewardedAd(() => TryRefillMachine(1), "RefillMachine1Detergent");

        public void Rin3RefillWithAdds() =>
            TssAdsManager._Instance.ShowRewardedAd(() => TryRefillMachine(2), "RefillMachine1Detergent");

        public void Cross()
        {
            ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.MainControls);
        }

        public void ShowToast(string msg)
        {
            Debug.Log("TOAST: " + msg);
            // Replace with your ToastManager if available
        }
    }
}
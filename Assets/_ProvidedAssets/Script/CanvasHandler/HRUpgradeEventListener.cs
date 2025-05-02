using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class HRUpgradeEventListener : MonoBehaviour
    {
        #region HrPlayerUpgradeSection

        public Text playerMovingSpeedText;
        public int[] playerMovingSpeed;

        public Text playerStackingSpeedText;
        public int[] playerStackingSpeed;

        public Text playerCapacityText;
        public int[] playerCapacity;

        public Text machineUpgradeText;
        public int[] machineUpgrade;

        public Text machine1UpgradeText;
        public int[] machine1Upgrade;

        public Text machine2UpgradeText;
        public int[] machine2Upgrade;


        public Button maxOutSpeed;
        public Button maxOutStack;
        public Button maxOutCapacity;
        public Button maxOutMachine;
        public Button maxOutMachine1;
        public Button maxOutMachine2;

        public GameObject[] speedAdsButtons;
        public GameObject[] stackingAdsButtons;
        public GameObject[] capacityAdsButtons;
        public GameObject[] machineAdsButtons;
        public GameObject[] machine1AdsButtons;
        public GameObject[] machine2AdsButtons;

        public ParticleSystem[] particle;

        public void PlayerMovingSpeedIncreaseRewarded()
        {
            if (TssAdsManager._Instance)
            {
                TssAdsManager._Instance.ShowRewardedAd(() =>
                {
                    int index = ReferenceManager.Instance.GameData.playerMovingSpeed - 1;
                    if (index >= playerMovingSpeed.Length) return;

                    ReferenceManager.Instance.GameData.playerMovingSpeed++;
                    print("Player Moving Speed Upgraded: " + ReferenceManager.Instance.GameData.playerMovingSpeed);
                    ReferenceManager.Instance.SaveGameDataObserver();
                    UiUpdate();
                    if (SoundManager.instance)
                    {
                        SoundManager.instance.Play(SoundName.Upgrade);
                    }
                }, "MovingSpeedUpgrade");
            }

            if (SoundManager.instance)
            {
                SoundManager.instance.Play(SoundName.Upgrade);
            }
        }

        public void PlayerMovingSpeedIncrease()
        {
            int index = ReferenceManager.Instance.GameData.playerMovingSpeed - 1;
            if (index >= playerMovingSpeed.Length) return;
            int cost = playerMovingSpeed[Mathf.Max(0, index)];
            if (ReferenceManager.Instance.GameData.playerCash >= cost)
            {
                ReferenceManager.Instance.GameData.playerCash -= cost;
                ReferenceManager.Instance.GameData.playerMovingSpeed++;
                print("Player Moving Speed Upgraded: " + ReferenceManager.Instance.GameData.playerMovingSpeed);
                ReferenceManager.Instance.SaveGameDataObserver();
                UiUpdate();
                if (SoundManager.instance)
                {
                    SoundManager.instance.Play(SoundName.Upgrade);
                }
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("Not Enough Money");
            }
        }

        public void StackingSpeedIncreaseRewarded()
        {
            if (TssAdsManager._Instance)
            {
                TssAdsManager._Instance.ShowRewardedAd(() =>
                {
                    int index = ReferenceManager.Instance.GameData.stackingSpeed - 1;
                    if (index >= playerStackingSpeed.Length) return;

                    ReferenceManager.Instance.GameData.stackingSpeed++;

                    print("Stacking Speed Upgraded: " + ReferenceManager.Instance.GameData.stackingSpeed);
                    ReferenceManager.Instance.SaveGameDataObserver();
                    UiUpdate();
                    if (SoundManager.instance)
                    {
                        SoundManager.instance.Play(SoundName.Upgrade);
                    }
                }, "StackingSpeed");
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("Not Enough Money");
            }

            if (SoundManager.instance)
            {
                SoundManager.instance.Play(SoundName.Upgrade);
            }
        }

        public void StackingSpeedIncrease()
        {
            int index = ReferenceManager.Instance.GameData.stackingSpeed - 1;
            if (index >= playerStackingSpeed.Length) return;
            int cost = playerStackingSpeed[Mathf.Max(0, index)];
            if (ReferenceManager.Instance.GameData.playerCash >= cost)
            {
                ReferenceManager.Instance.GameData.playerCash -= cost;
                ReferenceManager.Instance.GameData.stackingSpeed++;

                print("Stacking Speed Upgraded: " + ReferenceManager.Instance.GameData.stackingSpeed);
                ReferenceManager.Instance.SaveGameDataObserver();
                UiUpdate();
                if (SoundManager.instance)
                {
                    SoundManager.instance.Play(SoundName.Upgrade);
                }
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("Not Enough Money");
            }
        }

        public void CarryCapacityIncreaseRewarded()
        {
            if (TssAdsManager._Instance)
            {
                TssAdsManager._Instance.ShowRewardedAd(() =>
                    {
                        int index = ReferenceManager.Instance.GameData.playerCapacity;
                        if (index >= playerCapacity.Length) return;
                        int cost = playerCapacity[Mathf.Max(0, index)];

                        ReferenceManager.Instance.GameData.playerCapacity++;

                        print("Player Capacity Upgraded: " + ReferenceManager.Instance.GameData.playerCapacity);
                        ReferenceManager.Instance.taskHandler.OnTaskCompleted();
                        ReferenceManager.Instance.SaveGameDataObserver();
                        UiUpdate();
                    },
                    "CarryCapacity");
            }
        }

        public void CarryCapacity()
        {
            int index = ReferenceManager.Instance.GameData.playerCapacity;
            if (index >= playerCapacity.Length) return;
            int cost = playerCapacity[Mathf.Max(0, index)];
            if (ReferenceManager.Instance.GameData.playerCash >= cost)
            {
                ReferenceManager.Instance.GameData.playerCash -= cost;
                ReferenceManager.Instance.GameData.playerCapacity++;

                print("Player Capacity Upgraded: " + ReferenceManager.Instance.GameData.playerCapacity);
                ReferenceManager.Instance.taskHandler.OnTaskCompleted();
                ReferenceManager.Instance.SaveGameDataObserver();
                UiUpdate();
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("Not Enough Money");
            }


            if (SoundManager.instance)
            {
                SoundManager.instance.Play(SoundName.Upgrade);
            }
        }

        private void OnEnable()
        {
            UiUpdate();
        }

        void UiUpdate()
        {
            foreach (var btn in speedAdsButtons)
            {
                btn.SetActive(ReferenceManager.Instance.GameData.playerMovingSpeed < playerMovingSpeed.Length);
            }

            foreach (var btn in stackingAdsButtons)
            {
                btn.SetActive(ReferenceManager.Instance.GameData.stackingSpeed < playerStackingSpeed.Length);
            }

            foreach (var btn in capacityAdsButtons)
            {
                btn.SetActive(ReferenceManager.Instance.GameData.playerCapacity < playerCapacity.Length);
            }


            if (ReferenceManager.Instance.GameData.playerMovingSpeed < playerMovingSpeed.Length)
            {
                maxOutSpeed.gameObject.SetActive(false);
                playerMovingSpeedText.text =
                    playerMovingSpeed[Mathf.Max(0, ReferenceManager.Instance.GameData.playerMovingSpeed)] + "";
            }
            else
            {
                maxOutSpeed.gameObject.SetActive(true);
            }

            if (ReferenceManager.Instance.GameData.stackingSpeed < playerStackingSpeed.Length)
            {
                maxOutStack.gameObject.SetActive(false);
                playerStackingSpeedText.text =
                    playerStackingSpeed[Mathf.Max(0, ReferenceManager.Instance.GameData.stackingSpeed)] + "";
            }
            else
            {
                maxOutStack.gameObject.SetActive(true);
            }

            if (ReferenceManager.Instance.GameData.playerCapacity < playerCapacity.Length)
            {
                maxOutCapacity.gameObject.SetActive(false);
                playerCapacityText.text =
                    playerCapacity[Mathf.Max(0, ReferenceManager.Instance.GameData.playerCapacity)] + "";
            }
            else
            {
                maxOutCapacity.gameObject.SetActive(true);
            }

            for (int i = 0; i < machinePanel.Length; i++)
            {
                if (i < ReferenceManager.Instance.GameData.unlockedMachine)
                {
                    machinePanel[i].SetActive(true);
                }
                else
                {
                    machinePanel[i].SetActive(false);
                }
            }

            foreach (var btn in machineAdsButtons)
            {
                btn.SetActive(
                    ReferenceManager.Instance.GameData.gameEconomy.machineUpgradeIndex < machineUpgrade.Length - 1);
            }

            foreach (var btn in machine1AdsButtons)
            {
                btn.SetActive(
                    ReferenceManager.Instance.GameData.gameEconomy.machine1UpgradeIndex < machine1Upgrade.Length - 1);
            }

            foreach (var btn in machine2AdsButtons)
            {
                btn.SetActive(
                    ReferenceManager.Instance.GameData.gameEconomy.machine2UpgradeIndex < machine2Upgrade.Length - 1);
            }


            if (ReferenceManager.Instance.GameData.gameEconomy.machineUpgradeIndex < machine1Upgrade.Length - 1)
            {
                maxOutMachine.gameObject.SetActive(false);
                machineUpgradeText.text =
                    machineUpgrade[Mathf.Max(0, ReferenceManager.Instance.GameData.gameEconomy.machineUpgradeIndex)] +
                    "";
            }
            else
            {
                maxOutMachine.gameObject.SetActive(true);
            }

            if (ReferenceManager.Instance.GameData.gameEconomy.machine1UpgradeIndex < machine1Upgrade.Length - 1)
            {
                maxOutMachine1.gameObject.SetActive(false);
                machine1UpgradeText.text =
                    machine1Upgrade[Mathf.Max(0, ReferenceManager.Instance.GameData.gameEconomy.machine1UpgradeIndex)] +
                    "";
            }
            else
            {
                maxOutMachine1.gameObject.SetActive(true);
            }

            if (ReferenceManager.Instance.GameData.gameEconomy.machine2UpgradeIndex < machine2Upgrade.Length - 1)
            {
                maxOutMachine2.gameObject.SetActive(false);
                machine2UpgradeText.text =
                    machine2Upgrade[Mathf.Max(0, ReferenceManager.Instance.GameData.gameEconomy.machine2UpgradeIndex)] +
                    "";
            }
            else
            {
                maxOutMachine2.gameObject.SetActive(true);
            }


            CurrentMachine();

            ReferenceManager.OnPLayerGotUpgrade?.Invoke();
        }

        public void OnClickCross()
        {
            ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.MainControls);
            ReferenceManager.Instance.SaveGameDataObserver();
        }

        #endregion

        #region MachineUpgradeSection

        public GameObject[] machinePanel;
        public MachineParts[] machinesModels;

        public void CurrentMachine()
        {
            var gameEconomy = ReferenceManager.Instance.GameData.gameEconomy;

            // Deactivate all parts first
            DeactivateAllParts();

            // Then activate the upgraded parts
            ActivateMachinePart(0, gameEconomy.machineUpgradeIndex);
            ActivateMachinePart(1, gameEconomy.machine1UpgradeIndex);
            ActivateMachinePart(2, gameEconomy.machine2UpgradeIndex);
        }

        private void DeactivateAllParts()
        {
            // Iterate over each machine and deactivate all parts
            for (int i = 0; i < machinesModels.Length; i++)
            {
                for (int j = 0; j < machinesModels[i].machineParts.Length; j++)
                {
                    machinesModels[i].machineParts[j].SetActive(false);
                }
            }
        }

        private void ActivateMachinePart(int machineIndex, int upgradeIndex)
        {
            if (machineIndex < machinesModels.Length &&
                upgradeIndex < machinesModels[machineIndex].machineParts.Length)
            {
                machinesModels[machineIndex].machineParts[upgradeIndex].SetActive(true);
            }
        }

        public void UpgradeMachine0()
        {
            int machineUpgradeIndex = ReferenceManager.Instance.GameData.gameEconomy.machineUpgradeIndex;

            if (ReferenceManager.Instance.GameData.playerCash >= machineUpgrade[machineUpgradeIndex])
            {
                ReferenceManager.Instance.GameData.playerCash -= machineUpgrade[machineUpgradeIndex];
                UpgradeMachine(0);
            }else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("Not Enough Money");
            }

            UiUpdate();
        }

        public void UpgradeMachine1()
        {
            int machineUpgradeIndex = ReferenceManager.Instance.GameData.gameEconomy.machine1UpgradeIndex;

            if (ReferenceManager.Instance.GameData.playerCash >= machineUpgrade[machineUpgradeIndex])
            {
                ReferenceManager.Instance.GameData.playerCash -= machineUpgrade[machineUpgradeIndex];
                UpgradeMachine(1);
            }else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("Not Enough Money");
            }

            UiUpdate();
        }

        public void UpgradeMachine2()
        {
            int machineUpgradeIndex = ReferenceManager.Instance.GameData.gameEconomy.machine2UpgradeIndex;

            if (ReferenceManager.Instance.GameData.playerCash >= machineUpgrade[machineUpgradeIndex])
            {
                ReferenceManager.Instance.GameData.playerCash -= machineUpgrade[machineUpgradeIndex];
                UpgradeMachine(2);
            }else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("Not Enough Money");
            }

            UiUpdate();
        }

        public void UpgradeMachine1Rewarded()
        {
            TssAdsManager._Instance.ShowRewardedAd(() =>
            {
                int machineUpgradeIndex = ReferenceManager.Instance.GameData.gameEconomy.machineUpgradeIndex;

                if (ReferenceManager.Instance.GameData.playerCash >= machineUpgrade[machineUpgradeIndex])
                {
                    UpgradeMachine(0);
                }
            }, "Machine1Upgrade");
            
            UiUpdate();
        }

        public void UpgradeMachine2Rewarded()
        {
            TssAdsManager._Instance.ShowRewardedAd(() =>
            {
                int machineUpgradeIndex = ReferenceManager.Instance.GameData.gameEconomy.machineUpgradeIndex;

                if (ReferenceManager.Instance.GameData.playerCash >= machineUpgrade[machineUpgradeIndex])
                {
                    UpgradeMachine(1);
                }
            }, "Machine2Upgrade");
            UiUpdate();
        }

        public void UpgradeMachine3Rewarded()
        {
            TssAdsManager._Instance.ShowRewardedAd(() =>
            {
                int machineUpgradeIndex = ReferenceManager.Instance.GameData.gameEconomy.machineUpgradeIndex;

                if (ReferenceManager.Instance.GameData.playerCash >= machineUpgrade[machineUpgradeIndex])
                {
                    UpgradeMachine(2);
                }
            }, "Machine3Upgrade");
            UiUpdate();
        }

        void UpgradeMachine(int machineIndex)
        {
            switch (machineIndex)
            {
                case 0:
                    if (ReferenceManager.Instance.GameData.gameEconomy.machineUpgradeIndex <
                        machineUpgrade.Length)
                    {
                        ReferenceManager.Instance.GameData.gameEconomy.machineUpgradeIndex++;
                        particle[0].Play();
                    }

                    ReferenceManager.Instance.machineUpgradeManager.UpgradeMachine(0);
                    break;
                case 1:
                    if (ReferenceManager.Instance.GameData.gameEconomy.machine1UpgradeIndex <
                        machine1Upgrade.Length)
                    {
                        ReferenceManager.Instance.GameData.gameEconomy.machine1UpgradeIndex++;
                        particle[1].Play();
                    }

                    ReferenceManager.Instance.machineUpgradeManager.UpgradeMachine(1);

                    break;
                case 2:
                    if (ReferenceManager.Instance.GameData.gameEconomy.machine2UpgradeIndex <
                        machine2Upgrade.Length)
                    {
                        ReferenceManager.Instance.GameData.gameEconomy.machine2UpgradeIndex++;
                        particle[2].Play();
                    }

                    ReferenceManager.Instance.machineUpgradeManager.UpgradeMachine(2);

                    break;
            }

            CurrentMachine();
            if (SaveAndLoadSystem.Instance)
                SaveAndLoadSystem.Instance.SaveGame();
        }

        #endregion
    }
}
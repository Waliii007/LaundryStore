using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class HRUpgradeEventListener : MonoBehaviour
    {
        public Text playerMovingSpeedText;
        public int[] playerMovingSpeed;

        public Text playerStackingSpeedText;
        public int[] playerStackingSpeed;

        public Text playerCapacityText;
        public int[] playerCapacity;

        public Button maxOutSpeed;
        public Button maxOutStack;
        public Button maxOutCapacity;

        public GameObject[] speedAdsButtons;
        public GameObject[] stackingAdsButtons;
        public GameObject[] capacityAdsButtons;

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

            ReferenceManager.OnPLayerGotUpgrade?.Invoke();
        }

        public void OnClickCross()
        {
            ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.MainControls);
            ReferenceManager.Instance.SaveGameDataObserver();
        }
    }
}
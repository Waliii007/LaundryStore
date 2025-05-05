using System;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class SnackbarManager : MonoBehaviour
    {
        public enum SnackbarType
        {
            None,
            Basic,
            Pro,
            Max
        }

        [Header("Pack Settings")] [SerializeField]
        private int[] packPrices = { 1000, 2000, 3000 };

        [SerializeField] private float[] tipMultipliers = { 1.1f, 1.25f, 1.5f };
        [SerializeField] private int[] boostDurations = { 50, 80, 100 }; // Number of customers

        [Header("UI")] [SerializeField] private Image snackbarFillImage; // Replaced Slider with Image

        [Header("Debug / Test Settings")] public float baseTip = 10f;

        private SnackbarType activePack = SnackbarType.None;
        public float currentMultiplier = 1f;
        private int remainingBoostedCustomers = 0;
        public int currentBoostMax = 0;
        private bool[] purchased = new bool[3];

        private void Start()
        {
            ResetFillImage();
            UpdateCanvasState(); // Manual call instead of Update()
        }

        private void UpdateCanvasState()
        {
            MachineCanvasStates newState = currentBoostMax == 0
                ? MachineCanvasStates.RefillNeeded
                : MachineCanvasStates.Full;

            snackbarCanvas.CanvasStateChanger(newState);
        }

        public SnackbarCanvas snackbarCanvas;

        public void BuyPack(int index)
        {
            if (index < 0 || index >= packPrices.Length)
            {
                Debug.LogError("Invalid pack index.");
                return;
            }

            if (purchased[index])
            {
                Debug.Log("Pack already purchased.");
                return;
            }

            if (ReferenceManager.Instance.GameData.playerCash >= packPrices[index])
            {
                ReferenceManager.Instance.GameData.playerCash -= packPrices[index];
                purchased[index] = true;

                ActivatePack((SnackbarType)(index + 1), index);
                ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.MainControls);
            }
            else
            {
                
                ReferenceManager.Instance.notificationHandler.ShowNotification("Not enough currency.");
            }
        }

        public void ActivatePack(SnackbarType type, int index)
        {
            activePack = type;
            currentMultiplier = tipMultipliers[index];
            remainingBoostedCustomers = boostDurations[index];
            currentBoostMax = boostDurations[index];
            UpdateFillImage();
            UpdateCanvasState();
            ReferenceManager.Instance.notificationHandler.ShowNotification($"{type} Cash Boost Activated ");
            
            
            // Update canvas on pack activation
        }

        public void OnCustomerServed()
        {
            if (activePack == SnackbarType.None) return;

            remainingBoostedCustomers--;
            UpdateFillImage();

            if (remainingBoostedCustomers <= 0)
            {
                EndBoost();
                UpdateCanvasState(); // Update canvas after boost ends
            }
        }

        

        private void EndBoost()
        {
            activePack = SnackbarType.None;
            currentMultiplier = 1f;
            ResetFillImage();
        }

        private void ResetFillImage()
        {
            snackbarFillImage.fillAmount = 0f;
            currentBoostMax = 0;
        }

        private void UpdateFillImage()
        {
            if (currentBoostMax > 0)
            {
                snackbarFillImage.fillAmount = (float)remainingBoostedCustomers / currentBoostMax;
            }
            else
            {
                snackbarFillImage.fillAmount = 0f;
            }
        }


        // Optional: expose public UI hooks
        public void BuyBasicPack() => BuyPack(0);
        public void BuyProPack() => BuyPack(1);
        public void BuyMaxPack() => BuyPack(2);

        public float GetCurrentMultiplier() => currentMultiplier;
        public bool IsBoostActive() => activePack != SnackbarType.None;
    }
}
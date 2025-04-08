using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LaundaryMan
{
    public class WeeklyCashDeduction : MonoBehaviour
    {
        public int playerCash = 1000; // Starting cash
        public int deductionAmount = 100; // Amount to deduct weekly
        private string lastDeductionKey = "LastDeductionTime";

        void Start()
        {
            int cleanAi = ReferenceManager.Instance.GameData.cleanAIUnlocked == -1
                ? 0
                : ReferenceManager.Instance.GameData.cleanAIUnlocked;
            int dirtyBoxAiUnlocked = ReferenceManager.Instance.GameData.dirtyBoxAiUnlocked == -1
                ? 0
                : ReferenceManager.Instance.GameData.dirtyBoxAiUnlocked;
            ;
            deductionAmount = (cleanAi + dirtyBoxAiUnlocked) * 1000;
            CheckAndDeductCash();
        }

        void CheckAndDeductCash()
        {
            if (PlayerPrefs.HasKey(lastDeductionKey))
            {
                long lastDeductionTime = Convert.ToInt64(PlayerPrefs.GetString(lastDeductionKey));
                DateTime lastDeductionDate = DateTime.FromBinary(lastDeductionTime);
                TimeSpan timeSinceLastDeduction = DateTime.Now - lastDeductionDate;

                if (timeSinceLastDeduction.TotalDays >= 7)
                {
                    DeductCash();
                }
            }
            else
            {
                DeductCash();
            }
        }

        void DeductCash()
        {
            if (playerCash > 0)
            {
                playerCash = Mathf.Max(0, playerCash - deductionAmount);
                PlayerPrefs.SetString(lastDeductionKey, DateTime.Now.ToBinary().ToString());
                PlayerPrefs.Save();
                Debug.Log("Weekly deduction applied. New cash balance: " + playerCash);
            }
        }
    }
}
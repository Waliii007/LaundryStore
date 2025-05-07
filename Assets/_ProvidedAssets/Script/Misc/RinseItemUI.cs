using System;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class RinseItemUI : MonoBehaviour
    {
        public Text greenRinseText;
        public Text blueRinseText;
        public Text redRinseText;
        public RinsePurchase purchase;
        public int index;
        public GameObject crossButton;
        public GameObject tutorialObject;
        private void OnEnable()
        {
            UpdateRinseUI();
            crossButton.SetActive(ReferenceManager.Instance.GameData.isTutorialCompleted);
            tutorialObject.SetActive(!ReferenceManager.Instance.GameData.isTutorialCompleted);
        }

        public bool once;

        public void GreenRinse()
        {
            purchase.OnUseGreenButton(index);
            if (!ReferenceManager.Instance.GameData.isTutorialCompleted && !once)
            {
                ReferenceManager.Instance.tutorialHandler.TaskCompleted();
                ReferenceManager.Instance.tutorialHandler.CompleteTutorial();
            }
        }

        public void BlueRinse()
        {
            purchase.OnUseBlueButton(index);
        }

        public void RedRinse()
        {
            purchase.OnUseRedButton(index);
        }

        public void UpdateRinseUI()
        {
            greenRinseText.text = "X" + purchase.GetRinseCount(RinseType.Green).ToString();
            blueRinseText.text = "X" + purchase.GetRinseCount(RinseType.Blue).ToString();
            redRinseText.text = "X" + purchase.GetRinseCount(RinseType.Red).ToString();
        }
    }
}
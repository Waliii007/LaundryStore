using System;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class DetergentItemUI : MonoBehaviour
    {
        public Text greenDetergentText;
        public Text blueDetergentText;
        public Text redDetergentText;
        public DetergentPurchase purchase;
        public int index;
        public GameObject tutorialPanel;
        public GameObject crossButton;

        private void OnEnable()
        {
            crossButton.SetActive(!ReferenceManager.Instance.GameData.isTutorialCompleted);

            UpdateDetergentUI();
            tutorialPanel.SetActive(!ReferenceManager.Instance.GameData.isTutorialCompleted);
        }

        public bool once;

        public void GreenDetergent()
        {
            purchase.OnUseGreenButton(index);
            if (!ReferenceManager.Instance.GameData.isTutorialCompleted && !once)
            {
                ReferenceManager.Instance.tutorialHandler.TaskCompleted();
            }
        }

        public void BlueDetergent()
        {
            
            purchase.OnUseBlueButton(index);
        }

        public void RedDetergent()
        {
            purchase.OnUseRedButton(index);
        }

        public void UpdateDetergentUI()
        {
            greenDetergentText.text = "X" + purchase.GetDetergentCount(DetergentType.Green).ToString();
            blueDetergentText.text = "X" + purchase.GetDetergentCount(DetergentType.Blue).ToString();
            redDetergentText.text = "X" + purchase.GetDetergentCount(DetergentType.Red).ToString();
        }
    }
}
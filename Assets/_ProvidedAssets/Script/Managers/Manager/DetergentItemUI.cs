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

        private void OnEnable()
        {
            UpdateDetergentUI();
        }

        public void GreenDetergent()
        {
            purchase.OnUseGreenButton(index);
        }

        public void BlueDetergent()
        {
            purchase.OnUseGreenButton(index);
        }

        public void RedDetergent()
        {
            purchase.OnUseGreenButton(index);
        }

        public void UpdateDetergentUI()
        {
            greenDetergentText.text = "X" + purchase.GetDetergentCount(DetergentType.Green).ToString();
            blueDetergentText.text = "X" + purchase.GetDetergentCount(DetergentType.Blue).ToString();
            redDetergentText.text = "X" + purchase.GetDetergentCount(DetergentType.Red).ToString();
        }
    }
}
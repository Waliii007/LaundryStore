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

        private void OnEnable()
        {
            UpdateRinseUI();
        }

        public void GreenRinse()
        {
            purchase.OnUseGreenButton(index);
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
using System;
using UnityEngine;

namespace LaundaryMan
{
    public class RefillPanelEventListener : MonoBehaviour
    {
        public WashingMachineDropper[] washingMachineDropper;
        public PressBasketHandler[] pressMachine;

        public GameObject[] washingMachinePanel;


        private void OnEnable()
        {
            for (int i = 0; i < washingMachineDropper.Length; i++)
            {
                if (i < ReferenceManager.Instance.GameData.unlockedMachine)
                    washingMachinePanel[i].SetActive(true);
                else
                    washingMachinePanel[i].SetActive(false);
            }
        }


        public void Machine1Refill()
        {
            washingMachineDropper[0].Refill();
        }

        public void Machine2Refill()
        {
            washingMachineDropper[1].Refill();
        }

        public void Machine3Refill()
        {
            washingMachineDropper[2].Refill();
        }

        public void Rins1Refill()
        {
            pressMachine[0].RefillRin();
        }

        public void Rins2Refill()
        {
            pressMachine[1].RefillRin();
        }

        public void Rins3Refill()
        {
            pressMachine[2].RefillRin();
        }

        public void Cross()
        {
            ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.MainControls);
        }
    }
}
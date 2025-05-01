using System;
using UnityEngine;

namespace LaundaryMan
{
    [System.Serializable]
    public class Machine
    {
        public MachineParts[] partsLevels; // Each index represents a level of upgrade
    }

    public class MachineUpgradeManager : MonoBehaviour
    {
        public Machine[] machines; // Size should be 3 for your case

        private void OnEnable()
        {
            CurrentMachine(0, ReferenceManager.Instance.GameData.gameEconomy.machineUpgradeIndex);
            CurrentMachine(1, ReferenceManager.Instance.GameData.gameEconomy.machine1UpgradeIndex);
            CurrentMachine(2, ReferenceManager.Instance.GameData.gameEconomy.machine2UpgradeIndex);
        }

        public void CurrentMachine(int machineIndex, int updateIndex)
        {
            int upgradeIndex = updateIndex;

            // Loop through all levels
            for (int i = 0; i < machines[machineIndex].partsLevels.Length; i++)
            {
                bool isActive = (i == upgradeIndex);
                foreach (var part in machines[machineIndex].partsLevels[i].machineParts)
                {
                    part.gameObject.SetActive(isActive);
                }
            }
        }

        public void UpgradeMachine(int machineIndex)
        {
            int upgradeIndex = ReferenceManager.Instance.GameData.gameEconomy.machineUpgradeIndex;

            switch (machineIndex)
            {
                case 0:
                    upgradeIndex = ReferenceManager.Instance.GameData.gameEconomy.machineUpgradeIndex;
                    break;
                case 1:
                    upgradeIndex = ReferenceManager.Instance.GameData.gameEconomy.machine1UpgradeIndex;
                    break;
                case 2:
                    upgradeIndex = ReferenceManager.Instance.GameData.gameEconomy.machine2UpgradeIndex;
                    break;
            }

            // Disable previous level parts if exists
            if (upgradeIndex > 0 && upgradeIndex - 1 < machines[machineIndex].partsLevels.Length)
            {
                foreach (var part in machines[machineIndex].partsLevels[upgradeIndex - 1].machineParts)
                {
                    part.gameObject.SetActive(false);
                }
            }

            // Enable current level parts
            if (upgradeIndex < machines[machineIndex].partsLevels.Length)
            {
                foreach (var part in machines[machineIndex].partsLevels[upgradeIndex].machineParts)
                {
                    part.gameObject.SetActive(true);
                }
            }
        }

        // Optionally call all upgrades
        public void UpgradeAllMachines()
        {
            for (int i = 0; i < machines.Length; i++)
            {
                UpgradeMachine(i);
            }
        }
    }

    [Serializable]
    public class MachineParts
    {
        public GameObject[] machineParts;
    }
}
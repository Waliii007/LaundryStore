using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace LaundaryMan
{
    public class MachineManager : MonoBehaviour
    {
        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            
            for (int i = 0; i < ReferenceManager.Instance.GameData.unlockedMachine; i++)
            {
                MachineByIndex(i);
            }
        }

        public void MachineByIndex(int index)
        {
            StartCoroutine(MachineUnlockingByIndex(index));
        }

        IEnumerator MachineUnlockingByIndex(int i)
        {
            machines[i].SetActive(true);
            ReferenceManager.Instance.queueSystem.gameObject.SetActive(true);
            yield return null;
        }

        public IEnumerator MachineUnlocking(int i)
        {
            machines[i].SetActive(true);
            ReferenceManager.Instance.queueSystem.gameObject.SetActive(true);
            yield return null;
        }

        public List<GameObject> machines = new List<GameObject>();
    }
}
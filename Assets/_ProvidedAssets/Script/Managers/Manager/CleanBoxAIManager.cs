using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LaundaryMan
{
    public class CleanBoxAIManager : MonoBehaviour
    {
        private Queue<AICleanClothes.Task> taskQueue = new Queue<AICleanClothes.Task>();
        [SerializeField] private List<AICleanClothes> availableAgents = new List<AICleanClothes>();
        [SerializeField] private GameObject[] dropPosition;
        [SerializeField] private AICleanClothes[] aiPlayers;

        public void AddTask(GameObject pickPosition, WashingMachineDropper dropper)
        {
            int i = Random.Range(0, ReferenceManager.Instance.GameData.unlockedMachine);
            print(i);
            AICleanClothes.Task newTask = new AICleanClothes.Task(pickPosition, dropPosition[i]);
            newTask.dropper = dropper;
            taskQueue.Enqueue(newTask);
            AssignTask();
        }
        
        private void OnEnable()
        {
            for (int j = 0; j < ReferenceManager.Instance.GameData.cleanAIUnlocked; j++)
            {
                aiPlayers[j].gameObject.SetActive(true);
            }
        }

        public void RegisterAgent(AICleanClothes agent)
        {
            availableAgents.Add(agent);
        }

        public void UnregisterAgent(AICleanClothes agent)
        {
            availableAgents.Remove(agent);
        }

        private void AssignTask()
        {
            if (taskQueue.Count > 0 && availableAgents.Count > 0)
            {
                AICleanClothes agent = availableAgents[0];
                availableAgents.RemoveAt(0);
                AICleanClothes.Task currentTask = taskQueue.Dequeue();
                currentTask.dropper.isOccupied = true;
                agent.AssignTask(currentTask);
            }
        }

        public void UnlockAi(int index)
        {
            StartCoroutine(UnlockSpawnAndEnqueue(index));
        }

        IEnumerator UnlockSpawnAndEnqueue(int index)
        {
            for (int i = 0; i < index; i++)
            {
                aiPlayers[i].gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(.01f);
        }
    }
}
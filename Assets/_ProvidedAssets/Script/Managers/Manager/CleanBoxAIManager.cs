using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LaundaryMan
{
    public class CleanBoxAIManager : MonoBehaviour
    {
        public List<AICleanClothes.Task> taskList = new List<AICleanClothes.Task>();
        [SerializeField] private List<AICleanClothes> availableAgents = new List<AICleanClothes>();
        [SerializeField] private GameObject[] dropPosition;
        [SerializeField] private AICleanClothes[] aiPlayers;

        public void AddTask(GameObject pickPosition, WashingMachineDropper dropper)
        {
            // Check if a similar task already exists
            foreach (var task in taskList)
            {
                if (task.PickPosition == pickPosition && task.dropper == dropper)
                {
                    return; // Task already exists, don't add again
                }
            }

            int i = Random.Range(0, ReferenceManager.Instance.GameData.unlockedMachine);
            AICleanClothes.Task newTask = new AICleanClothes.Task(pickPosition, dropPosition[i]);
            newTask.dropper = dropper;
            taskList.Add(newTask);
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

        public bool IsTaskAlreadyAdded(PressingClothPickingHandler machine)
        {
            foreach (var task in taskList)
            {
                if (task.dropper == machine)
                    return true;
            }

            return false;
        }

        public void UnregisterAgent(AICleanClothes agent)
        {
            availableAgents.Remove(agent);
        }

        public void AssignTask()
        {
            while (taskList.Count > 0 && availableAgents.Count > 0)
            {
                AICleanClothes agent = availableAgents[0];
                availableAgents.RemoveAt(0);
                AICleanClothes.Task currentTask = taskList[0];
                taskList.RemoveAt(0);
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
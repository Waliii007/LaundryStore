using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace LaundaryMan
{
    public class DirtyBoxAiManager : MonoBehaviour
    {
        private Queue<DirtyBoxAi.Task> taskQueue = new Queue<DirtyBoxAi.Task>();
        [SerializeField] private List<DirtyBoxAi> availableAgents = new List<DirtyBoxAi>();
        [SerializeField] private GameObject[] dropPosition;
        [SerializeField] private DirtyBoxAi[] aiPlayers;

        public void AddTask(GameObject pickPosition, WashingMachineDropper dropper, int index)
        {
            if (ReferenceManager.Instance.GameData.unlockedMachine == -1)
                return;

            DirtyBoxAi.Task newTask = new DirtyBoxAi.Task(pickPosition, dropPosition[index]);
            newTask.dropper = dropper;
            taskQueue.Enqueue(newTask);
            AssignTask();
        }

        private void OnEnable()
        {
            for (int j = 0; j < ReferenceManager.Instance.GameData.dirtyBoxAiUnlocked; j++)
            {
                aiPlayers[j].gameObject.SetActive(true);
            }
        }

        public void RegisterAgent(DirtyBoxAi agent)
        {
            availableAgents.Add(agent);
        }

        public void UnregisterAgent(DirtyBoxAi agent)
        {
            availableAgents.Remove(agent);
        }

        public void AssignTask()
        {
            if (taskQueue.Count > 0 && availableAgents.Count > 0)
            {
                DirtyBoxAi agent = availableAgents[0];
                availableAgents.RemoveAt(0);
                DirtyBoxAi.Task currentTask = taskQueue.Dequeue();
                currentTask.dropper.isOccupiedbyDirty = true;
                agent.AssignTask(currentTask);
            }
        }

        public void UnlockAi(int index)
        {
            StartCoroutine(UnlockSpawnAndEnqueue(index));
        }

        public WaitForSeconds waitForSeconds = new WaitForSeconds(.01f);

        IEnumerator UnlockSpawnAndEnqueue(int index)
        {
            for (int i = 0; i < index; i++)
            {
                aiPlayers[i].gameObject.SetActive(true);
            }

            yield return waitForSeconds;
        }
    }
}

[System.Serializable]
public class DirtyTask
{
    public GameObject PickPosition;
    public GameObject DropPosition;

    public DirtyTask(GameObject pick, GameObject drop)
    {
        PickPosition = pick;
        DropPosition = drop;
    }
}
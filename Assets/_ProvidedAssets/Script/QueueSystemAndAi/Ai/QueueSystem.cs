using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace LaundaryMan
{
    public class QueueSystem : MonoBehaviour
    {
        #region Queue

        public List<QueuePoint> dirtyQueuePoints;
        public List<QueuePoint> washedQueuePoints;
        public Queue<Ai> _dirtyClothQueueAi = new Queue<Ai>();
        public Queue<Ai> _washedClothQueueAi = new Queue<Ai>();

        public Ai[] aiPrefab;
        public GameObject aiSpawnPoint;
        public GameObject aiParent;
        public GameObject tooManyCustomer;
        public GameObject exitPoint;

        public bool isGameEnd;
        private Queue<Ai> NeedToEnqueue = new Queue<Ai>();

        public bool _canMove => _washedClothQueueAi.Count < washedQueuePoints.Count;
        public bool _canSpawn => _dirtyClothQueueAi.Count < dirtyQueuePoints.Count;

        private void Start()
        {
            StartCoroutine(ReferenceManager.Instance.GameData.isTutorialCompleted
                ? SpawnAndEnqueue()
                : SpawnAndEnqueue(1));
            StartCoroutine(CheckingCustomerAmount());
        }

        public void OnTutorialCompleted()
        {
            StartCoroutine(SpawnAndEnqueue());
        }

        public WaitForSeconds waitForSeconds;
        public WaitForSeconds _waitForSeconds;

        private IEnumerator CheckingCustomerAmount()
        {
            _waitForSeconds = new WaitForSeconds(.1f);
            while (!isGameEnd)
            {
                // print(_dirtyClothQueueAi.Count + ":" + (_dirtyClothQueueAi.Count < dirtyQueuePoints.Count));
                tooManyCustomer.SetActive(!_canSpawn && !_canMove);
                yield return _waitForSeconds;
            }
        }

        private IEnumerator SpawnAndEnqueue()
        {
            waitForSeconds = new WaitForSeconds(Random.Range(5, 10));
            while (!isGameEnd)
            {
                // print(_dirtyClothQueueAi.Count + ":" + (_dirtyClothQueueAi.Count < dirtyQueuePoints.Count));
                if (!_canSpawn) yield return new WaitUntil(() => _canSpawn || isGameEnd);
                if (isGameEnd) yield break;
                SpawnAi();
                yield return waitForSeconds;
            }
        }

        public void SpawnOnButton()
        {
            StartCoroutine(SpawnAndEnqueue(1));
        }

        public WaitForSeconds waitForSecondsAiSpawn = new WaitForSeconds(1f);

        private IEnumerator SpawnAndEnqueue(int count)
        {
            for (int j = 0; j < count; j++)
            {
                if (!_canSpawn) yield break;
                SpawnAi();
                yield return waitForSecondsAiSpawn;
            }
        }

        private Ai ais;

        private void SpawnAi()
        {
            ais = Instantiate(aiPrefab[0], aiParent.transform);
            ais.transform.SetPositionAndRotation(aiSpawnPoint.transform.position, aiSpawnPoint.transform.rotation);
            ais.gameObject.SetActive(true);
            _dirtyClothQueueAi.Enqueue(ais);

            AssignToQueue(ais, dirtyQueuePoints);
        }

        private void AssignToQueue(Ai ai, List<QueuePoint> queuePoints)
        {
            foreach (var queuePoint in queuePoints)
            {
                if (!queuePoint.occupied)
                {
                    ai.SetDestination(queuePoint.queuePoint);
                    queuePoint.occupied = true;
                    return;
                }
            }
        }

        public void DequeueOnButton()
        {
            ShiftToWaitingAndDequeue();
        }

        private void ShiftToWaitingAndDequeue()
        {
            if (_dirtyClothQueueAi.Count == 0) return;

            Ai removedAi = _dirtyClothQueueAi.Dequeue();
            dirtyQueuePoints[0].occupied = false;

            if (_canMove)
            {
                ShiftToQueue(removedAi);
                MovingQueue();
            }
            else
            {
                NeedToEnqueue.Enqueue(removedAi);
                StartCoroutine(CanMove());
            }
        }

        private IEnumerator CanMove()
        {
            yield return new WaitUntil(() => _canMove);
            while (NeedToEnqueue.Count > 0)
            {
                ShiftToQueue(NeedToEnqueue.Dequeue());
                MovingQueue();
                yield return null;
            }
        }

        private void ShiftToQueue(Ai ai)
        {
            AssignToQueue(ai, washedQueuePoints);
            _washedClothQueueAi.Enqueue(ai);
        }

        private void MovingQueue()
        {
            ResetQueue(dirtyQueuePoints);
            foreach (var ai in _dirtyClothQueueAi) AssignToQueue(ai, dirtyQueuePoints);
        }

        private void MovingWashedQueue()
        {
            ResetQueue(washedQueuePoints);
            foreach (var ai in _washedClothQueueAi)
            {
                if (ai.TryGetComponent(out CustomerAI customerAI))
                {
                    customerAI.customerObjectToStack.myClothFragment.Clear();
                }

                AssignToQueue(ai, washedQueuePoints);
            }
        }

        private void ResetQueue(List<QueuePoint> queuePoints)
        {
            foreach (var queuePoint in queuePoints) queuePoint.occupied = false;
        }

        private Ai removedAi;

        public void DequeueAndDestroyWashedQueue()
        {
            if (_washedClothQueueAi.Count == 0) return;

            removedAi = _washedClothQueueAi.Dequeue();
            removedAi.SetDestination(exitPoint);
            washedQueuePoints[0].occupied = false;

            float destroyTime = Vector3.Distance(exitPoint.transform.position, removedAi.transform.position) /
                                removedAi.speed();

            Destroy(removedAi.gameObject, destroyTime);

            MovingWashedQueue();
        }

        private void DestroyAi(float time)
        {
        }

        #endregion
    }

    [Serializable]
    public class QueuePoint
    {
        public bool occupied;
        public GameObject queuePoint;
    }
}
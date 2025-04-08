using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Invector.vCharacterController;
using LaundaryMan;
using Unity.Entities.Content;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LaundryMan
{
    public class DirtyBasketTrigger : Dropper
    {
        public Stack<ClothFragment> clothToWash = new Stack<ClothFragment>();
        public GameObject stackStarter;
        public int maxClothToStack = 5;
        public float stackTime = 1f;
        private bool isPlayerInside = false;
        private Coroutine aiCoroutine;
        private Coroutine playerCoroutine;
        private Coroutine _aiCoroutine;
        private Queue<Collider> aiQueue = new Queue<Collider>();
        public bool isOccupied = false;
        public Action InBasketHaveElements;
        private Coroutine loopCheckCoroutine;
        private int lastClothCount;

        public void StartCheckingLoop(int n) //Calling from Dirty cloths AI worker
        {
            if (loopCheckCoroutine != null)
            {
                StopCoroutine(loopCheckCoroutine);
            }

            loopCheckCoroutine = StartCoroutine(CheckLoopStop(n));
        }

        private IEnumerator CheckLoopStop(int n)
        {
            lastClothCount = clothToWash.Count;
            float timer = 0;

            while (timer < n)
            {
                yield return new WaitForSeconds(1f);
                if (clothToWash.Count != lastClothCount)
                {
                    lastClothCount = clothToWash.Count;
                    timer = 0; // Reset timer if count changes
                }
                else
                {
                    timer += 1f;
                }
            }

            checkifLOopStop(); // Call function if count remains unchanged for n seconds
        }

        public bool CanPickUp()
        {
            return clothToWash.Count > 0;
        }

        private void OnEnable()
        {
            ReferenceManager.OnPLayerGotUpgrade += OnUpgrade;
        }

        public int Count()
        {
            return clothToWash.Count;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent(out AiStackManager aIStackManager))
                {
                    isPlayerInside = true;
                    _aiCoroutine ??= StartCoroutine(HandlePlayerInteraction(aIStackManager));
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("AI"))
            {
                if (!aiQueue.Contains(other))
                    aiQueue.Enqueue(other);

                if (aiCoroutine == null)
                {
                    aiCoroutine = StartCoroutine(HandleAIDelay());
                }
            }
            else if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent(out PlayerStackManager playerStackManager) &&
                    other.TryGetComponent(out vThirdPersonInput tpi))
                {
                    isPlayerInside = true;


                    if (playerCoroutine == null)
                    {
                        playerCoroutine = StartCoroutine(HandlePlayerInteraction(playerStackManager));
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInside = false;
                if (playerCoroutine != null)
                {
                    StopCoroutine(playerCoroutine);
                    playerCoroutine = null;
                }

                if (_aiCoroutine != null)
                {
                    StopCoroutine(_aiCoroutine);
                    _aiCoroutine = null;
                }

                if (aiQueue.Count > 0 && aiCoroutine == null)
                {
                    aiCoroutine = StartCoroutine(HandleAIDelay());
                }
            }
        }

        private IEnumerator HandlePlayerInteraction(AiStackManager playerStackManager)
        {
            while (isPlayerInside)
            {
                if (clothToWash.Count > 0)
                {
                    var cloth = clothToWash.Pop();
                    cloth.transform.SetParent(playerStackManager.stackStarter.transform);

                    Vector3 targetPosition = (playerStackManager.ClothStack.Count == 0)
                        ? playerStackManager.stackStarter.transform.position
                        : playerStackManager.ClothStack.Peek().nextPosition.transform.position;

                    cloth.transform.DOJump(targetPosition, 1f, 1, stackTime).OnComplete(() =>
                    {
                        playerStackManager.ClothStack.Push(cloth);
                        RearrangeStack(playerStackManager);
                        ArrangeStack();
                    });
                    playerStackManager.SetIk(1);

                    yield return new WaitForSeconds(stackTime);
                }
                else
                {
                    yield return null;
                }
            }
        }

        public int limit = 7;

        private IEnumerator HandlePlayerInteraction(PlayerStackManager playerStackManager)
        {
            playerStackManager.TryGetComponent(out vThirdPersonInput tpi);
            if (tpi)
                yield return new WaitUntil(() => !tpi.IsMoving());
            while (isPlayerInside)
            {
                if (clothToWash.Count > 0 && playerStackManager.ClothStack.Count < limit)
                {
                    var cloth = clothToWash.Pop();
                    cloth.transform.SetParent(playerStackManager.stackStarter.transform);

                    Vector3 targetPosition = (playerStackManager.ClothStack.Count == 0)
                        ? playerStackManager.stackStarter.transform.position
                        : playerStackManager.ClothStack.Peek().nextPosition.transform.position;

                    cloth.transform.DOJump(targetPosition, 1f, 1, stackTime).OnComplete(() =>
                    {
                        playerStackManager.ClothStack.Push(cloth);
                        RearrangeStack(playerStackManager);
                        ArrangeStack();
                    });

                    playerStackManager.SetIk(1);
                    if (SoundManager.instance)
                    {
                        SoundManager.instance.Play(SoundName.Pick);
                    }

                    yield return new WaitForSeconds(stackTime);
                }
                else
                {
                    yield return null;
                }
            }
        }

        private void RearrangeStack(PlayerStackManager playerStackManager)
        {
            Vector3 startPosition = playerStackManager.stackStarter.transform.position;
            List<ClothFragment> tempList = playerStackManager.ClothStack.ToList();
            tempList.Reverse();
            for (int i = 0; i < tempList.Count; i++)
            {
                Vector3 targetPosition = (i == 0) ? startPosition : tempList[i - 1].nextPosition.transform.position;
                tempList[i].transform.transform.SetPositionAndRotation(targetPosition, quaternion.identity);
            }
        }

        public enum UpgradeIndex
        {
            Level1,
            Level2,
            Level3,
        }

        public enum CollectSpeedUpgradeIndex
        {
            Level1,
            Level2,
            Level3,
        }

        public void OnUpgrade()
        {
            CollectSpeedUpgradeIndex index = (CollectSpeedUpgradeIndex)ReferenceManager.Instance.GameData.stackingSpeed;
            switch (index)
            {
                case CollectSpeedUpgradeIndex.Level1:

                    break;
                case CollectSpeedUpgradeIndex.Level2:
                    break;
                case CollectSpeedUpgradeIndex.Level3:
                    break;
            }

            UpgradeIndex indexes = (UpgradeIndex)ReferenceManager.Instance.GameData.playerCapacity;

            switch (indexes)
            {
                case UpgradeIndex.Level1:
                    stackTime = 0.3f;
                    break;
                case UpgradeIndex.Level2:
                    stackTime = 0.1f;
                    break;
                case UpgradeIndex.Level3:
                    stackTime = 0.09f;
                    break;
            }
        }

        private void ArrangeStack()
        {
            if (clothToWash.Count == 0) return;

            Stack<ClothFragment> tempStack = new Stack<ClothFragment>();
            Vector3 lastPosition = stackStarter.transform.position;

            while (clothToWash.Count > 0)
            {
                ClothFragment clothFragment = clothToWash.Pop();
                clothFragment.transform.SetParent(stackStarter.transform);
                clothFragment.transform.position = lastPosition;
                lastPosition = clothFragment.nextPosition.transform.position;
                tempStack.Push(clothFragment);
            }

            while (tempStack.Count > 0)
            {
                clothToWash.Push(tempStack.Pop());
            }
        }

        private IEnumerator HandleAIDelay()
        {
            while (aiQueue.Count > 0)
            {
                // {isPlayerInside ||
                if (clothToWash.Count >= maxClothToStack)
                {
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }

                yield return new WaitForSeconds(0.5f);

                Collider aiCollider = aiQueue.Peek();

                if (aiCollider && aiCollider.TryGetComponent(out CustomerAI ai) &&
                    ai.customerObjectToStack)
                {
                    if (ai.customerObjectToStack.myClothFragment.Count > 0)
                        for (int i = ai.customerObjectToStack.myClothFragment.Count - 1; i >= 0; i--)
                        {
                            ClothFragment clothFragment = ai.customerObjectToStack.myClothFragment[i];

                            if (clothToWash.Count >= maxClothToStack)
                                yield return new WaitUntil(() => clothToWash.Count < maxClothToStack);

                            clothFragment.transform.SetParent(stackStarter.transform);

                            Vector3 targetPosition = (clothToWash.Count == 0)
                                ? stackStarter.transform.position
                                : clothToWash.Peek().nextPosition.transform.position;

                            clothFragment.transform.DOJump(targetPosition, 1f, 1, stackTime).
                                OnComplete(() =>
                            {
                                clothToWash.Push(clothFragment);
                                ai.customerObjectToStack.myClothFragment.Remove(clothFragment);
                            });
                            InBasketHaveElements?.Invoke();
                                        
                        }
                    yield return new WaitForSeconds(stackTime);
                    yield return new WaitUntil(() => ReferenceManager.Instance.queueSystem._canMove);
                    ai.SetIk(0);
                    ReferenceManager.Instance.queueSystem.DequeueOnButton();
                    aiQueue.Dequeue();
                }

                yield return new WaitForSeconds(stackTime);
                int k = Random.Range(0,
                    (ReferenceManager.Instance.GameData.unlockedMachine == 0
                        ? 0
                        : ReferenceManager.Instance.GameData.unlockedMachine));
                ReferenceManager.Instance.dirtyBoxAiManager.AddTask(pickupPoint,
                    dropPoint[k], k);
            }

            aiCoroutine = null;
        }

        private int indexShuffling = 0;

        public void checkifLOopStop()
        {
            // //
            // // int k = Random.Range(0,
            // //     (ReferenceManager.Instance.GameData.unlockedMachine == 0
            // //         ? 0
            // //         : ReferenceManager.Instance.GameData.unlockedMachine - 1));
            // int k = 0;
            // if (ReferenceManager.Instance.GameData.unlockedMachine == 1)
            // {
            //     k = 0;
            // }
            // else if (ReferenceManager.Instance.GameData.unlockedMachine == 2)
            // {
            //     if (indexShuffling == 0)
            //     {
            //         k = indexShuffling = 1;
            //     }
            //     else
            //     {
            //         k = indexShuffling = 0;
            //     }
            // }
            // else if (ReferenceManager.Instance.GameData.unlockedMachine == 3)
            // {
            //     if (indexShuffling == 0)
            //     {
            //         k = indexShuffling = 1;
            //     }
            //     else if (indexShuffling == 1)
            //     {
            //         k = indexShuffling = 2;
            //     }
            //     else if (indexShuffling == 2)
            //     {
            //         k = indexShuffling = 0;
            //     }
            // }
            int k = 0;
            int unlockedMachine = ReferenceManager.Instance.GameData.unlockedMachine;

            if (unlockedMachine == 1)
            {
                k = 0;
            }
            else if (unlockedMachine > 1)
            {
                k = indexShuffling = (indexShuffling + 1) % unlockedMachine;
            }

            ReferenceManager.Instance.dirtyBoxAiManager.AddTask(pickupPoint,
                dropPoint[k], k);
        }

        private void OnDisable()
        {
            ReferenceManager.OnPLayerGotUpgrade -= OnUpgrade;
        }

        public GameObject pickupPoint;
        public WashingMachineDropper[] dropPoint;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ToastPlugin;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class PressBasketHandler : MonoBehaviour
    {
        #region TriggerEvent

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent(out PlayerStackManager playerStackManager))
                {
                    // **Store and return remaining dirty clothes**
                    Stack<ClothFragment> tempReversedStack = new Stack<ClothFragment>();
                    while (TempCLothes.Count > 0)
                    {
                        tempReversedStack.Push(TempCLothes.Pop());
                    }

                    while (tempReversedStack.Count > 0)
                    {
                        var cloth = tempReversedStack.Pop();
                        playerStackManager.ClothStack.Push(cloth);
                    }

                    isPlayerInside = false;

                    if (_playerCoroutine != null)
                    {
                        StopCoroutine(_playerCoroutine);
                        _playerCoroutine = null;
                    }

                    if (_aiPlayerCoroutine != null)
                    {
                        StopCoroutine(_aiPlayerCoroutine);
                        _aiPlayerCoroutine = null;
                    }
                }

                if (other.TryGetComponent(out AiStackManager aiStackManager))
                {
                    Stack<ClothFragment> tempReversedStack = new Stack<ClothFragment>();
                    while (TempCLothes.Count > 0)
                    {
                        tempReversedStack.Push(TempCLothes.Pop());
                    }

                    while (tempReversedStack.Count > 0)
                    {
                        var cloth = tempReversedStack.Pop();
                        playerStackManager.ClothStack.Push(cloth);
                    }

                    isPlayerInside = false;

                    if (_aiPlayerCoroutine != null)
                    {
                        StopCoroutine(_aiPlayerCoroutine);
                        _aiPlayerCoroutine = null;
                    }
                }
            }
        }


        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent(out AiStackManager aiPlayerStackManager))
                {
                    isPlayerInside = true;
                    if (_aiPlayerCoroutine == null &&
                        aiPlayerStackManager.TryGetComponent(out AICleanClothes cleanClothes) &&
                        cleanClothes.currentState == AICleanClothes.AIState.Dropping)
                        _aiPlayerCoroutine = StartCoroutine(HandlePlayerInteraction(aiPlayerStackManager));
                }
            }
        }

        public GameObject playerPoint;
        public GameObject lookatPoint;


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.TryGetComponent(out PlayerStackManager playerStackManager))
                {
                    isPlayerInside = true;

                    _playerCoroutine ??= StartCoroutine(HandlePlayerInteraction(playerStackManager));
                }
            }
        }

        #endregion

        #region PlayerToBasket

        public bool isPlayerInside = false;
        private Coroutine _playerCoroutine;
        private Coroutine _aiPlayerCoroutine;
        private Coroutine _clothMovementCoroutine;
        private Stack<ClothFragment> clothToWash = new Stack<ClothFragment>();
        public float stackTime = 1f;
        public GameObject stackMachineStarter;
        public GameObject pressClothStackPoint;


        private void RearrangeStack(PlayerStackManager playerStackManager)
        {
            Vector3 startPosition = playerStackManager.stackStarter.transform.position;
            List<ClothFragment> tempList = playerStackManager.ClothStack.ToList();

            tempList.Reverse();
            for (int i = 0; i < tempList.Count; i++)
            {
                Vector3 targetPosition = (i == 0) ? startPosition : tempList[i - 1].nextPosition.transform.position;
                tempList[i].transform.SetPositionAndRotation(targetPosition, quaternion.identity);
            }
        }

        public Stack<ClothFragment> TempCLothes = new Stack<ClothFragment>();
        Stack<ClothFragment> dirtyClothes = new Stack<ClothFragment>();


        private IEnumerator HandlePlayerInteraction(PlayerStackManager playerStackManager)
        {
            Stack<ClothFragment> tempStack = new Stack<ClothFragment>();

            while (playerStackManager.ClothStack.Count > 0)
            {
                tempStack.Push(playerStackManager.ClothStack.Pop());
            }

            while (tempStack.Count > 0)
            {
                var cloth = tempStack.Pop();
                if (cloth.state == ClothState.Washing)
                {
                    TempCLothes.Push(cloth);
                }
                else
                {
                    dirtyClothes.Push(cloth);
                }
            }

            while (dirtyClothes.Count > 0)
            {
                playerStackManager.ClothStack.Push(dirtyClothes.Pop());
            }

            TempCLothes = new Stack<ClothFragment>(TempCLothes.Reverse());
            while (isPlayerInside && TempCLothes.Count > 0)
            {
                var cloth = TempCLothes.Pop();
                cloth.transform.SetParent(stackMachineStarter.transform);

                Vector3 targetPosition = (clothToWash.Count == 0)
                    ? stackMachineStarter.transform.position
                    : clothToWash.Peek().nextPosition.transform.position;

                cloth.transform.DOJump(targetPosition, 1f, 1, stackTime).OnComplete(() =>
                {
                    clothToWash.Push(cloth);
                    if (SoundManager.instance)
                    {
                        SoundManager.instance.Play(SoundName.Drop);
                    }

                    if (_clothMovementCoroutine == null)
                    {
                        _clothMovementCoroutine = StartCoroutine(MoveClothesToPress());
                    }
                });
                RearrangeStack(playerStackManager);

                yield return new WaitForSeconds(stackTime);
            }


            playerStackManager.SetIk(playerStackManager.ClothStack.Count <= 0 ? 0 : 1);
        }

        #endregion

        #region FromBasketToPress

        public Transform[] exitWaypoints; // Waypoints path to exit
        public float moveSpeed = 3f;
        public float rotateSpeed = 200f;
        public Queue<ClothFragment> washedClothes = new Queue<ClothFragment>();
        private Coroutine _tubeCoroutine;


        public void MoveClothesToPress(ClothFragment cloth)
        {
            washedClothes.Enqueue(cloth);

            if (_tubeCoroutine == null)
            {
//                print("tube me araha hn");
                _tubeCoroutine = StartCoroutine(PressClothesAndRelease());
            }
        }

        private IEnumerator MoveClothesToPress()
        {
            while (clothToWash.Count > 0 || isPlayerInside)
            {
//                print("this is working "+ name);
                yield return new WaitUntil(() => !isRinEmpty);

//                print("this is working "+ name);

                if (clothToWash.Count > 0)
                {
//                    print("this is working "+ name);

                    var cloth = clothToWash.Pop();
                    Vector3 targetPosition = washedClothes.Count <= 0
                        ? pressClothStackPoint.transform.position
                        : washedClothes.Peek().nextPosition.transform.position;

                    // Move cloth to machine
                    cloth.transform.DOJump(targetPosition, 0.12f, 1, .5f)
                        .OnComplete(() => { MoveClothesToPress(cloth); });
//                    print("this is working "+ name);

                    yield return new WaitForSeconds(2);
                }
                else
                {
                    yield return null;
                }
            }

            _clothMovementCoroutine = null;
        }

        public GameObject ironObject;
        public GameObject ironMovePoint;
        public GameObject ironStartPositionPoint;
        public GameObject ironClothesPoint;
        [SerializeField] bool isIronMoving = false;
        public ParticleSystem steam;
        public float rinCost = 5;
        public float totalRin = 200;
        public bool isRinEmpty;

        public IEnumerator IronClothes(ClothFragment cloth)
        {
            cloth.transform.DOMove(ironClothesPoint.transform.position, .1f).OnComplete(() =>
            {
                // steam.gameObject.SetActive(false);
                steam.gameObject.SetActive(true);
                steam.Play();
                ironObject.transform.DOMove(ironMovePoint.transform.position, .5f).OnComplete(() =>
                {
                    cloth.transform.DOMove(exitWaypoints.ElementAt(0).transform.position, .5f)
                        .OnComplete(
                            () =>
                            {
                                StartCoroutine(MoveThroughWaypoints(cloth));
                                ironObject.transform.DOLookAt(ironClothesPoint.transform.position, .01f);
                                isIronMoving = true;
                            });
                });
            });
            // });

            yield return new WaitForSeconds(1);
            yield return new WaitUntil(() => isIronMoving);
        }

        public IEnumerator IronClothesReverse(ClothFragment cloth)
        {
            isRinEmpty = totalRin < rinCost;

            if (totalRin >= rinCost)
            {
                totalRin -= rinCost;
                rinseMachineCanvas.rinseTrackingImage.fillAmount = (float)totalRin / refillAmount;
            }
            else
            {
                rinseMachineCanvas.CanvasStateChanger(MachineCanvasStates.RefillNeeded);
                ReferenceManager.Instance.notificationHandler.ShowNotification("PressMachine need Rinse Refill");
            }

            cloth.transform.DOMove(ironClothesPoint.transform.position, .1f).OnComplete(() =>
            {
                steam.gameObject.SetActive(true);
                steam.Play();
                ironObject.transform.DOMove(ironStartPositionPoint.transform.position, .5f).OnComplete(() =>
                {
                    cloth.transform.DOMove(exitWaypoints.ElementAt(0).transform.position, .5f).OnComplete(() =>
                    {
                        StartCoroutine(MoveThroughWaypoints(cloth));
                        ironObject.transform.DOLookAt(ironClothesPoint.transform.position, .01f);
                        isIronMoving = true;
                    });
                });
            });

            yield return new WaitForSeconds(1);
            yield return new WaitUntil(() => isIronMoving);
        }

        public int myIndex;

        private void Awake()
        {
            InitDetergent();
        }

        private int fullAmount;

        public void InitDetergent()
        {
            switch (myIndex)
            {
                case 0:
                    refillAmount = totalRin = ReferenceManager.Instance.GameData.gameEconomy.rin1Capacity;
                    break;
                case 1:
                    refillAmount = totalRin = ReferenceManager.Instance.GameData.gameEconomy.rin2Capacity;
                    break;
                case 2:
                    refillAmount = totalRin = ReferenceManager.Instance.GameData.gameEconomy.rin3Capacity;
                    break;
            }
        }

        public RinseMachineCanvas rinseMachineCanvas;
        public float refillAmount;

        public void RefillRin(int amount)
        {
            if (totalRin < refillAmount)
            {
                rinseMachineCanvas.CanvasStateChanger(MachineCanvasStates.Full);
                refillAmount = totalRin = amount;
                isRinEmpty = false;
                rinseMachineCanvas.rinseTrackingImage.fillAmount = 1f;
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("No need to refill Rin");
            }
        }

        public int index;

        private IEnumerator PressClothesAndRelease()
        {
            isIronMoving = false;

            while (washedClothes.Count > 0)
            {
                ClothFragment cloth = washedClothes.Dequeue();
                if (index == 0)
                {
                    StartCoroutine(IronClothes(cloth));
                    index = 1;
                }
                else if (index == 1)
                {
                    StartCoroutine(IronClothesReverse(cloth));
                    index = 0;
                }

                yield return new WaitUntil(() => isIronMoving);
                cloth.transform.SetParent(null);
            }

            _tubeCoroutine = null; // Reset when all clothes are processed
        }

        private IEnumerator MoveThroughWaypoints(ClothFragment cloth)
        {
            cloth.SetState(ClothState.Ready);

            foreach (Transform waypoint in exitWaypoints)
            {
                while (Vector3.Distance(cloth.transform.position, waypoint.position) > 0.1f)
                {
                    cloth.transform.position = Vector3.MoveTowards(cloth.transform.position, waypoint.position,
                        moveSpeed * Time.deltaTime);

                    Vector3 direction = (waypoint.position - cloth.transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    float springAmount = Mathf.Sin(Time.time * 10f) * 10f;
                    Quaternion springRotation = Quaternion.Euler(springAmount, 0, springAmount);

                    cloth.transform.rotation = Quaternion.RotateTowards(cloth.transform.rotation,
                        lookRotation * springRotation,
                        rotateSpeed * Time.deltaTime);

                    yield return null;
                }
            }

            cloth.transform.SetParent(checkoutHandler.counterStackPosition.transform);
            yield return new WaitForSeconds(0.5f);

            Vector3 targetPosition;
            if (checkoutHandler.ReadyToShipClothes.Count == 0)
            {
                targetPosition = checkoutHandler.counterStackPosition.transform.position;
            }
            else
            {
                targetPosition = checkoutHandler.ReadyToShipClothes.Peek().nextPosition.transform.position;
            }

            bool isMoved = false;

            cloth.transform.DOMove(targetPosition, 0.1f)
                .OnComplete(() =>
                {
                    cloth.SetState(ClothState.Washing);
                    cloth.positionInherit = (checkoutHandler.ReadyToShipClothes.Count == 0)
                        ? checkoutHandler.counterStackPosition
                        : checkoutHandler.ReadyToShipClothes.Peek().nextPosition;

                    checkoutHandler.ReadyToShipClothes.Push(cloth);
                    cloth.transform.rotation = Quaternion.identity;

                    isMoved = true;
                });

            // Wait for DOMove to complete
            yield return new WaitUntil(() => isMoved);
        }

        public Vector3 offset;
        public CheckoutHandler checkoutHandler;

        #endregion
    }
}
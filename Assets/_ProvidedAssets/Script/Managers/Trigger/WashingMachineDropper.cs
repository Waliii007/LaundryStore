using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Invector.vCharacterController;
using ToastPlugin;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class WashingMachineDropper : Dropper
    {
        #region myAnimator

        private void OnEnable()
        {
            limitOnBasket = ReferenceManager.Instance.GameData.gameEconomy.limitBasket;
            limitOnCleanBasket = ReferenceManager.Instance.GameData.gameEconomy.limitOnCleanBasket;
        }

        #endregion


        private static readonly int MachineWashed = Animator.StringToHash("WashingMachine");
        private static readonly int DrayerAnimation = Animator.StringToHash("DrayerAnimation");

        #region PlayerToBasket

        public bool isPlayerInside = false;
        private Coroutine _playerCoroutine;
        private Coroutine aICoroutine;
        private Coroutine _clothMovementCoroutine;
        public Stack<ClothFragment> clothToWash = new Stack<ClothFragment>();
        public float stackTime = 1f;
        public GameObject stackMachineStarter;
        public Transform machineStackPoint; // Machine stacking position

        public Animator washingMachine;
        public int limitOnBasket;
        public int limitOnCleanBasket;

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

        private void RearrangeStack(Stack<ClothFragment> _playerStack, GameObject _stackStarter)
        {
            Vector3 startPosition = _stackStarter.transform.position;
            List<ClothFragment> tempList = _playerStack.ToList();
            tempList.Reverse();
            for (int i = 0; i < tempList.Count; i++)
            {
                Vector3 targetPosition = (i == 0) ? startPosition : tempList[i - 1].nextPosition.transform.position;
                tempList[i].transform.SetPositionAndRotation(targetPosition, quaternion.identity);
            }
        }

        public Stack<ClothFragment> TempCLothes = new Stack<ClothFragment>();
        Stack<ClothFragment> cleanClothes = new Stack<ClothFragment>();


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
                if (cloth.state == ClothState.Dirty)
                {
                    TempCLothes.Push(cloth);
                }
                else
                {
                    cleanClothes.Push(cloth);
                }
            }


            while (cleanClothes.Count > 0)
            {
                playerStackManager.ClothStack.Push(cleanClothes.Pop());
            }

            TempCLothes = new Stack<ClothFragment>(TempCLothes.Reverse());
            while (isPlayerInside && TempCLothes.Count > 0)
            {
                yield return new WaitUntil(() => clothToWash.Count < limitOnBasket);
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
                        SoundManager.instance.Play(SoundName.Pick);
                    }

                    if (_clothMovementCoroutine == null)
                    {
                        _clothMovementCoroutine = StartCoroutine(MoveClothesToMachine());
                    }
                });
                RearrangeStack(playerStackManager);

                yield return new WaitForSeconds(stackTime);
            }

            playerStackManager.SetIk(playerStackManager.ClothStack.Count <= 0 ? 0 : 1);
        }

        private IEnumerator HandlePlayerInteraction(AiStackManager playerStackManager)
        {
            Stack<ClothFragment> tempStack = new Stack<ClothFragment>();
            while (playerStackManager.ClothStack.Count > 0)
            {
                tempStack.Push(playerStackManager.ClothStack.Pop());
            }

            while (tempStack.Count > 0)
            {
                var cloth = tempStack.Pop();
                if (cloth.state == ClothState.Dirty)
                {
                    TempCLothes.Push(cloth);
                }
                else
                {
                    cleanClothes.Push(cloth);
                }
            }


            while (cleanClothes.Count > 0)
            {
                playerStackManager.ClothStack.Push(cleanClothes.Pop());
            }

            TempCLothes = new Stack<ClothFragment>(TempCLothes.Reverse());
            while (isPlayerInside && TempCLothes.Count > 0 && clothToWash.Count < limitOnBasket)
            {
                var cloth = TempCLothes.Pop();
                cloth.transform.SetParent(stackMachineStarter.transform);

                Vector3 targetPosition = (clothToWash.Count == 0)
                    ? stackMachineStarter.transform.position
                    : clothToWash.Peek().nextPosition.transform.position;

                cloth.transform.DOJump(targetPosition, 1f, 1, stackTime).OnComplete(() =>
                {
                    clothToWash.Push(cloth);


                    if (_clothMovementCoroutine == null)
                    {
                        _clothMovementCoroutine = StartCoroutine(MoveClothesToMachine());
                    }
                });
                RearrangeStack(playerStackManager);

                yield return new WaitForSeconds(stackTime);
            }

            print((playerStackManager.ClothStack.Count <=
                   0) + ""
                      + (playerStackManager.ClothStack.Count) +
                      ":" + (TempCLothes.Count <= 0));

            playerStackManager.SetIk(playerStackManager.ClothStack.Count <= 0 ? 0 : 1);
        }

        private Stack<ClothFragment> reversedStack;

        #endregion

        #region BasketToMachine

        public Image detargentTrackingImage;
        public bool isDetergentEmpty;
        public int totalDetergent;
        public int detergentCost;
        public WaitForSeconds waitForSecondsForClothes = new WaitForSeconds(1f);


        public void Refill()
        {
            if (totalDetergent < 100)
            {
                totalDetergent = 100;
                isDetergentEmpty = false;
                detargentTrackingImage.fillAmount = 1f;
            }
            else
            {
                ToastHelper.ShowToast("Do not need to refill This Machine");
            }
        }

        private IEnumerator MoveClothesToMachine()
        {
            yield return new WaitUntil(() =>
                pressingClothPickingHandler.clothToPress.Count < limitOnCleanBasket);
            while (clothToWash.Count > 0 || isPlayerInside)
            {
                isDetergentEmpty = totalDetergent < detergentCost;
                yield return new WaitUntil(() => !isDetergentEmpty);

                if (clothToWash.Count > 0 && totalDetergent >= 0)
                {
                    totalDetergent -= detergentCost;
                    detargentTrackingImage.fillAmount = (float)totalDetergent / 100f;

                    var cloth = clothToWash.Pop();
                    Vector3 targetPosition = machineStackPoint.position;

                    cloth.transform.DOJump(targetPosition, 1f, 1, 2).OnComplete(() =>
                    {
                        AddWashedCloth(cloth);
                        washingMachine.SetTrigger(MachineWashed);
                    });
                    yield return waitForSecondsForClothes;
                }
                else
                {
                    yield return null;
                }
            }

            _clothMovementCoroutine = null;
        }

        #endregion

        #region FromTheTube

        public Transform[] exitWaypoints; // Waypoints path to exit
        public float moveSpeed = 3f;
        public float rotateSpeed = 200f;
        public Stack<ClothFragment> washedClothes = new Stack<ClothFragment>();
        private Coroutine _tubeCoroutine;

        public void AddWashedCloth(ClothFragment cloth)
        {
            washedClothes.Push(cloth);
            // _tubeCoroutine =

            StartCoroutine(ReleaseClothesFromTube());
        }


        private IEnumerator ReleaseClothesFromTube()
        {
            while (washedClothes.Count > 0)
            {
                var cloth = washedClothes.Pop();
                cloth.transform.SetParent(null); // Free cloth from machine
                if (SoundManager.instance)
                {
                    SoundManager.instance.Play(SoundName.Washing);
                }

                yield return StartCoroutine(MoveThroughWaypoints(cloth));
            }


            // _tubeCoroutine = null; // Reset when all clothes are processed
        }

        private IEnumerator MoveThroughWaypoints(ClothFragment cloth)
        {
            cloth.SetState(ClothState.Washing);
            foreach (Transform waypoint in exitWaypoints)
            {
                while (Vector3.Distance(cloth.transform.position, waypoint.position) > 0.1f)
                {
                    // Move towards waypoint
                    cloth.transform.position = Vector3.MoveTowards(cloth.transform.position, waypoint.position,
                        moveSpeed * Time.deltaTime);

                    // Rotate towards waypoint with spring effect
                    Vector3 direction = (waypoint.position - cloth.transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(direction);

                    // Spring-like oscillation using sine wave
                    float springAmount = Mathf.Sin(Time.time * 10f) * 10f; // 10 degrees oscillation
                    Quaternion springRotation = Quaternion.Euler(springAmount, 0, springAmount);

                    // // Combine rotations
                    // cloth.transform.rotation = Quaternion.RotateTowards(cloth.transform.rotation,
                    //     lookRotation * springRotation,
                    //     rotateSpeed * Time.deltaTime);

                    yield return null;
                }
            }

            yield return StartCoroutine(MoveThenJumpAndStack(cloth));

            if (!isOccupied)
                ReferenceManager.Instance.cleanBoxAIManager.AddTask(pressingClothPickingHandler.aiPickPoint, this);
        }

        public bool isOccupied = false;
        public bool isOccupiedbyDirty = false;

        #endregion

        #region Drayer

        public GameObject drayerPoint;
        public GameObject stackPoint;
        public Animator drayerAnimator;

        private IEnumerator MoveThenJumpAndStack(ClothFragment cloth)
        {
            drayerAnimator.SetTrigger(DrayerAnimation);
            Vector3 movePoint = new Vector3(drayerPoint.transform.position.x, drayerPoint.transform.position.y,
                drayerPoint.transform.position.z);
            Vector3 jumpPoint;

            if (pressingClothPickingHandler.clothToPress.Count == 0)
            {
                cloth.positionInherit = pressingClothPickingHandler.stackStarter;
                jumpPoint = new Vector3(pressingClothPickingHandler.stackStarter.transform.position.x,
                    pressingClothPickingHandler.stackStarter.transform.position.y,
                    pressingClothPickingHandler.stackStarter.transform.position.z);
            }
            else
            {
                cloth.positionInherit = pressingClothPickingHandler.Peek().nextPosition;

                jumpPoint = new Vector3(
                    pressingClothPickingHandler.clothToPress.Peek().nextPosition.transform.position.x,
                    pressingClothPickingHandler.clothToPress.Peek().nextPosition.transform.position.y + .01f,
                    pressingClothPickingHandler.clothToPress.Peek().nextPosition.transform.position.z);
            }

            bool moveDone = false;
            cloth.transform.rotation = Quaternion.identity;
            cloth.transform.DOMove(movePoint, 1f).OnComplete(() => moveDone = true);
            yield return new WaitUntil(() => moveDone);


            bool jumpDone = false;
            cloth.transform.SetParent(pressingClothPickingHandler.stackStarter.transform);

            cloth.transform.DOJump(jumpPoint, 1.5f, 1, 0.8f).OnComplete(() =>
            {
                pressingClothPickingHandler.Push(cloth);
                jumpDone = true;
            });
            //RearrangeStack(pressingClothPickingHandler,);

            yield return new WaitUntil(() => jumpDone);
        }

        public float offset = 1f;
        public PressingClothPickingHandler pressingClothPickingHandler;

        private void ArrangeStack()
        {
            int index = 0;
            foreach (ClothFragment cloth in pressingClothPickingHandler.clothToPress)
            {
                Vector3 arrangedPosition =
                    index == 0
                        ? pressingClothPickingHandler.stackStarter.transform.position
                        : cloth.nextPosition.transform.position;

                cloth.transform.transform.SetPositionAndRotation(arrangedPosition,
                    Quaternion.identity);
                index++;
            }
        }

        #endregion

        #region TriggerEvent

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInside = false;
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

                    RearrangeStack(playerStackManager);
                    if (_playerCoroutine != null)
                    {
                        StopCoroutine(_playerCoroutine);
                        _playerCoroutine = null;
                    }

                    playerStackManager.SetIk(playerStackManager.ClothStack.Count == 0 ? 0 : 1);
                }
                else if (other.TryGetComponent(out AiStackManager aIStackManager))
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
                        aIStackManager.ClothStack.Push(cloth);
                    }

                    RearrangeStack(aIStackManager);
                    if (aICoroutine != null)
                    {
                        // StopCoroutine(aICoroutine);
                        aICoroutine = null;
                    }

                    aIStackManager.SetIk(aIStackManager.ClothStack.Count == 0 ? 0 : 1);
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //     print("calling");
                if (other.TryGetComponent(out PlayerStackManager playerStackManager))
                {
                    isPlayerInside = true;
                    //          print("calling");

                    if (_playerCoroutine == null)
                    {
//                        print("calling");
                        if (playerStackManager)
                            _playerCoroutine = StartCoroutine(HandlePlayerInteraction(playerStackManager));
                    }
                }
                else if (other.TryGetComponent(out AiStackManager aiStackManager))
                {
                    isPlayerInside = true;
                    print("calling");

                    if (aICoroutine == null)
                    {
                        if (GlobalConstant.isLogger)
                            print("calling");
                        aICoroutine = StartCoroutine(HandlePlayerInteraction(aiStackManager));
                    }
                }
            }
        }

        #endregion
    }
}
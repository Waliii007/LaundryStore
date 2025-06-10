using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
            //   limitOnBasket = ReferenceManager.Instance.GameData.gameEconomy.limitBasket;


            if (ReferenceManager.Instance.GameData.isTutorialCompleted)
                canvasCheckCoroutine = StartCoroutine(CheckCanvasStateRoutine());
            InitDetergent();
            upgradeindex = myIndex switch
            {
                0 => ReferenceManager.Instance.GameData.gameEconomy.machineUpgradeIndex,
                1 => ReferenceManager.Instance.GameData.gameEconomy.machine1UpgradeIndex,
                2 => ReferenceManager.Instance.GameData.gameEconomy.machine2UpgradeIndex,
                _ => upgradeindex
            };

            limitOnCleanBasket = maxClothesPerCycle = upgradeindex switch
            {
                0 => 15,
                1 => 25,
                2 => 40,
                _ => maxClothesPerCycle
            };
        }

        private void Start()
        {
            StartCoroutine(CheckMachineSpaceRoutine());
        }

        #endregion

        private IEnumerator CheckMachineSpaceRoutine()
        {
            var wait = new WaitForSeconds(0.1f); // Check every 0.1 seconds

            while (true)
            {
                availableSpaceInsideMachine =
                    maxClothesPerCycle - stackMachineStarter.transform.childCount - 1;
                fullIndicator.SetActive(availableSpaceInsideMachine <= 0);

                yield return wait;
            }
            yield return wait;

        }

        

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
        public int availableSpaceInsideMachine = 0;
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

        private void Awake()
        {
            totalDetergent = ReferenceManager.Instance.GameData.gameEconomy.detergentMax;
        }

        public bool cleanBasket = false;
        public int maxClothesPerCycle = 10;
        public int currentClothesCount = 0;
        [SerializeField] private GameObject fullIndicator;

        public Stack<ClothFragment> tempStack = new Stack<ClothFragment>();
        int upgradeindex = 0;
        public int available = 0;

        private IEnumerator HandlePlayerInteraction(PlayerStackManager playerStackManager)
        {
            try
            {
                currentClothesCount = 0;
                int availableSpace =
                    maxClothesPerCycle - stackMachineStarter.transform.childCount - 1;
                availableSpaceInsideMachine = availableSpace;
                //    fullIndicator.SetActive(availableSpace <= 0);
                if (availableSpace <= 0)
                {
                    yield break;
                }

                available =
                    currentClothesCount += availableSpace;


                while (playerStackManager.ClothStack.Count > 0)
                {
                    tempStack.Push(playerStackManager.ClothStack.Pop());
                }

                while (tempStack.Count > 0)
                {
                    var cloth = tempStack.Pop();

                    if (cloth.state == ClothState.Dirty && availableSpace > 0)
                    {
                        TempCLothes.Push(cloth);
                        availableSpace--;
                        availableSpaceInsideMachine = availableSpace;
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
                    var cloth = TempCLothes.Pop();
                    cloth.transform.SetParent(stackMachineStarter.transform);

                    Vector3 targetPosition = (clothToWash.Count == 0)
                        ? stackMachineStarter.transform.position
                        : clothToWash.Peek().nextPosition.transform.position;

                    cloth.transform.DOJump(targetPosition, 1f, 1, stackTime).OnComplete(() =>
                    {
                        clothToWash.Push(cloth);

                        if (SoundManager.instance)
                            SoundManager.instance.Play(SoundName.Pick);

                        if (_clothMovementCoroutine == null)
                            _clothMovementCoroutine = StartCoroutine(MoveClothesToMachine());
                    });

                    RearrangeStack(playerStackManager);
                    yield return new WaitForSeconds(stackTime);
                }

                playerStackManager.SetIk(playerStackManager.ClothStack.Count <= 0 ? 0 : 1);
            }
            finally
            {
                //  fullIndicator.SetActive(currentClothesCount >= maxClothesPerCycle);

                _playerCoroutine = null;
            }
        }

        private IEnumerator HandlePlayerInteraction(AiStackManager playerStackManager)
        {
            currentClothesCount = 0;
            Stack<ClothFragment> tempStack = new Stack<ClothFragment>();

            int availableSpace = Mathf.Min(playerStackManager.ClothStack.Count,
                maxClothesPerCycle - stackMachineStarter.transform.childCount);
            availableSpaceInsideMachine = availableSpace;

            if (availableSpace <= 0)
            {
                aICoroutine = null;
                yield break;
            }

            available = availableSpace;
            //    fullIndicator.SetActive(stackMachineStarter.transform.childCount >= (maxClothesPerCycle-1));

            currentClothesCount += availableSpace;
            //    currentClothesCount = stackMachineStarter.transform.childCount;

            while (playerStackManager.ClothStack.Count > 0)
            {
                tempStack.Push(playerStackManager.ClothStack.Pop());
            }

            while (tempStack.Count > 0)
            {
                var cloth = tempStack.Pop();

                if (cloth.state == ClothState.Dirty && availableSpace > 0)
                {
                    TempCLothes.Push(cloth);
                    availableSpace--;
                    availableSpaceInsideMachine = availableSpace;

                    // Position the fullIndicator after pushing cloth 
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
                var cloth = TempCLothes.Pop();
                cloth.transform.SetParent(stackMachineStarter.transform);

                Vector3 targetPosition = (clothToWash.Count == 0)
                    ? stackMachineStarter.transform.position
                    : clothToWash.Peek().nextPosition.transform.position;


                cloth.transform.DOJump(targetPosition, 1f, 1, stackTime).OnComplete(() =>
                {
                    clothToWash.Push(cloth);

                    if (_clothMovementCoroutine == null)
                        _clothMovementCoroutine = StartCoroutine(MoveClothesToMachine());
                });

                RearrangeStack(playerStackManager);
                yield return new WaitForSeconds(stackTime);
            }

            playerStackManager.SetIk(playerStackManager.ClothStack.Count <= 0 ? 0 : 1);
            aICoroutine = null;
        }

        private Stack<ClothFragment> reversedStack;

        #endregion

        #region BasketToMachine

        public MachineCanvasManager machineCanvasManager;
        public bool isDetergentEmpty;
        public int totalDetergent;
        public int detergentCost;
        public WaitForSeconds waitForSecondsForClothes = new WaitForSeconds(1f);
        public int myIndex;

        public void InitDetergent()
        {
            switch (myIndex)
            {
                case 0:
                    totalDetergent = refillAmount = 250;
                    break;
                case 1:
                    totalDetergent = refillAmount = 250;
                    break;
                case 2:
                    totalDetergent = refillAmount = 250;
                    break;
            }
        }

        public int refillAmount;

        public void EmptyForTutorial()
        {
            totalDetergent = 0;
            isDetergentEmpty = true;
            machineCanvasManager.detergentTrackingImage.fillAmount = 0;
            machineCanvasManager.CanvasStateChanger(
                MachineCanvasStates.RefillNeeded);
            ReferenceManager.Instance.notificationHandler.ShowNotification(
                $"Machine {myIndex} detergent needs to refill This Machine");
        }

        public void Refill(int amount)
        {
            if (totalDetergent <= refillAmount)
            {
                totalDetergent = amount;
                refillAmount = amount;
                isDetergentEmpty = false;
                machineCanvasManager.detergentTrackingImage.fillAmount = 1f;
            }
            else
            {
                ReferenceManager.Instance.notificationHandler.ShowNotification("Do not need to refill This Machine");
            }
        }

        private Coroutine canvasCheckCoroutine;


        private void OnDisable()
        {
            // Stop coroutine when disabled
            if (canvasCheckCoroutine != null
               )
                StopCoroutine(canvasCheckCoroutine);
        }

        private float fill;

        public IEnumerator CheckCanvasStateRoutine()
        {
            while (true)
            {
                fill = machineCanvasManager.detergentTrackingImage.fillAmount;

                machineCanvasManager.CanvasStateChanger(
                    fill == 0f ? MachineCanvasStates.RefillNeeded : MachineCanvasStates.Full);

                yield return waitState; // Adjust interval as needed
            }
        }

        [SerializeField] private GameObject cleanBasketFullIndicator;
        public WaitForSeconds waitState = new WaitForSeconds(.1f);


        private IEnumerator MoveClothesToMachine()
        {
            yield return new WaitUntil(() =>
            {
                bool isFull = pressingClothPickingHandler.clothToPress.Count >= limitOnCleanBasket;

                if (cleanBasketFullIndicator)
                    cleanBasketFullIndicator.SetActive(isFull);

                if (isFull)
                {
                    ReferenceManager.Instance.cleanBoxAIManager.AddTask(pressingClothPickingHandler.aiPickPoint, this);
                    ReferenceManager.Instance.cleanBoxAIManager.AssignTask();
                }

                return !isFull;
            });

            if (cleanBasketFullIndicator)
                cleanBasketFullIndicator.SetActive(false);


            yield return new WaitUntil(() => !isDetergentEmpty);

            while (clothToWash.Count > 0 || isPlayerInside)
            {
                isDetergentEmpty = totalDetergent < detergentCost;

                if (pressingClothPickingHandler.clothToPress.Count >= limitOnCleanBasket)
                {
                    if (cleanBasketFullIndicator)
                        cleanBasketFullIndicator.SetActive(true);

                    ReferenceManager.Instance.cleanBoxAIManager.AddTask(pressingClothPickingHandler.aiPickPoint, this);
                    ReferenceManager.Instance.cleanBoxAIManager.AssignTask();

                    yield return new WaitUntil(() =>
                    {
                        bool hasSpace = pressingClothPickingHandler.clothToPress.Count < limitOnCleanBasket;

                        if (cleanBasketFullIndicator)
                            cleanBasketFullIndicator.SetActive(!hasSpace);

                        return hasSpace;
                    });
                }

                if (clothToWash.Count > 0 && totalDetergent >= detergentCost)
                {
                    totalDetergent -= detergentCost;
                    machineCanvasManager.detergentTrackingImage.fillAmount = (float)totalDetergent / refillAmount;
                    if (currentClothesCount > 0)
                        currentClothesCount--;
                    var cloth = clothToWash.Pop();
                    Vector3 targetPosition = machineStackPoint.position;

                    cloth.transform.DOJump(targetPosition, 1f, 1, 2).OnComplete(() =>
                    {
                        fullIndicator.SetActive(false);

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
                    //       Vector3 direction = (waypoint.position - cloth.transform.position).normalized;
                    //    Quaternion lookRotation = Quaternion.LookRotation(direction);

                    // Spring-like oscillation using sine wave
                    float springAmount = Mathf.Sin(Time.time * 10f) * 10f; // 10 degrees oscillation
                    //      Quaternion springRotation = Quaternion.Euler(springAmount, 0, springAmount);

                    // // Combine rotations
                    // cloth.transform.rotation = Quaternion.RotateTowards(cloth.transform.rotation,
                    //     lookRotation * springRotation,
                    //     rotateSpeed * Time.deltaTime);

                    yield return null;
                }
            }

            yield return StartCoroutine(MoveThenJumpAndStack(cloth));
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
            yield return new WaitUntil(() =>
            {
//                print((pressingClothPickingHandler.clothToPress.Count < limitOnCleanBasket)
                // );
                if (!(pressingClothPickingHandler.clothToPress.Count < limitOnCleanBasket))
                {
                    ReferenceManager.Instance.cleanBoxAIManager.AddTask(pressingClothPickingHandler.aiPickPoint, this);
//                    print(this.name + ": Adding to clothes " + pressingClothPickingHandler.name);
                }

                ;
                return pressingClothPickingHandler.clothToPress.Count < limitOnCleanBasket;
            });

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
                cleanBasketFullIndicator.transform.SetParent(stackPoint.transform);
                // Position the fullIndicator after pushing cloth
            }
            else
            {
                cloth.positionInherit = pressingClothPickingHandler.Peek().nextPosition;

                jumpPoint = new Vector3(
                    pressingClothPickingHandler.clothToPress.Peek().nextPosition.transform.position.x,
                    pressingClothPickingHandler.clothToPress.Peek().nextPosition.transform.position.y + .01f,
                    pressingClothPickingHandler.clothToPress.Peek().nextPosition.transform.position.z);
                // Position the fullIndicator after pushing cloth
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
                if (!isOccupied)
                    ReferenceManager.Instance.cleanBoxAIManager.AddTask(pressingClothPickingHandler.aiPickPoint, this);
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
                if (other.TryGetComponent(out PlayerStackManager playerStackManager) &&
                    other.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    // **Store and return remaining dirty clothes**
                    Stack<ClothFragment> tempReversedStack = new Stack<ClothFragment>();
                    while (TempCLothes.Count > 0)
                    {
                        tempReversedStack.Push(TempCLothes.Pop());
                        // adjust Y offset if needed
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
                else if (other.TryGetComponent(out AiStackManager aIStackManager) &&
                         other.gameObject.layer == LayerMask.NameToLayer("AILAYER"))
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
            if (other.CompareTag("Player") && other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (other.TryGetComponent(out PlayerStackManager playerStackManager))
                {
                    isPlayerInside = true;

                    // Retry coroutine if it's null and not at max capacity
                    if (_playerCoroutine == null &&
                        currentClothesCount < maxClothesPerCycle &&
                        playerStackManager.ClothStack.Count > 0)
                    {
                        _playerCoroutine = StartCoroutine(HandlePlayerInteraction(playerStackManager));
                    }
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("AILAYER"))
            {
                if (other.TryGetComponent(out AiStackManager aiStackManager))
                {
                    isPlayerInside = true;

                    // Restart coroutine if already ended and conditions are met
                    if ((aICoroutine == null || !isActiveAndEnabled) &&
                        currentClothesCount < maxClothesPerCycle &&
                        aiStackManager.ClothStack.Count > 0)
                    {
                        if (GlobalConstant.isLogger)
                            print("AI coroutine restarting");

                        aICoroutine = StartCoroutine(HandlePlayerInteraction(aiStackManager));
                    }
                }
            }
        }

        #endregion
    }
}
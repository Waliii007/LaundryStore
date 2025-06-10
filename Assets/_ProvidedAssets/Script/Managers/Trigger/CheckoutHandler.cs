using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Invector.vCharacterController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace LaundaryMan
{
    public class CheckoutHandler : MonoBehaviour
    {
        public Stack<ClothFragment> ReadyToShipClothes = new Stack<ClothFragment>();
        public GameObject counterStackPosition;
        private Coroutine _playerCoroutine;
        bool _isBusy;
        [SerializeField] bool isPlayerInside = false;


        #region TriggerEvent

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInside = false;
                if (_isBusy && _playerCoroutine != null)
                {
                    StopCoroutine(_playerCoroutine);
                    _playerCoroutine = null;
                    _isBusy = false;
                }
            }

            if (other.CompareTag("AI"))
            {
                selectedAiReached = false;
            }
        }

        public WaitForSeconds waitForSecondsToCashier = new WaitForSeconds(0.5f);
        public bool isAICashierUnlocked;

        public IEnumerator AICashierUnlocked()
        {
            //      print("Calling coroutine"+_isBusy);
            if (!_isBusy)
            {
                _isBusy = true;
                _playerCoroutine = StartCoroutine(CheckOut());
            }

            yield return null;
        }

        private void Update()
        {
            if (ReferenceManager.Instance.queueSystem._washedClothQueueAi.Count > 0)
            {
                ReferenceManager.Instance.queueSystem._washedClothQueueAi.Peek().TryGetComponent(out CustomerAI ai);
                if (ai && ai.canvasObject)
                    ai.canvasObject.SetActive(true);
            }
            //  print("isAICashierUnlocked"+isAICashierUnlocked);
            //  print("isPlayerInside"+isPlayerInside);
            //  print("isBusy"+_isBusy);
        }

        private void OnTriggerStay(Collider other)
        {
//            print("Trigger stay"+other.name);
            if (other.CompareTag("Player"))
            {
                StartCoroutine(AICashierUnlocked());
                isPlayerInside = true;
            }
        }

        public GameObject playerPoint;
        public GameObject lookatPoint;
        public bool onceOnly;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInside = true;

                if (!_isBusy)
                {
                    _isBusy = true;
                    _playerCoroutine = StartCoroutine(CheckOut());
                    if (!onceOnly && !ReferenceManager.Instance.GameData.isTutorialCompleted)
                    {
                        ReferenceManager.Instance.playerStackManager.myRigidBody.isKinematic = true;
                        onceOnly = true;
                        other.transform.DOMove(this.transform.position, 1f);
                        ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.Empty);
                    }
                }
            }

            if (other.CompareTag("AI"))
            {
                selectedAiReached = true;
            }
        }

        public bool selectedAiReached;
        bool isCheckOut = false;
        private CustomerAI ai;
        int _index = 0;
        [SerializeField] private Vector3 offset;

        //  public Stack<ClothFragment> IronClothes = new Stack<ClothFragment>();
        float multiplier;
        public int income;
        public int cofeeToServe;
        public int coffeeGiven;


        [SerializeField] private Image coffeeTimerImage; // assign in Inspector
        [SerializeField] private Image coffeeImage; // assign in Inspector
        private Coroutine _coffeeTimerCoroutine;

        private bool _coffeeTimeout = false;

//         public IEnumerator CheckOut()
//         {
//             yield return new WaitForSeconds(0.5f);
//             if (isPlayerInside && ReferenceManager.Instance.queueSystem._washedClothQueueAi.Count > 0)
//             {
//                 if (_index == 0)
//                 {
//                     ai = (CustomerAI)ReferenceManager.Instance.queueSystem._washedClothQueueAi.Peek();
//                     cofeeToServe = ReferenceManager.Instance.coffeeBarHandler.coffeeConsumeTrigger.serveIndex =
//                         ai.coffeeCupsRequire;
//                     ai.canvasObject.gameObject.SetActive(true);
// //                    print("new ai found"+ai.name);
//                     print(cofeeToServe);
//                 }
//
//
//                 if (!ai)
//                 {
//                     _index = 0;
//                     _isBusy = false;
//                     _playerCoroutine = null;
//                     yield break;
//                 }
//
// //            print("cloths to give"+ReadyToShipClothes.Count +" and ai clothes:"+ai.clothStack);
//                 while (_index < ai.clothStack && ReadyToShipClothes.Count > 0)
//                 {
//                     ClothFragment cloth = ReadyToShipClothes.Pop();
//
//                     isCheckOut = false;
//
//                     if (ai.customerObjectToStack.myClothFragment.Count <= 0)
//                     {
//                         cloth.transform.DOMove(ai.customerObjectToStack.transform.position + offset, .2f).OnComplete(
//                             () =>
//                             {
//                                 isCheckOut = true;
//                                 cloth.transform.SetParent(ai.customerObjectToStack.transform);
//                                 ai.customerObjectToStack.myClothFragment.Add(cloth);
//                             });
//                         ai.SetIk(1);
//                     }
//                     else
//                     {
//                         cloth.transform
//                             .DOMove(
//                                 ai.customerObjectToStack.myClothFragment[^1].nextPosition.transform.position + offset,
//                                 .2f).OnComplete(() =>
//                             {
//                                 isCheckOut = true;
//                                 cloth.transform.SetParent(ai.customerObjectToStack.transform);
//                                 ai.customerObjectToStack.myClothFragment.Add(cloth);
//                             });
//                     }
//
//                     yield return new WaitUntil(() => isCheckOut);
//                     isCheckOut = false;
//                     _index++;
//                     ai.clothesText.text = _index + "/" + ai.clothStack;
//                 }
//
//                 if (ReferenceManager.playerHasTheCoffee && coffeeGiven <= cofeeToServe)
//                 {
//                     coffeeGiven++;
//                     ai.requireCoffeeText.text = coffeeGiven + "/" + cofeeToServe;
//                     ReferenceManager.Instance.playerStackManager.CupsOff();
//                 }
//
//                 if (_index == ai.clothStack && coffeeGiven >= cofeeToServe)
//                 {
//                     ReferenceManager.Instance.queueSystem.DequeueAndDestroyWashedQueue();
//                     income = _index * 10;
//                     if (ReferenceManager.Instance.snackbarManager.IsBoostActive())
//                     {
//                         multiplier = UnityEngine.Random.Range(10f, 20f);
//                         multiplier = multiplier / 100f;
//                     }
//                     else
//                     {
//                         multiplier = 0;
//                     }
//
//                     _index = 0;
//                     StartCoroutine(CashGenerate());
//
//                     ReferenceManager.Instance.snackbarManager.tipCash += income;
//                     ReferenceManager.Instance.snackbarManager.OnCustomerServed();
//                     ShowTipText($"{income} $, {(income * multiplier):F2} Tips+");
//                     yield return new WaitUntil(() => selectedAiReached);
//                     yield return new WaitForSeconds(1f);
//
//
//                     if (!ReferenceManager.Instance.GameData.isTutorialCompleted)
//                     {
//                         ReferenceManager.Instance.tutorialHandler.TaskCompleted();
//                         ReferenceManager.Instance.tutorialHandler.UnSubscribe();
//                     }
//
//                     ReferenceManager.playerHasTheCoffee = false;
//                     coffeeGiven = 0;
//                 }
//
// //                print("Nulling coroutine");
//                 _isBusy = false;
//                 _playerCoroutine = null;
//                 yield return null;
//                 // break;
//             }
//             else
//             {
//                 ///                print("Nulling coroutine2");
//                 _isBusy = false;
//                 _playerCoroutine = null;
//             }
//         }
        public IEnumerator CheckOut()
        {
            yield return new WaitForSeconds(0.5f);

            if (isPlayerInside && ReferenceManager.Instance.queueSystem._washedClothQueueAi.Count > 0)
            {
                if (_index == 0)
                {
                    ai = (CustomerAI)ReferenceManager.Instance.queueSystem._washedClothQueueAi.Peek();
                    cofeeToServe = ReferenceManager.Instance.coffeeBarHandler.coffeeConsumeTrigger.serveIndex =
                        ai.coffeeCupsRequire;
                    ai.canvasObject.gameObject.SetActive(true);
                    print(cofeeToServe);
                }

                if (!ai)
                {
                    _index = 0;
                    _isBusy = false;
                    _playerCoroutine = null;
                    yield break;
                }

                while (_index < ai.clothStack && ReadyToShipClothes.Count > 0)
                {
                    ClothFragment cloth = ReadyToShipClothes.Pop();
                    isCheckOut = false;

                    if (ai.customerObjectToStack.myClothFragment.Count <= 0)
                    {
                        cloth.transform.DOMove(ai.customerObjectToStack.transform.position + offset, .2f).OnComplete(
                            () =>
                            {
                                isCheckOut = true;
                                cloth.transform.SetParent(ai.customerObjectToStack.transform);
                                ai.customerObjectToStack.myClothFragment.Add(cloth);
                            });
                        ai.SetIk(1);
                    }
                    else
                    {
                        cloth.transform.DOMove(
                            ai.customerObjectToStack.myClothFragment[^1].nextPosition.transform.position + offset,
                            .2f).OnComplete(() =>
                        {
                            isCheckOut = true;
                            cloth.transform.SetParent(ai.customerObjectToStack.transform);
                            ai.customerObjectToStack.myClothFragment.Add(cloth);
                        });
                    }

                    yield return new WaitUntil(() => isCheckOut);
                    isCheckOut = false;
                    _index++;
                    ai.clothesText.text = _index + "/" + ai.clothStack;
                }

                // 🟡 Start coffee timer if clothes are done but coffee not given
                if (_index == ai.clothStack && coffeeGiven < cofeeToServe && _coffeeTimerCoroutine == null)
                {
                    _coffeeTimeout = false;
                    coffeeTimerImage.gameObject.SetActive(true);
                    coffeeImage.gameObject.SetActive(true);
                    if (_coffeeTimerCoroutine != null) StopCoroutine(_coffeeTimerCoroutine);
                    _coffeeTimerCoroutine = StartCoroutine(StartCoffeeWaitTimer(60f)); // 5 sec for example
                }

                // ✅ Check for coffee giving
                if (ReferenceManager.playerHasTheCoffee && coffeeGiven < cofeeToServe)
                {
                    coffeeGiven++;
                    ai.requireCoffeeText.text = coffeeGiven + "/" + cofeeToServe;
                    ReferenceManager.Instance.playerStackManager.CupsOff();
                    if (coffeeGiven >= cofeeToServe)
                        ReferenceManager.playerHasTheCoffee = false;
                    // ✅ Cancel timer and hide image if done
                    if (coffeeGiven >= cofeeToServe && _coffeeTimerCoroutine != null)
                    {
                        StopCoroutine(_coffeeTimerCoroutine);
                        coffeeTimerImage.fillAmount = 0;
                        coffeeTimerImage.gameObject.SetActive(false);
                        coffeeImage.gameObject.SetActive(false);
                        _coffeeTimerCoroutine = null;
                    }
                }

                // ✅ Success: All clothes and coffee given
                if (_index >= ai.clothStack && coffeeGiven >= cofeeToServe)
                {
                    var removeAi = ai;
                    if (removeAi)
                    {
                        removeAi.canvasObject.gameObject.SetActive(false);
                        removeAi.ShowBadReview(Satisfaction.Satisfied);
                    }

                    ReferenceManager.Instance.queueSystem.DequeueAndDestroyWashedQueue();
                    income = _index * 10;
                    ReferenceManager.Instance.coffeeBarHandler.coffeeConsumeTrigger.serveIndex = 0;

                    multiplier = ReferenceManager.Instance.snackbarManager.IsBoostActive()
                        ? UnityEngine.Random.Range(10f, 20f) / 100f
                        : 0f;

                    _index = 0;
                    StartCoroutine(CashGenerate());

                    ReferenceManager.Instance.snackbarManager.tipCash += income;
                    ReferenceManager.Instance.snackbarManager.OnCustomerServed();


                    var tipText = $"{income} $, {(income * multiplier):F2} Tips+";
                    ShowTipText(tipText);

                    yield return new WaitUntil(() => selectedAiReached);
                    yield return new WaitForSeconds(1f);

                    if (!ReferenceManager.Instance.GameData.isTutorialCompleted)
                    {
                        ReferenceManager.Instance.tutorialHandler.TaskCompleted();
                        ReferenceManager.Instance.tutorialHandler.UnSubscribe();
                    }

                    ReferenceManager.playerHasTheCoffee = false;
                    coffeeGiven = 0;

                    // Cancel and hide timer if still active
                    if (_coffeeTimerCoroutine != null)
                    {
                        StopCoroutine(_coffeeTimerCoroutine);
                        coffeeTimerImage.fillAmount = 0;
                        coffeeTimerImage.gameObject.SetActive(false);
                        _coffeeTimerCoroutine = null;
                    }
                }

                _isBusy = false;
                _playerCoroutine = null;
                yield return null;
            }
            else
            {
                _isBusy = false;
                _playerCoroutine = null;
            }
        }

        private IEnumerator StartCoffeeWaitTimer(float duration)
        {
            coffeeTimerImage.gameObject.SetActive(true);
            float timeElapsed = 0f;

            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                coffeeTimerImage.fillAmount = 1f - (timeElapsed / duration);
                yield return null;
            }

            coffeeTimerImage.fillAmount = 0f;
            coffeeTimerImage.gameObject.SetActive(false);


            _coffeeTimeout = true;
            if (!isCoffeeServed())
            {
                ai.canvasObject.gameObject.SetActive(false);
                ai.ShowBadReview(Satisfaction.Unsatisfied); // Optional visual feedback
                ReferenceManager.Instance.queueSystem.DequeueAndDestroyWashedQueue();

                _index = 0;
                coffeeGiven = 0;
                ReferenceManager.Instance.coffeeBarHandler.coffeeConsumeTrigger.serveIndex = 0;
                ReferenceManager.playerHasTheCoffee = false;
                ReferenceManager.Instance.playerStackManager.CupsOff();
                coffeeImage.gameObject.SetActive(false);
                _isBusy = false;
                _playerCoroutine = null;
            }
        }

        public TextMeshPro textToShow;

        public void ShowTipText(string tipText)
        {
            textToShow.gameObject.SetActive(true);

            textToShow.text = tipText;

            DOVirtual.DelayedCall(2f, () => textToShow.gameObject.SetActive(false));
            if (cofeeToServe > 0)
                DOVirtual.DelayedCall(1.2f, () => ShowCoffeeIncome("Coffee: " + (float)cofeeToServe * 7.5f + "$"));
        }

        public TextMeshPro cofeePrice;

        public void ShowCoffeeIncome(string tipText)
        {
            cofeePrice.gameObject.SetActive(true);

            cofeePrice.text = tipText;

            DOVirtual.DelayedCall(2f, () => cofeePrice.gameObject.SetActive(false));
        }

        #endregion

        #region CashCollectionRegion

        [SerializeField] private Transform[] cashStackPositions = new Transform[3]; // Assign in Unity Inspector
        public int cashStackCount = 0;
        public CashScript cashPrefab;
        public GameObject InstentiatePoint;
        public float changeInY;

        private IEnumerator CashGenerate()
        {
            if (cashPrefab && cashStackPositions.Length == 3)
            {
                // Determine column index (0 = Left, 1 = Center, 2 = Right)
                int columnIndex = cashStackCount % 3;

                // Increase Y position after completing one row (every 3 cash items)
                if (columnIndex == 0 && cashStackCount > 0)
                {
                    changeInY += 0.1f; // Adjust stacking height
                }

                if (cashStackCount == 0)
                {
                    changeInY = 0.1f;
                }

                // Get base position from predefined transforms
                Transform basePosition = cashStackPositions[columnIndex]; // Use correct position

                // Instantiate cash at base position
                CashScript cash = Instantiate(cashPrefab, basePosition.position, Quaternion.identity);

                // Jump animation to target stacking position
                cash.transform.DOJump(
                    new Vector3(basePosition.position.x, basePosition.position.y + changeInY, basePosition.position.z),
                    1, 1, 0.2f // Adjust jump duration
                );

                // Set cash parent
                cash.transform.SetParent(basePosition);

                // Adjust height if stacking point exists
                Transform stackingPoint = cash.cashStackPoint?.transform;
                if (stackingPoint)
                {
                    cash.transform.position = stackingPoint.position;
                }

                // Add cash to stack
                cashCollectionTrigger.cashStack.Add(cash);

                // Play sound if available
                if (SoundManager.instance)
                {
                    SoundManager.instance.Play(SoundName.CashOut);
                }

                cashStackCount++; // Increment stack count
                yield return null;
            }
        }

        public CashCollectionTrigger cashCollectionTrigger;

        #endregion

        public bool isCoffeeServed()
        {
            return coffeeGiven >= cofeeToServe;
        }
    }
}
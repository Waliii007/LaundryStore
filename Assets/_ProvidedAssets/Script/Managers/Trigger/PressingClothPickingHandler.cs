using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Invector.vCharacterController;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace LaundaryMan
{
    public class PressingClothPickingHandler : Dropper
    {
        public Stack<ClothFragment> clothToPress = new Stack<ClothFragment>();
        public GameObject stackStarter;
        public GameObject aiPickPoint;

        public float stackTime = 1f;
        private bool isPlayerInside = false;
        private Coroutine aiCoroutine;
        private Coroutine playerCoroutine;

        #region TriggerEvent

        private void OnEnable()
        {
            StartCoroutine(Arranger());
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.TryGetComponent(out AICleanClothes cleanClothes);
                other.TryGetComponent(out AiStackManager aiStackManager);


                if (aiStackManager &&
                    cleanClothes.currentState == AICleanClothes.AIState.Picking)
                {
                    if (GlobalConstant.isLogger)
                        print("mub");
                    isPlayerInside = true;
                    if (aiCoroutine == null)
                    {
                        aiCoroutine = StartCoroutine(HandlePlayerInteraction(aiStackManager));
                        return;
                    }
                }
                else if (other.TryGetComponent(out PlayerStackManager playerStackManager) && cleanClothes == null)
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
                    //    StopCoroutine(playerCoroutine);
                    playerCoroutine = null;
                }

                aiCoroutine = null;
            }
        }

        #endregion


        public void Push(ClothFragment cloth)
        {
            clothToPress.Push(cloth);
            if (SoundManager.instance)
            {
                SoundManager.instance.Play(SoundName.Drop);
            }
        }

        private bool isAiPickPoint;

        public ClothFragment Peek()
        {
            return clothToPress.ElementAt(clothToPress.Count - 1);
        }

        private IEnumerator HandlePlayerInteraction(PlayerStackManager playerStackManager)
        {
            playerStackManager.TryGetComponent(out vThirdPersonInput tpi);
            if (tpi)
                yield return new WaitUntil(() => !tpi.IsMoving());
            while (isPlayerInside &&
                   playerStackManager.ClothStack.Count < ReferenceManager.Instance.basketTrigger.limit)
            {
                if (clothToPress.Count > 0)
                {
                    var cloth = clothToPress.Pop();
                    cloth.transform.SetParent(playerStackManager.stackStarter.transform);

                    Vector3 targetPosition = (playerStackManager.ClothStack.Count == 0)
                        ? playerStackManager.stackStarter.transform.position
                        : playerStackManager.ClothStack.Peek().nextPosition.transform.position;
                    cloth.transform.DOJump(targetPosition + new Vector3(0, .01f, 0), 1f, 1, stackTime).OnComplete(() =>
                    {
                        playerStackManager.ClothStack.Push(cloth);
                        RearrangeStack(playerStackManager);
                        RearrangeStack(clothToPress, stackStarter);
                        if (SoundManager.instance)
                        {
                            SoundManager.instance.Play(SoundName.Drop);
                        }
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


        IEnumerator Arranger()
        {
            Vector3 startPosition = stackStarter.transform.position;
            List<ClothFragment> tempList = clothToPress.ToList();
            tempList.Reverse();
            for (int i = 0; i < tempList.Count; i++)
            {
                Vector3 targetPosition = (i == 0) ? startPosition : tempList[i - 1].nextPosition.transform.position;
                tempList[i].transform.transform.SetPositionAndRotation(targetPosition, quaternion.identity);
            }

            yield return waitTimeForArranger;
            if (ReferenceManager.Instance.isGameEnd)
                StartCoroutine(Arranger());
        }

        public WaitForSeconds waitTimeForArranger = new WaitForSeconds(1f);

        private void RearrangeStack(Stack<ClothFragment> _playerStack, GameObject _stackStarter)
        {
            Vector3 startPosition = _stackStarter.transform.position;
            List<ClothFragment> tempList = _playerStack.ToList();
            tempList.Reverse();
            for (int i = 0; i < tempList.Count; i++)
            {
                Vector3 targetPosition = (i == 0) ? startPosition : tempList[i - 1].nextPosition.transform.position;
                tempList[i].transform.transform.SetPositionAndRotation(targetPosition, quaternion.identity);
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
    }
}
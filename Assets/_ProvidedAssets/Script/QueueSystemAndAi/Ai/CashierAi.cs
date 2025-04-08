using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using UnityEngine;

namespace LaundaryMan
{
    public class CashierAi : Ai
    {
        private static readonly int Movement = Animator.StringToHash("Movement");
        private static readonly int IsTyping = Animator.StringToHash("IsTyping");
        public FollowerEntity aiPathFinder;
        public AIDestinationSetter aiDestinationSetter;
        public GameObject cashPointToTarget;
        public CashierAiState cashierAiState;

        public enum CashierAiState
        {
        }

        protected override void OnAiEnable()
        {
            base.OnAiEnable();
            ReferenceManager.Instance.checkoutHandler.isAICashierUnlocked = true;
        }

        public IEnumerator OnCounter()
        {
            aiPathFinder.enabled = true;
            DOVirtual.DelayedCall(1f, () =>
            {
                aiPathFinder.enabled = true;
                aiDestinationSetter.enabled = true;
                aiPathFinder.SetDestination(cashPointToTarget.transform.position);
            });
            yield return new WaitUntil(() =>
                Vector3.Distance(this.gameObject.transform.position, cashPointToTarget.transform.position) < 1
            );
            animator.SetLayerWeight(1, 1);
            animator.SetBool(IsTyping, true);
            
        }


        public void OnGetHireState()
        {
            aiPathFinder.enabled = true;
            DOVirtual.DelayedCall(1f, () =>
            {
                aiPathFinder.enabled = true;
                aiDestinationSetter.enabled = true;
                aiPathFinder.SetDestination(cashPointToTarget.transform.position);
            });
        }

        public override void OnStartGame()
        {
            base.OnStartGame();


            // if (!ReferenceManager.Instance.taskHandler.workerUnlockHandler.isCashierUnlocked)
            // {
            //     OnGetHireState();
            // }
            // else
            // {
            StartCoroutine(OnCounter());
            // }
        }

        protected override void OnAnimationUpdate()
        {
            base.OnAnimationUpdate();
            animator.SetFloat(Movement, aiPathFinder.velocity.magnitude);
        }
    }
}
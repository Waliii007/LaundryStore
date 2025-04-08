using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;
using UnityEngine;

namespace LaundaryMan
{
    public class AICleanClothes : Ai
    {
        [System.Serializable]
        public class Task
        {
            public GameObject PickPosition;
            public GameObject DropPosition;
            public WashingMachineDropper dropper;

            public Task(GameObject pick, GameObject drop)
            {
                PickPosition = pick;
                DropPosition = drop;
            }
        }
        private static readonly int Movement = Animator.StringToHash("Movement");

        public enum AIState
        {
            Idle,
            MovingToPick,
            Picking,
            MovingToDrop,
            Dropping
        }

        public AIState currentState = AIState.Idle;
        private Task currentTask;
        public AiStackManager aiStackManager;
        public FollowerEntity followerEntity;
        public AIDestinationSetter aIDestinationSetter;
        public int limitToPickClothes = 0;
        public WashingMachineDropper washingMachineDropper;

        protected override void OnAiEnable()
        {
            base.OnAiEnable();
            DOVirtual.DelayedCall(2, RegisterMe);
        }

        public GameObject PickPoint;
        public GameObject DropPoint;

        protected override void OnAnimationUpdate()
        {
            base.OnAnimationUpdate();
            animator.SetFloat(Movement, followerEntity.velocity.magnitude);
        }

        public void AssignTask(AICleanClothes.Task task)
        {
            currentTask = task;
            ChangeState(AIState.MovingToPick);
//            print("Task assigned");
            PickPoint = task.PickPosition;
            DropPoint = task.DropPosition;
            washingMachineDropper = task.dropper;
        }

        private void ChangeState(AIState newState)
        {
            if (currentState == newState) return;
            currentState = newState;

            switch (currentState)
            {
                case AIState.Idle:
                    RegisterMe();
                    break;
                case AIState.MovingToPick:
                    StartCoroutine(MoveToPosition(currentTask.PickPosition.transform, AIState.Picking));
                    break;
                case AIState.Picking:
                    StartCoroutine(PickLaundry());
                    break;
                case AIState.MovingToDrop:
                    
                    StartCoroutine(MoveToPosition(currentTask.DropPosition.transform, AIState.Dropping));
                    break;
                case AIState.Dropping:
                    
                    StartCoroutine(DropLaundry());
                    break;
            }
        }


        public override void SetDestination(GameObject destination)
        {
            aIDestinationSetter.target = destination.transform;
        }

        private IEnumerator MoveToPosition(Transform targetPosition, AIState nextState)
        {
            switch (currentState)
            {
                case AIState.Idle:
                    break;
                case AIState.MovingToPick:
                    SetDestination(targetPosition.gameObject);
                    yield return new WaitUntil(() =>
                        Vector3.Distance(transform.position, targetPosition.position) < 0.5f);
                    ChangeState(nextState);
                    break;
                case AIState.Picking:
                    StartCoroutine(PickLaundry());
                    break;
                case AIState.MovingToDrop:
                    SetDestination(targetPosition.gameObject);
                    yield return new WaitUntil(() =>
                        Vector3.Distance(transform.position, targetPosition.position) < 0.5f);
                    ChangeState(nextState);
                    break;
                case AIState.Dropping:
                    break;
            }

            ChangeState(nextState);
        }
        WaitForSeconds waitForSecondsToPickClothes = new WaitForSeconds(2f);
        private IEnumerator PickLaundry()
        {
            yield return new WaitUntil(() =>
                aiStackManager.ClothStack.Count >= limitToPickClothes || washingMachineDropper.washedClothes.Count <= 0);
            yield return waitForSecondsToPickClothes;
            ChangeState(AIState.MovingToDrop);
        }

        private IEnumerator DropLaundry()
        {
            yield return new WaitUntil(() => aiStackManager.ClothStack.Count <= 0);
            ReferenceManager.Instance.cleanBoxAIManager.UnregisterAgent(this);
            washingMachineDropper.isOccupied = false;
            yield return waitForSecondsToPickClothes;
            ChangeState(AIState.Idle);
            SetDestination(sleepPoint);
            
        }
        public GameObject sleepPoint;
        public void RegisterMe()
        {
            ReferenceManager.Instance.cleanBoxAIManager.RegisterAgent(this);
            if (washingMachineDropper)
            {
                washingMachineDropper.isOccupied = false;
                washingMachineDropper = null;
            }
        }
    }
}
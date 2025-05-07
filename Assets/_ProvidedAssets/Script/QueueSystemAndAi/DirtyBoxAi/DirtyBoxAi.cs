using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LaundryMan;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LaundaryMan
{
    public class DirtyBoxAi : Ai
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
        public GameObject PickPoint;
        public GameObject DropPoint;
        public GameObject sleepPoint;

        private float stateStartTime;
        private float maxStateDuration = 10f;
        private Coroutine observerCoroutine;
        WaitForSeconds waitForSecondsToPickClothes = new WaitForSeconds(2f);

        protected override void OnAiEnable()
        {
            base.OnAiEnable();
            DOVirtual.DelayedCall(2, RegisterMe);
            DOVirtual.DelayedCall(3, ReferenceManager.Instance.dirtyBoxAiManager.AssignTask);
        }

        public bool isInitialized
        {
            get => PlayerPrefs.GetInt("isInitialized" + this.name, 0) == 1;
            set => PlayerPrefs.SetInt("isInitialized" + this.name, value ? 1 : 0);
        }

        protected override void OnAnimationUpdate()
        {
            base.OnAnimationUpdate();
            animator.SetFloat(Movement, followerEntity.velocity.magnitude);
            followerEntity.enabled = washingMachineDropper == null || washingMachineDropper.refillAmount > 0;
        }

        public void AssignTask(Task task)
        {
            currentTask = task;
            PickPoint = task.PickPosition;
            DropPoint = task.DropPosition;
            washingMachineDropper = task.dropper;
            ChangeState(AIState.MovingToPick);
        }

        private void ChangeState(AIState newState)
        {
            if (currentState == newState) return;
            currentState = newState;

            stateStartTime = Time.time;
            if (observerCoroutine != null)
                StopCoroutine(observerCoroutine);
            observerCoroutine = StartCoroutine(StateObserver());

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

        private IEnumerator StateObserver()
        {
            while (true)
            {
                if (currentTask != null && currentTask.dropper)
                {
                    yield return new WaitUntil((() =>
                        !currentTask.dropper.isDetergentEmpty && aiStackManager.ClothStack.Count > 0));
                }

                if (Time.time - stateStartTime > maxStateDuration && aiStackManager.ClothStack.Count <= 0)
                {
                    RegisterMe();
                    ChangeState(AIState.Idle);
                    yield break;
                }
                else
                {
                    stateStartTime = maxStateDuration;
                }

                yield return waitForSecondsChangeState;
            }
        }

        public WaitForSeconds waitForSecondsChangeState = new WaitForSeconds(1f);

        public override void SetDestination(GameObject destination)
        {
            aIDestinationSetter.target = destination.transform;
        }

        private IEnumerator MoveToPosition(Transform targetPosition, AIState nextState)
        {
            if (currentState == AIState.MovingToPick)
            {
                yield return new WaitUntil(() => !ReferenceManager.Instance.basketTrigger.isPlayerInside);
            }

            SetDestination(targetPosition.gameObject);
            yield return new WaitUntil(() => Vector3.Distance(transform.position, targetPosition.position) < 0.5f);
            ChangeState(nextState);
        }

        private IEnumerator PickLaundry()
        {
            yield return new WaitUntil(() => aiStackManager.ClothStack.Count >= aiStackManager.maxClothesPerCycle);
            yield return waitForSecondsToPickClothes;
            ChangeState(AIState.MovingToDrop);
        }

        private IEnumerator DropLaundry()
        {
            yield return new WaitUntil(() => aiStackManager.ClothStack.Count <= 0);
            ReferenceManager.Instance.dirtyBoxAiManager.UnregisterAgent(this);
            if (washingMachineDropper)
            {
                washingMachineDropper.isOccupiedbyDirty = false;
            }

            yield return waitForSecondsToPickClothes;
            ChangeState(AIState.Idle);
            SetDestination(sleepPoint);
        }

        public void RegisterMe()
        {
            ReferenceManager.Instance.dirtyBoxAiManager.RegisterAgent(this);
            if (washingMachineDropper)
            {
                washingMachineDropper.isOccupiedbyDirty = false;
                ReferenceManager.Instance.basketTrigger.StartCheckingLoop(1);
                washingMachineDropper = null;
            }
        }
    }
}
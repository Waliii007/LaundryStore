using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.RVO;
using Unity.VisualScripting;
using UnityEngine;

namespace LaundaryMan
{
    public class Character : MonoBehaviour
    {
        public Animator animator;
        protected float characterSpeed = 2f;


        private void OnEnable()
        {
            OnAiEnable();
        }

        private void Start()
        {
            OnStartGame();
        }

        private void Update()
        {
            OnAnimationUpdate();
        }

        private void LateUpdate()
        {
            OnAiUpdate();
        }

        public virtual void OnStartGame()
        {
        }

        protected virtual void OnAiUpdate()
        {
        }

        protected virtual void OnAiEnable()
        {
        }

        protected virtual void OnAnimationUpdate()
        {
        }
    }
}
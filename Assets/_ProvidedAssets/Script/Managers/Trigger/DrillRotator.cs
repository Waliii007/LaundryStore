using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace LaundaryMan
{
    public class DrillRotator : MonoBehaviour
    {
        public Vector3 vec;
        public float speed;

        private void Awake()
        {
            WaitForSeconds = new WaitForSeconds(0.001f);
        }

        public WaitForSeconds WaitForSeconds;

        IEnumerator Start()
        {
            while (ReferenceManager.Instance.isGameEnd)
            {
                transform.Rotate(vec, speed);
                yield return WaitForSeconds;
            }
        }
    }
}
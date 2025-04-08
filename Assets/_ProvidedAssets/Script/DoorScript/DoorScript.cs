using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LaundaryMan
{
    public class DoorScript : MonoBehaviour
    {
        private static readonly int DoorOpen = Animator.StringToHash("DoorOpen");
        public Animator animator;

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                animator.SetBool(DoorOpen, true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                animator.SetBool(DoorOpen, false);
            }
            
        }
    }
}
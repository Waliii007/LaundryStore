using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;

namespace LaundaryMan
{
    public class PlayerStackManager : MonoBehaviour
    {
        public GameObject stackStarter;
        [SerializeField] protected FullBodyBipedIK bipedIk;
        public Stack<ClothFragment> ClothStack = new Stack<ClothFragment>();
        
        private void Awake()
        {
            SetIk(0);
        }

        public void SetIk(float iKWeight)
        {
            bipedIk.solver.leftHandEffector.positionWeight = iKWeight;
            bipedIk.solver.leftHandEffector.rotationWeight = iKWeight;
            bipedIk.solver.rightHandEffector.positionWeight = iKWeight;
            bipedIk.solver.rightHandEffector.rotationWeight = iKWeight;
            bipedIk.solver.leftHandEffector.rotationWeight = iKWeight;
        }
    }
}
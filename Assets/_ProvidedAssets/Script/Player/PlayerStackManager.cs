using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.AI;

namespace LaundaryMan
{
    public class PlayerStackManager : MonoBehaviour
    {
        public GameObject stackStarter;
        [SerializeField] protected FullBodyBipedIK bipedIk;
        public Stack<ClothFragment> ClothStack = new Stack<ClothFragment>();
        public NavmeshPathDraw pathDraw;
        public int maxClothesPerCycle = 10;
        public int currentClothesCount = 0;
        private void Awake()
        {
            SetIk(0);
            
        }

        public int ReturnIkValue()
        {
            return (int)bipedIk.solver.leftHandEffector.positionWeight;
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
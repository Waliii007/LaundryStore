using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using RootMotion.FinalIK;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LaundaryMan
{
    public class CustomerAI : Ai
    {
        [SerializeField] protected AIDestinationSetter navMeshDestination;
        [SerializeField] private FollowerEntity followerEntity;
        public ClothPrefab customerObjectToStack;
        public int clothStack;
        public List<Animator> aiSkins;
        public GameObject leftPoint;
        public GameObject rightPoint;

        protected override void OnAiEnable()
        {
            int i = Random.Range(0, aiSkins.Count);

            animator.avatar = aiSkins.ElementAt(i).avatar;
            aiSkins[i].gameObject.SetActive(true);
            for (int j = 0; j < aiSkins.Count; j++)
            {
                if (j != i)
                {
                    Destroy(aiSkins[j].gameObject);
                }
            }

            if (aiSkins[i].TryGetComponent(out Animator anim))
            {
                animator = anim;
            }

            if (aiSkins[i].TryGetComponent(out FullBodyBipedIK ik))
            {
                bipedIk = ik;
            }

            bipedIk.solver.leftHandEffector.target = leftPoint.transform;
            bipedIk.solver.rightHandEffector.target = rightPoint.transform;
            SetIk(1);
        }


        private void Start()
        {
            clothStack =
                customerObjectToStack.myClothFragment.Count;
        }

        public void SetIk(float iKWeight)
        {
            bipedIk.solver.leftHandEffector.positionWeight = iKWeight;
            bipedIk.solver.leftHandEffector.rotationWeight = iKWeight;
            bipedIk.solver.rightHandEffector.positionWeight = iKWeight;
            bipedIk.solver.rightHandEffector.rotationWeight = iKWeight;
            bipedIk.solver.leftHandEffector.rotationWeight = iKWeight;
        }

        public FullBodyBipedIK bipedIk;
        [SerializeField] private float magnitude;

        protected override void OnAnimationUpdate()
        {
            magnitude = followerEntity.velocity.magnitude;
            magnitude = Mathf.Clamp01(magnitude);
            animator.SetFloat("Movement", magnitude);
        }

        public override void SetDestination(GameObject target)
        {
            navMeshDestination.target = (target.transform);
        }

        public override float speed()
        {
            return followerEntity.maxSpeed;
        }
    }
}
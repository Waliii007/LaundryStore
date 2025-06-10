using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.UI;
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
        public bool requireCoffee;
        public int coffeeCupsRequire;
        public Text requireCoffeeText;
        public Text clothesText;
        public GameObject canvasObject;

        protected override void OnAiEnable()
        {
            canvasObject.gameObject.SetActive(false);


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
            if (ReferenceManager.Instance.GameData.isTutorialCompleted)
            {
                requireCoffee = Random.Range(0, 2) == 1;
                print(requireCoffee);
                coffeeCupsRequire = requireCoffee ? Random.Range(1, 3) : 0;
            }

            clothStack =
                customerObjectToStack.myClothFragment.Count;
            print(coffeeCupsRequire + ":" + "coffeeCupsRequire");
            requireCoffeeText.text = coffeeCupsRequire.ToString();
            clothesText.text = clothStack.ToString();
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

        public GameObject angry;
        public GameObject happy;

        public void ShowBadReview(Satisfaction satisfaction)
        {
            
            canvasObject.gameObject.SetActive(false);
            switch (satisfaction)
            {
                case Satisfaction.Satisfied:
                    happy.SetActive(true);
                    if (SoundManager.instance)
                    {
                        //SoundManager.instance.Play(SoundName.Happy);
                    }
                    break;
                case Satisfaction.Unsatisfied:
                    angry.SetActive(true);
                    if (SoundManager.instance)
                    {
                       // SoundManager.instance.Play(SoundName.Angry);
                    }
                    break;
            }

            //print("Fuck u i will give u a bad review" + "Teri maa ki siri bc you are a bad consumer");
        }
    }
}

public enum Satisfaction
{
    Satisfied,
    Unsatisfied
}
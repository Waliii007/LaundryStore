using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LaundaryMan
{
    public class ClothPrefab : MonoBehaviour
    {
        public GameObject stackStarter;
        public ClothFragment myCloth;
        public List<ClothFragment> myClothFragment;
        public CustomerAI myAiScript;

        private void Awake()
        {
            int j = Random.Range(5, 8);
            ReferenceManager.CustomerAmountOfClothForTutorial = j;
            for (int i = 0; i < j; i++)
            {
                if (i == 0)
                {
                    ClothFragment cloth = Instantiate(myCloth,
                        stackStarter.transform.position, Quaternion.identity, transform);
                    myClothFragment.Add(cloth);
                }
                else
                {
                    ClothFragment cloth = Instantiate(myCloth,
                        myClothFragment[^1].nextPosition.transform.position, Quaternion.identity, transform);
                    myClothFragment.Add(cloth);
                }
            }

            StartCoroutine(StackOrganizer());
        }

        private void RearrangeStack()
        {
            List<ClothFragment> tempList = myClothFragment;
            for (int i = 0; i < tempList.Count; i++)
            {
                Vector3 targetPosition = (i == 0)
                    ? stackStarter.transform.position
                    : tempList[i - 1].nextPosition.transform.position;
                tempList[i].transform.transform.SetPositionAndRotation(targetPosition, Quaternion.identity);
            }
        }

        public WaitForSeconds waitForSeconds = new WaitForSeconds(.5f);

        public IEnumerator StackOrganizer()
        {
            while (ReferenceManager.Instance.isGameEnd)
            {
                RearrangeStack();
                yield return waitForSeconds;
            }
        }
    }
}
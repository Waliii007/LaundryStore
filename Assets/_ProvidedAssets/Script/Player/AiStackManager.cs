using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace LaundaryMan
{
    public class AiStackManager : PlayerStackManager
    {
       
        
        public int maxClothesPerCycle = 5;
        private void Start()
        {
            StartCoroutine(ArrangeStack());
        }

        private IEnumerator ArrangeStack()
        {
            RearrangeStack(ClothStack, stackStarter);
            yield return new WaitForSeconds(.25f);
            StartCoroutine(ArrangeStack());
        }

        private List<ClothFragment> tempList = new List<ClothFragment>();

        private void RearrangeStack(Stack<ClothFragment> _playerStack, GameObject _stackStarter)
        {
            Vector3 startPosition = _stackStarter.transform.position;
            tempList = _playerStack.ToList();
            tempList.Reverse();
            for (int i = 0; i < tempList.Count; i++)
            {
                Vector3 targetPosition = (i == 0) ? startPosition : tempList[i - 1].nextPosition.transform.position;
                tempList[i].transform.SetPositionAndRotation(targetPosition, quaternion.identity);
            }
        }

        public void CheckIfPosition()
        {
            
        }
    }
}
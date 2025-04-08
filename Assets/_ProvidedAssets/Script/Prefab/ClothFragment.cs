using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LaundaryMan
{
    public class ClothFragment : MonoBehaviour
    {
        public GameObject nextPosition;
        public GameObject positionInherit;
        public ClothState state;

        public Cloth[] cloth;
        public Cloth selectedCloth;
        public bool doNotCallDefaultState;

        private void OnEnable()
        {
            int i = Random.Range(0, cloth.Length);
            selectedCloth.hangedClothes = cloth[i].hangedClothes;
            selectedCloth.dirtyCloth = cloth[i].dirtyCloth;
            selectedCloth.ironedCloth = cloth[i].ironedCloth;
            if (!doNotCallDefaultState)
                SetState(ClothState.Dirty);
            else
            {
                SetState(ClothState.Washing);
            }
        }

        public void SetState(ClothState newState)
        {
            state = newState;
//            print("called");
            switch (state)
            {
                case ClothState.Dirty:
                    selectedCloth.ironedCloth.SetActive(false);
                    selectedCloth.hangedClothes.SetActive(false);
                    selectedCloth.dirtyCloth.SetActive(true);
                    break;
                case ClothState.Washing:
                    selectedCloth.ironedCloth.SetActive(true);
                    selectedCloth.dirtyCloth.SetActive(false);
                    selectedCloth.hangedClothes.SetActive(false);
                    break;
                case ClothState.Ready:
                    selectedCloth.ironedCloth.SetActive(false);
                    selectedCloth.hangedClothes.SetActive(true);
                    selectedCloth.dirtyCloth.SetActive(false);
                    break;
            }
        }
    }

    public enum ClothState
    {
        Dirty,
        Washing,
        Ready,
    }
}

[System.Serializable]
public class Cloth
{
    public GameObject dirtyCloth;
    public GameObject ironedCloth;
    public GameObject hangedClothes;
}
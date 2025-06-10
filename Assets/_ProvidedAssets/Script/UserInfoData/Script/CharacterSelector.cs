using System;
using RootMotion.FinalIK;
using UnityEngine;

namespace LaundaryMan
{
    public class CharacterSelector : MonoBehaviour
    {
        public GameObject[] maleCharacters;
        public GameObject[] femaleCharacters;

        public Sex currentSex;
        public int characterIndex;
        public PlayerStackManager stackManager;
        public Avatar[] maleAvatar;
        public Avatar[] femaleAvatar;
        public Animator animator;
        public void Awake()
        {
           // PlayerPrefsManager.GenderSelection = Sex.Male;
            switch (PlayerPrefsManager.GenderSelection)
            {
                case Sex.Male:
                    maleCharacters[characterIndex].gameObject.SetActive(true);
                    stackManager.bipedIk = maleCharacters[characterIndex].GetComponent<FullBodyBipedIK>();
                    animator.avatar = maleAvatar[characterIndex];
                    animator.enabled = true;
                    break;
                case Sex.Female:
                    femaleCharacters[characterIndex].gameObject.SetActive(true);
                    stackManager.bipedIk = femaleCharacters[characterIndex].GetComponent<FullBodyBipedIK>();
                    animator.avatar = femaleAvatar[characterIndex];
                    animator.enabled = true;
                    
                    break;
            }
        }
    }
}

public enum Sex
{
    Male,
    Female
}
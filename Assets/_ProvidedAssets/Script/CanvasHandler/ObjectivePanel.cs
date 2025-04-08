using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class ObjectivePanel : MonoBehaviour
    {
        public Text objectiveText;  
        public float typingSpeed = 0.05f; 
        public void ShowObjective(string objective)
        {
            StartCoroutine(TypeObjective(objective));
        }

        public void BackButton()
        {
            ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.MainControls);
            if (SoundManager.instance)
            {
                SoundManager.instance.Play(SoundName.Click);
            }
        }
        
        private IEnumerator TypeObjective(string objective)
        {
            objectiveText.text = "";
            foreach (char letter in objective.ToCharArray())
            {
                objectiveText.text += letter;
                if (SoundManager.instance)
                {
                    SoundManager.instance.Play(SoundName.Click);
                }
                yield return new WaitForSeconds(typingSpeed);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class ObjectivePanel : MonoBehaviour
    {
        public Text objectiveText;
        public float typingSpeed = 0.05f;
        public Transform[] positionsHolders;
        public GameObject panel;

        public void ShowObjective(string objective)
        {
            int i = Random.Range(0, positionsHolders.Length);
            panel.transform.position = positionsHolders[i].position;
            panel.transform.DOLocalMove(new Vector3(0, 0, 0), 1).SetEase(Ease.OutBounce);
            /*panel.transform.DOMove(positions[i], 0.01f).OnComplete((() =>
            {
               
            }));*/
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
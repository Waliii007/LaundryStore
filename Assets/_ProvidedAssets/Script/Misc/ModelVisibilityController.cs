using UnityEngine;

namespace LaundaryMan
{
    public class ModelVisibilityController : MonoBehaviour
    {
        public Transform positionChecker;

        public GameObject character;

        // Update is called once per frame
        void Update()
        {
            character.SetActive(transform.position.y > positionChecker.position.y);
        }
    }
}
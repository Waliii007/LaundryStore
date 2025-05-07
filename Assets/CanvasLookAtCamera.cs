using UnityEngine;

namespace LaundaryMan
{
    public class CanvasLookAtCamera : MonoBehaviour
    {
        private Camera mainCamera;

        void Start()
        {
            mainCamera = ReferenceManager.Instance.mainCamera; // Get the main camera
        }

        void LateUpdate()
        {
            if (mainCamera)
            {
                transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                    mainCamera.transform.rotation * Vector3.up);
            }
        }
    }
}
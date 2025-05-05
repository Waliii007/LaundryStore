using UnityEngine;

namespace LaundaryMan
{
    public class SnackbarCanvas : MonoBehaviour
    {
        public MachineCanvasStates prev;
        public MachineCanvasStates currentState;
        [SerializeField] private GameObject[] canvasStates;

        private void OnEnable()
        {
            CanvasStateChanger(MachineCanvasStates.Full);
        }

        public void CanvasStateChanger(MachineCanvasStates newState)
        {
            if (canvasStates == null || canvasStates.Length == 0) return;

            prev = currentState;
            currentState = newState;

            canvasStates[(int)prev].SetActive(false);
            canvasStates[(int)currentState].SetActive(true);

            switch (newState)
            {
                case MachineCanvasStates.Full:
                    break;

                case MachineCanvasStates.RefillNeeded:
                    ReferenceManager.Instance.notificationHandler.ShowNotification("Boost is empty.");
                    break;
            }
        }
        
    }
}

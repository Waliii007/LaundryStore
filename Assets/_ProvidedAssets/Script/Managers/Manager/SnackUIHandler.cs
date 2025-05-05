using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class SnackUIHandler : MonoBehaviour
    {
        [Header("UI References")] [Header("Dependencies")]
        public SnackbarManager snackbarManager;

        public int machineIndex;
 
        #region Button Events

        public void OnClickUseGreen()
        {
            snackbarManager.BuyProPack();
          
        }

        public void OnClickUseBlue()
        {
            snackbarManager.BuyMaxPack();
         
        }

        public void OnClickUseRed()
        {
            snackbarManager.BuyProPack();
        }

        #endregion

    }
}
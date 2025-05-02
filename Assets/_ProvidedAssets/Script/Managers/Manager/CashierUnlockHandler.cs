using ToastPlugin;
using UnityEngine;

namespace LaundaryMan
{
    public class CashierUnlockHandler : MonoBehaviour
    {
        public void CashierUnlock()
        {
            TssAdsManager._Instance.ShowRewardedAd(HandleAdWatched, "CashierUnlock");
        }


        public void CashierUnlockBycash()
        {
            if (ReferenceManager.Instance.GameData.playerCash >=
                ReferenceManager.Instance.GameData.gameEconomy.cashierPrice)
            {
                ReferenceManager.Instance.GameData.gameEconomy.cashierPrice -=
                    ReferenceManager.Instance.GameData.gameEconomy.cashierPrice;
                ReferenceManager.Instance.GameData.isCashierUnlocked = true;
                ReferenceManager.Instance.SaveGameDataObserver();
                ReferenceManager.Instance.taskHandler.OnTaskCompleted();
                ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.MainControls);
            }
            else
            {
               ReferenceManager.Instance.notificationHandler.ShowNotification("Not enough coins to unlock the cashier.");
            }
        }

        public void Crossed()
        {
            ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.MainControls);
        }

        public void HandleAdWatched()
        {
            ReferenceManager.Instance.GameData.isCashierUnlocked = true;
            ReferenceManager.Instance.SaveGameDataObserver();
            ReferenceManager.Instance.taskHandler.OnTaskCompleted();
            ReferenceManager.Instance.canvasManager.CanvasStateChanger(CanvasStates.MainControls);

        }
    }
}
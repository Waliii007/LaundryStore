using UnityEngine;

namespace LaundaryMan
{
    public class CashierManager : MonoBehaviour
    {
        bool isCashierUnlocked = false;
        public GameObject cashier;

        private void OnEnable()
        {
            cashier.SetActive(ReferenceManager.Instance.GameData.isCashierUnlocked);
            
        }
    }
}
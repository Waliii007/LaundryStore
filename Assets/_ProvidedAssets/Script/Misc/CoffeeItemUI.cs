using UnityEngine;
using UnityEngine.UI;

namespace LaundaryMan
{
    public class CoffeeItemUI : MonoBehaviour
    {
        public Text gloriaText;
        public Text starText;
        public Text simText;
        public CoffeePurchase purchase;
        public int index;
        public GameObject crossButton;
        public bool once;

        private void OnEnable()
        {
            UpdateCoffeeUI();
            crossButton.SetActive(ReferenceManager.Instance.GameData.isTutorialCompleted);
            //  tutorialObject.SetActive(!ReferenceManager.Instance.GameData.isTutorialCompleted);
        }

        public void UseGloria()
        {
            purchase.OnUseCoffee(CoffeeType.GloriaPants, index);
            UpdateCoffeeUI();
        }

        public void UseStar()
        {
            purchase.OnUseCoffee(CoffeeType.StarDucks, index);
            UpdateCoffeeUI();
        }

        public void UseSim()
        {
            purchase.OnUseCoffee(CoffeeType.GloriaPants, index);
            UpdateCoffeeUI();
        }

        public void UpdateCoffeeUI()
        {
            gloriaText.text = "X" + purchase.GetCoffeeCount(CoffeeType.GloriaPants);
            starText.text = "X" + purchase.GetCoffeeCount(CoffeeType.StarDucks);
            simText.text = "X" + purchase.GetCoffeeCount(CoffeeType.SimBortan);
        }
    }
}
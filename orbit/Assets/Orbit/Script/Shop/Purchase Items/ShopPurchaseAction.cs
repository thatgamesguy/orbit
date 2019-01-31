using UnityEngine;
using UnityEngine.UI;

namespace Orbit
{
    /// <summary>
    /// Base shop purchase action. 
    /// </summary>
    public abstract class ShopPurchaseAction : MonoBehaviour
    {
        /// <summary>
        /// The shop item.
        /// </summary>
        public ShopItem item;

        /// <summary>
        /// returns true if this item has been upgraded the maximum amount of times.
        /// </summary>
        public bool fullyUpgraded { get { return item.currentNumberOfPurchases >= item.numOfPurchasesAllowed; } }

        private Image _image;

        void Awake()
        {
            _image = GetComponent<Image>();
            _image.fillAmount = 0f;
        }

        /// <summary>
        /// Performs shop item action.
        /// </summary>
        public virtual void PerformAction()
        {
            CollectibleManager.instance.Spent(item.itemPrice);

            item.currentNumberOfPurchases++;
            item.itemPrice += item.priceIncrementAmount;

            _image.fillAmount = (float)item.currentNumberOfPurchases / (float)item.numOfPurchasesAllowed;


            UpdateShopText();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if item not fullt upgraded and player can afford purchase.</returns>
        public virtual bool IsActionable()
        {
            return !fullyUpgraded && (CollectibleManager.instance.collected >= item.itemPrice);
        }

        void OnEnable()
        {
            UpdateShopText();
        }

        private void UpdateShopText()
        {
            var postText = (item.currentNumberOfPurchases >= item.numOfPurchasesAllowed) ? "Max" : item.itemPrice.ToString();
            item.buttonText.text = item.itemPreText + " \n" + postText;

        }

    
    }
}
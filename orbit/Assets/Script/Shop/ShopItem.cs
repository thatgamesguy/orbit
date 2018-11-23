using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Orbit
{
    /// <summary>
    /// Represents a purchasable item.
    /// </summary>
    [System.Serializable]
    public struct ShopItem
    {
        /// <summary>
        /// Reference to the items button text object.
        /// </summary>
        public Text buttonText;

        /// <summary>
        /// The button label.
        /// </summary>
        public string itemPreText;

        /// <summary>
        /// The price of the item.
        /// </summary>
        public int itemPrice;

        /// <summary>
        /// The amount the price is increased on successful purchase.
        /// </summary>
        public int priceIncrementAmount;

        /// <summary>
        /// The maximum number of purchases allowed.
        /// </summary>
        public int numOfPurchasesAllowed;

        /// <summary>
        /// The current number of purchases.
        /// </summary>
        public int currentNumberOfPurchases { get; set; }
    }
}
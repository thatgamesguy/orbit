using UnityEngine;
using System.Collections;

namespace Orbit
{
    /// <summary>
    /// Heal purchase action.
    /// </summary>
    public class HealPurchaseAction : ShopPurchaseAction
    {
        /// <summary>
        /// Amount to add to player health.
        /// </summary>
        public int amountToAdd = 5;

        /// <summary>
        /// Adds specified amount to health.
        /// </summary>
        public override void PerformAction()
        {
            base.PerformAction();

            GameManager.Instance.UpdateEnergy(amountToAdd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>returns true if player health is not full.</returns>
        public override bool IsActionable()
        {
            return base.IsActionable() && !GameManager.Instance.EnergyFull();
        }
    }
}

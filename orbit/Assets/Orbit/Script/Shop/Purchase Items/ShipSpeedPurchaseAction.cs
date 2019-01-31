namespace Orbit
{
    /// <summary>
    /// Ship speed purchase action. Increase sheep speed.
    /// </summary>
    public class ShipSpeedPurchaseAction : ShopPurchaseAction
    {
        /// <summary>
        /// The amount to add to ship speed.
        /// </summary>
        public int amountToAdd = 10;

        /// <summary>
        /// Adds the specified amount to the ships speed.
        /// </summary>
        public override void PerformAction()
        {
            base.PerformAction();

            if (GameManager.Instance.playerController.currentSpeed > 0)
            {
                GameManager.Instance.playerController.currentSpeed += amountToAdd;
            }
            else
            {
                GameManager.Instance.playerController.currentSpeed -= amountToAdd;
            }
        }
    }
}
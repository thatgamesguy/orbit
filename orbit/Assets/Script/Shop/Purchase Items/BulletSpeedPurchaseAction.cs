namespace Orbit
{
    /// <summary>
    /// Bullet speed purchase action.
    /// </summary>
    public class BulletSpeedPurchaseAction : ShopPurchaseAction
    {
        /// <summary>
        /// The amount to add to bullet speed.
        /// </summary>
        public float amountToAdd = 0.2f;

        /// <summary>
        /// Adds specified amount to projecile speed.
        /// </summary>
        public override void PerformAction()
        {
            base.PerformAction();

            GameManager.Instance.playerController.bulletSpeed += amountToAdd;
        }
    }
}

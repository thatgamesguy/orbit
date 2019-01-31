namespace Orbit
{
    /// <summary>
    /// Extra shot purchase action.
    /// </summary>
    public class AdditionalShotPurchaseAction : ShopPurchaseAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if ok to attach additional shot.</returns>
        public override bool IsActionable()
        {
            return base.IsActionable() && GameManager.Instance.playerController.OkToAttachAdditionalShot;
        }

        /// <summary>
        /// Adds additonal shot to player ship.
        /// </summary>
        public override void PerformAction()
        {
            base.PerformAction();

            GameManager.Instance.playerController.AddAdditionalShot();
        }
    }
}

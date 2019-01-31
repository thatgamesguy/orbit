namespace Orbit
{
    /// <summary>
    /// Extra ship purchase action.
    /// </summary>
    public class AdditionalShipPurchaseAction : ShopPurchaseAction
    {
        /// <summary>
        /// Invokes GameManager::shipSpawner::SpawnShip.
        /// </summary>
        public override void PerformAction()
        {
            base.PerformAction();

            GameManager.Instance.shipSpawner.SpawnShip();
        }
    }
}
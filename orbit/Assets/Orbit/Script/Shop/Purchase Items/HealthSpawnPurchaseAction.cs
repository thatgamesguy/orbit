namespace Orbit
{
    /// <summary>
    /// Health spawn purchase action. Increases chance of spawning health.
    /// </summary>
    public class HealthSpawnPurchaseAction : ShopPurchaseAction
    {
        /// <summary>
        /// The amount to increase the health spawn chance.
        /// </summary>
        public int percentageChanceIncrease = 10;

        /// <summary>
        /// The amount to decrease the time between health spawns.
        /// </summary>
        public float spawnTimeDecrease = 0.4f;

        /// <summary>
        /// Increases health spawn chance.
        /// </summary>
        public override void PerformAction()
        {
            base.PerformAction();

            HealthSpawner.Instance.IncreaseSpawnChance(percentageChanceIncrease, spawnTimeDecrease);
        }
    }

}

namespace Orbit
{
    /// <summary>
    /// Collectible spawn chance purchase action.
    /// </summary>
    public class IncreasedCollectiblePurchaseAction : ShopPurchaseAction
    {
        /// <summary>
        /// The amount to increase the percentage chance of spawning collectibles.
        /// </summary>
        public int percentageChanceIncrease = 10;

        /// <summary>
        /// The amount to decrease the time between collectible spawns.
        /// </summary>
        public float collectibleSpawnTimeDecrease = 0.4f;

        /// <summary>
        /// The amount to increase the chance to spawn an additional collectible.
        /// </summary>
        public int additionalPercentageIncrease = 5;

        /// <summary>
        /// Increases the chance to spawn collectibles.
        /// </summary>
        public override void PerformAction()
        {
            base.PerformAction();

            CollectibleManager.instance.IncreaseSpawnChance(percentageChanceIncrease, collectibleSpawnTimeDecrease, additionalPercentageIncrease);
        }

    }
}
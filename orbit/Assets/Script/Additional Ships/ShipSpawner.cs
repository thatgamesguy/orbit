using UnityEngine;

namespace Orbit
{
    /// <summary>
    /// Spawns an additional player ship when a player buys the relevant upgrade.
    /// </summary>
    public class ShipSpawner : MonoBehaviour
    {
        /// <summary>
        /// The player prefab.
        /// </summary>
        public GameObject playerPrefab;

        /// <summary>
        /// The possible spawn locations.
        /// </summary>
        public Transform[] shipSpawnLocations;

        /// <summary>
        /// returns true if the number of spawned ships is less than the length of ShipSpawner::shipSpawnLocations.
        /// </summary>
        public bool canSpawn
        {
            get
            {
                return _currentSpawn < shipSpawnLocations.Length;
            }
        }

        private int _currentSpawn = 0;

        /// <summary>
        /// Spawns ship at next spawn location if spawn location found else returns.
        /// </summary>
        /// <param name="player">The player controller. Spawned ships are added to this controller. </param>
        public void SpawnShip(PlayerController player = null)
        {
            if (shipSpawnLocations.IsNullOrEmpty() || !canSpawn)
            {
                return;
            }

            if (player == null)
            {
                player = GameManager.Instance.playerController;
            }

            int index = _currentSpawn++;

            var shipObj = (GameObject)Instantiate(playerPrefab, shipSpawnLocations[index].position, shipSpawnLocations[index].rotation);

            player.AddAdditionalShip(shipObj.transform);
        }

    }
}

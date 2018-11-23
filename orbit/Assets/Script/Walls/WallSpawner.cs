using UnityEngine;
using System.Collections.Generic;

namespace Orbit
{
    /// <summary>
    /// Responsible for spawning walls.
    /// </summary>
    public class WallSpawner : MonoBehaviour
    {
        /// <summary>
        /// The wall prefab.
        /// </summary>
        public GameObject wallPrefab;

        /// <summary>
        /// The percentage chance to spawn a wall.
        /// </summary>
        [Range(0f, 100f)]
        public float
            chanceToSpawnWall;

        /// <summary>
        /// The percentage chance to spawn an additional wall.
        /// </summary>
        [Range(0f, 100f)]
        public float
            chanceToSpawnAdditional;

        /// <summary>
        /// the percentage chance to increase the chance to spawn an additional wall.
        /// </summary>
        [Range(0f, 100f)]
        public float
            chanceToIncreaseSpawnRate;

        /// <summary>
        /// THe amount to increase the wall spawn chance percentage.
        /// </summary>
        public int spawnChanceIncrease = 5;

        /// <summary>
        /// The maximum percentage.
        /// </summary>
        [Range(0f, 100f)]
        public float
            maxAdditionalSpawnChanceIncrease = 60f;

        /// <summary>
        /// The maximum number of walls.
        /// </summary>
        public int maxNumOfWalls = 2;

        private List<Wall> _spawnedWalls = new List<Wall>();

        /// <summary>
        /// Spawns wallsand increases chance of spawning wall next time.
        /// </summary>
        public void SpawnWalls()
        {
            IncreaseSpawnChance();

            if (Random.value > (chanceToSpawnWall / 100f))
                return;

            int spawned = 0;

            var initialRot = Random.rotation;
            initialRot.x = 0f;
            initialRot.y = 0f;

            float additionalSpawnChance = chanceToSpawnAdditional;

            do
            {
                spawned++;

                if (Random.value > (additionalSpawnChance / 100f))
                {
                    break;
                }

                additionalSpawnChance -= chanceToSpawnAdditional / (maxNumOfWalls - 1);


            } while (spawned < maxNumOfWalls);

            for (int i = 0; i < spawned; i++)
            {
                _spawnedWalls.Add(
                    ((GameObject)Instantiate(wallPrefab, Vector3.zero, initialRot * Quaternion.Euler(0, 0, i * (180f / spawned)))).GetComponent<Wall>()
                );
            }
        }

        /// <summary>
        /// Removes all spawned walls. Invoked at round end.
        /// </summary>
        public void RemoveWalls()
        {
            for (int i = 0; i < _spawnedWalls.Count; i++)
            {
                _spawnedWalls[i].Remove();
            }

            _spawnedWalls.Clear();
        }

        private void IncreaseSpawnChance()
        {
            if (Random.value > (chanceToIncreaseSpawnRate / 100f))
                return;

            chanceToSpawnAdditional += spawnChanceIncrease;

            if (chanceToSpawnAdditional > maxAdditionalSpawnChanceIncrease)
            {
                chanceToSpawnAdditional = maxAdditionalSpawnChanceIncrease;
            }

            if (chanceToSpawnAdditional > 100)
            {
                chanceToSpawnAdditional = 100;
            }
        }
    }
}
using UnityEngine;
using System.Collections.Generic;

namespace Orbit
{
    /// <summary>
    /// Responsible for spawning health objects.
    /// </summary>
    public class HealthSpawner : Singleton<HealthSpawner>
    {
        /// <summary>
        /// The prefab to spawn.
        /// </summary>
        public GameObject healthPrefab;

        /// <summary>
        /// TIme between spawn attempts.
        /// </summary>
        public float timeBetweenSpawns;

        /// <summary>
        /// The chance to spawn health.
        /// </summary>
        [Range(0f, 90f)]
        public float
            percentageChanceOfSpawn = 80f;

        private const float MINIMUM_SPAWN_TIME = 2f;
        private const float MAXIMUM_PERCENTAGE_CHANGE = 85f;

        private List<Health> _spawnedHealth = new List<Health>();
        private float _currentSpawnTime;

        void Update()
        {
            if (GameManager.Instance.gameState != GameState.Play)
            {
                ResetTimer();
                return;
            }

            _currentSpawnTime += Time.deltaTime;

            if (OkToSpawn())
            {
                ResetTimer();
                SpawnHealth();
            }
        }

        /// <summary>
        /// Increases chance to spawn health objects. Invoked when player purchases the relevant upgrade.
        /// </summary>
        /// <param name="percentageIncrease">The amount to increase precentage chance of spawn.</param>
        /// <param name="spawnTimeDecrease">The amount to decrease time between attempted spawns.</param>
        public void IncreaseSpawnChance(int percentageIncrease, float spawnTimeDecrease)
        {
            percentageChanceOfSpawn += percentageIncrease;

            if (percentageChanceOfSpawn > MAXIMUM_PERCENTAGE_CHANGE)
                percentageChanceOfSpawn = MAXIMUM_PERCENTAGE_CHANGE;


            timeBetweenSpawns -= spawnTimeDecrease;

            if (timeBetweenSpawns < MINIMUM_SPAWN_TIME)
            {
                timeBetweenSpawns = MINIMUM_SPAWN_TIME;
            }

        }

        /// <summary>
        /// Removes all spawned health objects.
        /// </summary>
        public void RemoveSpawned()
        {
            for (int i = 0; i < _spawnedHealth.Count; i++)
            {
                _spawnedHealth[i].OnDead();
            }
        }

        private bool OkToSpawn()
        {
            return (_currentSpawnTime >= timeBetweenSpawns) && (Random.value < percentageChanceOfSpawn / 100);
        }

        private void SpawnHealth()
        {
            Vector2 tmpPos = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            var randRot = Random.rotation;
            randRot.x = 0f;
            randRot.y = 0f;

            _spawnedHealth.Add(
                ((GameObject)Instantiate(healthPrefab, tmpPos, randRot)).GetComponent<Health>()
            );
        }

        private void ResetTimer()
        {
            _currentSpawnTime = 0f;
        }
    }
}
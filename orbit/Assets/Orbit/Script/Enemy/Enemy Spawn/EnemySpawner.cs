using UnityEngine;
using System.Collections.Generic;

namespace Orbit
{
    /// <summary>
    /// Resposible for spawning enemies.
    /// </summary>
    public class EnemySpawner : Singleton<EnemySpawner>
    {
        /// <summary>
        /// List of enemy prefabs and associated weights.
        /// </summary>
        public Enemy[] enemies;

        /// <summary>
        /// Percentage chance to decrease enemy spawn time. This is checked everytime an enemy is spawned.
        /// </summary>
        [Range(0f, 100f)]
        public float chanceToDecreaseSpawnTime = 50f;

        private List<EnemyController> _spawnedEnemies = new List<EnemyController>();
        private float _totalWeight;

        private float _enemySpawnTimer;
        private float _enemySpawnTimerMax = 2f;
        private float _origSpawnTimerMax;
        private float _enemySpawnReduction = 0.01f;

        private bool _firstRun = true;

        private const float MaxXY = 1.4f;

        void Awake()
        {
            Initialise();
        }

        private void Initialise()
        {
            foreach (var e in enemies)
            {
                _totalWeight += e.weight;
            }

            _enemySpawnTimer = _enemySpawnTimerMax;
            _origSpawnTimerMax = _enemySpawnTimerMax;

            _firstRun = true;
        }

        /// <summary>
        /// Resets enemy spawn timer, adds a random value to enemySpawnReduction.
        /// </summary>
        public void OnNewRound()
        {
            if (_firstRun)
            {
                _firstRun = false;
                return;
            }

            _enemySpawnTimerMax = _origSpawnTimerMax;

            _enemySpawnReduction += Random.Range(0f, 0.05f);

            if (_enemySpawnReduction > 0.26f)
            {
                _enemySpawnReduction = 0.26f;
            }

            Debug.Log("Reduction: " + _enemySpawnReduction);

        }

        /// <summary>
        /// Spawns enemy if enemySpawnTimer is zero. Reduces spawntimer based on enemySpawnReduction.
        /// </summary>
        public void Execute()
        {
            if (enemies.IsNullOrEmpty())
            {
                return;
            }

            _enemySpawnTimer -= Time.deltaTime;
            if (_enemySpawnTimer <= 0)
            {


                Vector3 enemyPos = new Vector3(Random.Range(-MaxXY, MaxXY), Random.Range(-MaxXY, MaxXY));

                var randRot = Random.rotation;
                randRot.x = 0f;
                randRot.y = 0f;


                int index = GetIndex();

                _spawnedEnemies.Add(
                    ((GameObject)Instantiate(enemies[index].prefab, enemyPos, randRot)).GetComponent<EnemyController>()
                );

                if (Random.value < (chanceToDecreaseSpawnTime / 100f))
                {
                    _enemySpawnTimerMax -= Random.Range(0.005f, _enemySpawnReduction);

                    if (_enemySpawnTimerMax < 0.5f)
                    {
                        _enemySpawnTimerMax = 0.5f;
                    }

                    Debug.Log("Spawn timer: " + _enemySpawnTimerMax);
                }

                _enemySpawnTimer = Random.Range (_enemySpawnTimerMax * 0.8f, _enemySpawnTimerMax * 1.2f);
            }




        }

        /// <summary>
        /// Removes all currently spawned enemies. Called on round end.
        /// </summary>
        public void RemoveAllEnemies()
        {
            for (int i = 0; i < _spawnedEnemies.Count; i++)
            {
                _spawnedEnemies[i].DestroyAtRoundEnd();
            }
            _spawnedEnemies.Clear();
        }

        /// <summary>
        /// Removes enemy from currently spawned list.
        /// </summary>
        /// <param name="enemy">The enemy destroyed.</param>
        public void RegisterEnemyDestroyed(EnemyController enemy)
        {
            _spawnedEnemies.Remove(enemy);
        }

        private int GetIndex()
        {
            if (enemies.Length == 1)
            {
                return 0;
            }

            var randomIndex = -1;
            var random = Random.value * _totalWeight;

            for (int i = 0; i < enemies.Length; ++i)
            {
                random -= enemies[i].weight;

                if (random <= 0f)
                {
                    randomIndex = i;
                    break;
                }
            }

            return randomIndex;
        }
    }
}
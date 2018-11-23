using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Orbit
{
    /// <summary>
    /// Spawns helpful shapes on round start.
    /// </summary>
    public class HelpfulShapeSpawner : MonoBehaviour
    {
        /// <summary>
        /// Chance to spawn helpful shapes on round start.
        /// </summary>
        [Range(0f, 100f)]
        public float
            percentageChanceToSpawn = 60;

        /// <summary>
        /// The shape prefabs to spawn.
        /// </summary>
        public GameObject[] helpfulShapePrefabs;

        private List<HelpfulShape> _spawnedShapes = new List<HelpfulShape>();

        /// <summary>
        /// Spawns shape at random location.
        /// </summary>
        public void SpawnShape()
        {
            if (helpfulShapePrefabs.IsNullOrEmpty() || Random.value > (percentageChanceToSpawn / 100f))
            {
                return;
            }

            var position = new Vector2(Random.Range(-1.2f, 1.2f), Random.Range(-1.2f, 1.2f));

            var objToSpawn = helpfulShapePrefabs[Random.Range(0, helpfulShapePrefabs.Length)];

            var rot = Random.rotation;
            rot.x = 0f;
            rot.y = 0f;

            _spawnedShapes.Add(((GameObject)Instantiate(objToSpawn, position, rot)).GetComponent<HelpfulShape>());

        }

        /// <summary>
        /// Removes all spawned shapes.
        /// </summary>
        public void RemoveSpawnedShapes()
        {
            if (_spawnedShapes.Count == 0)
            {
                return;
            }

            foreach (var shape in _spawnedShapes)
            {
                shape.OnDeath();
            }

            _spawnedShapes.Clear();
        }
    }
}
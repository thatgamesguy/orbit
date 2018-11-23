using UnityEngine;

namespace Orbit
{
    /// <summary>
    /// Container for spawning an enemy. Contains prefab and spawn weight.
    /// </summary>
    [System.Serializable]
    public struct Enemy
    {
        public GameObject prefab;
        public float weight;
    }
}
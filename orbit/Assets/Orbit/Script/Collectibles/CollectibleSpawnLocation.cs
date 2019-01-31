using UnityEngine;

namespace Orbit
{
    /// <summary>
    /// Attached to spawn locations. Responsible for determining if a spawn location is free.
    /// </summary>
    public class CollectibleSpawnLocation : MonoBehaviour
    {
        /// <summary>
        /// returns true if spawn location not currently occupied.
        /// </summary>
        public bool isFree { get; private set; }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isFree = false;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isFree = true;
            }
        }
    }
}

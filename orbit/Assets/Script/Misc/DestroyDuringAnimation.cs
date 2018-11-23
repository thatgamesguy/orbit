using UnityEngine;
using System.Collections;

namespace Orbit
{
    /// <summary>
    /// Provides access to destryong object from animation.
    /// </summary>
    public class DestroyDuringAnimation : MonoBehaviour
    {
        /// <summary>
        /// Destroys gameobject.
        /// </summary>
        public void Execute()
        {
            if (transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }
}
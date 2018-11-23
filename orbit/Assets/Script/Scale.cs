using UnityEngine;
using System.Collections;

namespace Orbit
{
    /// <summary>
    /// Scales object to target scale based on speed.
    /// </summary>
    public class Scale : MonoBehaviour
    {
        /// <summary>
        /// The speed of the scale.
        /// </summary>
        public float speed = 3f;

        /// <summary>
        /// Target scale.
        /// </summary>
        public Vector3 targetScale;

        private bool scaling;
        private float scale;

        void Start()
        {
            scaling = false;
        }

        /// <summary>
        /// Begins the scaling process.
        /// </summary>
        public void Execute()
        {
            scaling = true;
        }

        void Update()
        {
            if (scaling)
            {
                scale += speed * Time.deltaTime;
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scale);

                if (scale > 1f)
                {
                    scaling = false;
                }
            }
        }
    }
}
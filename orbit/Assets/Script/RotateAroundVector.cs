using UnityEngine;

namespace Orbit
{
    /// <summary>
    /// Rotates object around a specified point.
    /// </summary>
    public class RotateAroundVector : MonoBehaviour
    {
        /// <summary>
        /// The target transform.
        /// </summary>
        public Transform rotateTarget;

        private float _rotationIncrement = 0;
        private Vector3 _origPosition;
        private Vector3 _zAxis = new Vector3(0, 0, 1);

        private bool rotating;

        void Start()
        {
            _rotationIncrement = -0.2f;
            _origPosition = transform.position;
            rotating = true;
        }

        /// <summary>
        /// Places object in start position and disables rotation.
        /// </summary>
        public void Stop(bool hideText = true)
        {

           gameObject.SetActive(!hideText);

            rotating = false;
        }

        void FixedUpdate()
        {
            if (rotating)
            {
                _rotationIncrement -= 5 * Time.deltaTime;

                transform.RotateAround(rotateTarget.position, _zAxis, _rotationIncrement);

                if (Vector3.Distance(transform.position, _origPosition) < 15f)
                {
                    _rotationIncrement = -0.2f;
                }
            }
        }
    }
}
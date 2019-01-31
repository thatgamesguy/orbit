using UnityEngine;

namespace Orbit
{
    /// <summary>
    /// Performs camera shake on damage.
    /// </summary>
    public class CameraShake : MonoBehaviour
    {
        /// <summary>
        /// Main camera.
        /// </summary>
        public Transform camTransform;
      
        /// <summary>
        /// How long camera should shake for.
        /// </summary>
        public float shake = 0f;
      
        /// <summary>
        /// The amount of shake effect to apply.
        /// </summary>
        public float shakeAmount = 0.7f;

        /// <summary>
        /// The amount to decrease shake over time.
        /// </summary>
        public float decreaseFactor = 1.0f;

        private Vector3 _originalPos;

        void Awake()
        {
            if (camTransform == null)
            {
                camTransform = GetComponent<Transform>();
            }
        }

        void OnEnable()
        {
            _originalPos = camTransform.localPosition;
        }

        void Update()
        {
            if (shake > 0)
            {
                camTransform.localPosition = _originalPos + Random.insideUnitSphere * shakeAmount;
                shake -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shake = 0f;
                camTransform.localPosition = _originalPos;
            }
        }
    }
}
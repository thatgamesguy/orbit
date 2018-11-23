using UnityEngine;

namespace Orbit
{
    /// <summary>
    /// Animates orbit object on damage.
    /// </summary>
    public class OrbitDamageAnimation : MonoBehaviour
    {
        /// <summary>
        /// The orbit animator component.
        /// </summary>
        public Animator animator;

        /// <summary>
        /// The colour to change orbit to on hit.
        /// </summary>
        public Color hitColour;

        private Renderer _renderer;
        private float? t;
        private Color _targetColour;

        void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        /// <summary>
        /// Begins orbit animation.
        /// </summary>
        public void PlayAnimation()
        {
            _targetColour = _renderer.material.color;

            _renderer.material.color = hitColour;
        }

        void Update()
        {
            if (t.HasValue)
            {
                Debug.Log("Here");
                _renderer.material.color = Color.Lerp(hitColour, _targetColour, t.Value);
                if (t < 1)
                { 
                    t += Time.deltaTime / 0.3f;
                }
                else
                {
                    t = null;
                }
            }
        }
    }
}

using UnityEngine;

namespace Orbit
{
    /// <summary>
    /// Attached to particle damage controllers. Animates fade out.
    /// </summary>
    public class ParticleDamageController : MonoBehaviour
    {
        private Renderer _renderer;

        void Awake()
        {
            if (!_renderer)
            {
                _renderer = GetComponent<Renderer>();
            }
        }

        void Start()
        {
            this.transform.localScale = new Vector3(.4f, .4f);
        }

        void Update()
        {
            float scale = this.transform.localScale.x + Time.deltaTime;

            this.transform.localScale = new Vector3(scale, scale);
            Color color = _renderer.material.color;
            color.a -= Time.deltaTime * .9f;

            _renderer.material.color = color;

            if (color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

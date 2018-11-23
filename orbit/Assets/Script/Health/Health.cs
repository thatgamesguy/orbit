using UnityEngine;

namespace Orbit
{
    /// <summary>
    /// Attached to every health bubble spawned.
    /// </summary>
    public class Health : MonoBehaviour
    {
        /// <summary>
        /// Current hit points.
        /// </summary>
        public int points { get; set; }

        /// <summary>
        /// Animator, used to play animation on health collected.
        /// </summary>
        public Animator animator;

        private Collider2D _collider2D;

        void Awake()
        {
            _collider2D = GetComponent<Collider2D>();

            float s = Random.Range(0.1f, 0.2f);

            transform.localScale = new Vector2(s, s);

            points = (int)((s - 0.09f) * 150);
        }

        /// <summary>
        /// Calls animation.
        /// </summary>
        public void OnDead()
        {
            if (animator)
            {
                animator.SetTrigger("healthDie");
            }

            if (_collider2D)
                _collider2D.enabled = false;
        }

        /// <summary>
        /// Destroys gameobject.
        /// </summary>
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
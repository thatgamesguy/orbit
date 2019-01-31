using UnityEngine;

namespace Orbit
{
    /// <summary>
    /// Responsible for removing object when it reaches orbit circumference.
    /// </summary>
    [RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class DeathObject : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody2D _rigidBody2D;
        private SpriteRenderer _renderer;

        private static readonly int ON_DEATH_HASH = Animator.StringToHash("OnDeath");

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {

            if (Vector2.Distance((Vector2)transform.position, Vector2.zero) > 2.5f)
            {
                GameManager.Instance.UpdateEnergy(-1);
                GameManager.Instance.addParticle(transform.position.x, transform.position.y);
                RemoveObject();
            }
        }

        /// <summary>
        /// Destroys gameobject.
        /// </summary>
        public void RemoveObject()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// Updates sprite colour.
        /// </summary>
        /// <param name="colour">The new colour.</param>
        public void ChangeSpriteColour(Color colour)
        {
            _renderer.color = colour;
        }

        /// <summary>
        /// Add force to attached Rigidbody2D.
        /// </summary>
        /// <param name="force"></param>
        public void AddForce(Vector2 force)
        {
            _rigidBody2D.AddForce(force);
        }

        /// <summary>
        /// Invoked when object killed.
        /// </summary>
        public void OnDead()
        {
            gameObject.tag = "Untagged";
            _animator.SetTrigger(ON_DEATH_HASH);
        }

    }
}
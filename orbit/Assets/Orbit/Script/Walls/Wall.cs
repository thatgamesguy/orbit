using UnityEngine;

namespace Orbit
{
    /// <summary>
    /// Represents walls spawned at round begin.
    /// </summary>
    public class Wall : MonoBehaviour
    {
        private Animator _animator;

        void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Begins remove wall animation.
        /// </summary>
        public void Remove()
        {
            _animator.SetTrigger("wallRemove");
        }

        /// <summary>
        /// Destroys gameobject.
        /// </summary>
        public void ExecuteDestroy()
        {
            Destroy(gameObject);
        }
    }
}
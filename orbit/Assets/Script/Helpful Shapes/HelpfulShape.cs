using UnityEngine;
using System.Collections;

namespace Orbit
{
    /// <summary>
    /// Attached to every helpful shape spawned. The helpful shapes are the green bullet spawners the produce extra projectiles when shot.
    /// </summary>
    public class HelpfulShape : MonoBehaviour
    {
        /// <summary>
        /// The speed additional projectiles are fired.
        /// </summary>
        public float bulletSpeed;

        /// <summary>
        /// The bullet prefab to spawn.
        /// </summary>
        public GameObject bulletPrefab;

        /// <summary>
        /// The associated projectile spawn locations.
        /// </summary>
        public Transform[] bulltSpawnLocations;

        private static readonly int SHOOT_HASH = Animator.StringToHash("shoot");
        private static readonly int DEATH_HASH = Animator.StringToHash("remove");

        private Vector2 _scale;
        private Animator _animator;

        private bool _shooting;

        void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Begins shooting animation.
        /// </summary>
        /// <param name="scale"></param>
        public void InitialiseShoot(Vector2 scale)
        {
            if (_shooting)
            {
                return;
            }

            _shooting = true;

            _scale = scale;

            _animator.SetTrigger(SHOOT_HASH);
        }

        /// <summary>
        /// Instantiates projectiles at the specified location.
        /// </summary>
        public void Shoot()
        {
            for (int i = 0; i < bulltSpawnLocations.Length; i++)
            {
                PlayerBulletController bulletController = ((GameObject)Instantiate(bulletPrefab,
                                                                                    bulltSpawnLocations[i].position,
                                                                                    bulltSpawnLocations[i].rotation)).GetComponent<PlayerBulletController>();
                bulletController.speed = bulletSpeed * 6f;
                bulletController.helpfulShape = this;
                bulletController.transform.localScale = _scale;
            }

        }

        /// <summary>
        /// Called by animator. Disables shooting.
        /// </summary>
        public void FinishedShooting()
        {
            _shooting = false;
        }

        /// <summary>
        /// Starts relevant animation.
        /// </summary>
        public void OnDeath()
        {
            _animator.SetTrigger(DEATH_HASH);
        }


        /// <summary>
        /// Destroys gameobject.
        /// </summary>
        public void Remove()
        {
            Destroy(gameObject);
        }
    }
}
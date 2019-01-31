using UnityEngine;
using System.Collections;

namespace Orbit
{
    /// <summary>
    /// Attached to every player bullet. Responsible for updating projectiles location, adding reflections from obstacles, and handling collisions with
    /// enemy and health objects.
    /// </summary>
    public class PlayerBulletController : MonoBehaviour
    {
        /// <summary>
        /// Projectile speed.
        /// </summary>
        public float speed = .2f;

        /// <summary>
        /// The amount of damage applied to enemies.
        /// </summary>
        public int power = 1;

        /// <summary>
        /// The shape that spawned this projectile (null if spawned by player).
        /// </summary>
        public HelpfulShape helpfulShape;

        /// <summary>
        /// The layerMask of the relfection obstacle. Used for raycasting.
        /// </summary>
        public LayerMask reflectMask;

        private Collider2D _cahchedHit;

        void Update()
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);


            if (Vector2.Distance((Vector2)transform.position, Vector2.zero) > 2.5f)
            {

                ScoreController.Instance.ResetCombo();
                Destroy(gameObject);
            }
        }

        private void Reflect()
        {
            var hit = Physics2D.Raycast(transform.position, transform.up, 0.2f, reflectMask);

            if (hit.collider != null)
            {

                var rayDir = Vector2.Reflect((hit.point - (Vector2)transform.position).normalized, hit.normal);

                float angle = Mathf.Atan2(-rayDir.x, rayDir.y) * Mathf.Rad2Deg;
                var targetRot = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = targetRot;

            }

        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            if (_cahchedHit == coll)
            {
                return;
            }

            _cahchedHit = coll;

            if (coll.gameObject.name == "enemy")
            {
                ScoreController.Instance.UpdateScore(2);
                coll.gameObject.GetComponent<EnemyController>().Damage(power);
                Destroy(gameObject);
            }
            else if (coll.CompareTag("bulletSpawner"))
            {
                var shape = coll.GetComponent<HelpfulShape>();
                if (this.helpfulShape == null || (this.helpfulShape != shape))
                {
                    shape.InitialiseShoot(transform.localScale);
                    Destroy(gameObject);
                }
            }
            else if (coll.CompareTag("deathObject"))
            {
                ScoreController.Instance.UpdateScore(1);
                coll.gameObject.GetComponent<DeathObject>().OnDead();
            }
            else if (coll.CompareTag("health"))
            {
                var health = coll.GetComponent<Health>();
                GameManager.Instance.UpdateEnergy(health.points);
                health.OnDead();
                Destroy(gameObject);
            }
            else if (coll.CompareTag("wall"))
            {
                Reflect();
            }
        }
    }
}
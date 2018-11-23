using UnityEngine;

namespace Orbit
{
    /// <summary>
    /// Attached to each enemy projectile. Resposible for updating position each time step, and removing objects when it reaches obrit.
    /// </summary>
    public class EnemyBulletController : MonoBehaviour
    {
        /// <summary>
        /// Movement speed per time step.
        /// </summary>
        public float speed = .2f;

        /// <summary>
        /// The projectiles owner.
        /// </summary>
        public EnemyController enemy;

        /// <summary>
        /// Holds reference to previous EnemyBulletController.
        /// </summary>
        public EnemyBulletController prev;

        /// <summary>
        /// Holds reference to next EnemyBulletController.
        /// </summary>
        public EnemyBulletController next;


        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, new Vector3(0, 0, transform.position.z)) > 2.5f)
            {
                GameManager.Instance.UpdateEnergy(-1);
                GameManager.Instance.addParticle(transform.position.x, transform.position.y);
                Destroy(gameObject);

            }
        }

        void OnTriggerEnter2D(Collider2D coll)
        {

            if (coll.gameObject.name == "player")
            {
                GameManager.Instance.UpdateEnergy(-10);
                GameManager.Instance.addParticle(transform.position.x, transform.position.y);
                enemy.RemoveBullet(this);
                Destroy(gameObject);
            }
        }
    }
}
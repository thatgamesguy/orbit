using UnityEngine;

namespace Orbit
{
	/// <summary>
	/// Controls enemy behaviour once spawned. Resposible for shooting projectiles, destroying gameobject and damaging orbit.
	/// </summary>
	public class EnemyController : MonoBehaviour
	{
		
		/// <summary>
		/// The enemies sprite container.
		/// </summary>
		public GameObject sprite;

		/// <summary>
		/// The enemies sprite.
		/// </summary>
		public SpriteRenderer spriteRenderer;

		/// <summary>
		/// The animator.
		/// </summary>
		public Animator spriteAnimator;

		/// <summary>
		/// time until next projectile is fired.
		/// </summary>
		public float timerShoot = 1f;

		/// <summary>
		/// The maximum time between projectiles fired.
		/// </summary>
		public float maxTimerShoot = 1f;

		/// <summary>
		/// the bullet prefab.
		/// </summary>
		public GameObject bullet;

		/// <summary>
		/// Reference to current AI state.
		/// </summary>
		public EnemyState state;

		/// <summary>
		/// Enum for possible states.
		/// </summary>
		public enum EnemyState
		{
			Spawned,
			Alive,
			Dead
		}

		/// <summary>
		/// Reference to first bullet fired.
		/// </summary>
		public EnemyBulletController firstBullet;

		/// <summary>
		/// Reference to last bullet fired.
		/// </summary>
		public EnemyBulletController lastBullet;

		/// <summary>
		/// Current health.
		/// </summary>
		public int hp = 1;

		/// <summary>
		/// Object to spawn on death.
		/// </summary>
		public GameObject deathObject;

		private bool spawnDeathObjectsOnHit = false;
		private Collider2D _collider2D;
		private Vector3 _dir;
		private Quaternion _bulletAngle;

		void Start ()
		{
			_collider2D = GetComponent<Collider2D> ();
			_collider2D.enabled = false;

			if (!spriteRenderer) {
				spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
			}

			if (!spriteAnimator)
				spriteAnimator = sprite.GetComponentInChildren<Animator> ();

			state = EnemyState.Spawned;
			gameObject.name = "enemy";

			var heading = Vector2.zero - (Vector2)transform.position;
			var dist = heading.magnitude;
			_dir = -(heading / dist);
			float angle = Mathf.Atan2 (_dir.y, _dir.x) * Mathf.Rad2Deg + -90;
			_bulletAngle = Quaternion.AngleAxis (angle, Vector3.forward);
		}

		// Update is called once per frame
		void Update ()
		{
			if (state != EnemyState.Alive)
				return;

			timerShoot -= Time.deltaTime;
			if (timerShoot <= 0) {
				spriteAnimator.SetBool ("Shoot", true);
			}

			if (Vector3.Distance (transform.position, new Vector3 (0, 0, transform.position.z)) > 2.5f) {
				GameManager.Instance.UpdateEnergy (-5);
				Die ();
			}
		}

		/// <summary>
		/// Called when spawn has finished. Changes state to EnemyController::EnemyState::Alive.
		/// </summary>
		public void SpawnFinished ()
		{
			state = EnemyState.Alive;
			_collider2D.enabled = true;
		}

		/// <summary>
		/// Spawns projectile.
		/// </summary>
		public void ShootBullet ()
		{
			timerShoot = maxTimerShoot;

			EnemyBulletController enemyBulletController = ((GameObject)Instantiate (bullet,
				                                                       transform.position + _dir * 0.2f, _bulletAngle)).GetComponent<EnemyBulletController> ();
			enemyBulletController.enemy = this;

			if (!firstBullet) {
				firstBullet = enemyBulletController;
			} else if (!lastBullet) {
				lastBullet = enemyBulletController;
				firstBullet.next = lastBullet;
				lastBullet.prev = firstBullet;
			} else {
				lastBullet.next = enemyBulletController;
				enemyBulletController.prev = lastBullet;
				lastBullet = enemyBulletController;
			}

			spriteAnimator.SetBool ("Shoot", false);
		}

		/// <summary>
		/// Removes bullet from scene.
		/// </summary>
		/// <param name="bullet">The bullet to remove.</param>
		public void RemoveBullet (EnemyBulletController bullet)
		{
			if (bullet.prev) {
				if (bullet.next) {
					bullet.prev.next = bullet.next;
					bullet.next.prev = bullet.prev;
				} else {
					bullet.prev.next = null;
					lastBullet = bullet.prev;
					if (firstBullet == lastBullet) {
						lastBullet = null;
					}
				}
			} else {
				if (bullet.next) {
					bullet.next.prev = null;
					firstBullet = bullet.next;
					if (firstBullet == lastBullet) {
						lastBullet = null;
					}
				} else {
					firstBullet = null;
					lastBullet = null;
				}
			}
		}

		/// <summary>
		/// Removes all bullets from scene.
		/// </summary>
		public void RemoveAllBullets ()
		{
			while (firstBullet) {
				lastBullet = firstBullet.next;
				Destroy (firstBullet.gameObject);
				firstBullet = lastBullet;
			}
			firstBullet = null;
			lastBullet = null;
		}

		/// <summary>
		/// Applies damage to enemy. Removes from scene when health reaches zero.
		/// </summary>
		/// <param name="val"></param>
		public void Damage (int val)
		{
			hp -= val;
			if (hp <= 0) {
				Die ();
			} else {
                SoundEffectPlayer.Instance.PlayClip("enemy_hit");


                UpdateStateDamage ();

				if (spawnDeathObjectsOnHit) {
					SpawnDeathObjects (25f);
				}
			}
		}

		/// <summary>
		/// Destroys game object.
		/// </summary>
		public void DestroyObject ()
		{
			Destroy (gameObject);
		}

		/// <summary>
		/// Destroys object at round end.
		/// </summary>
		public void DestroyAtRoundEnd ()
		{
			if (state == EnemyState.Dead)
				return;

			state = EnemyState.Dead;

			spriteAnimator.Play ("enemyDie");
			if (_collider2D)
				_collider2D.enabled = false;
		}

		private void UpdateStateDamage ()
		{
			var s = transform.localScale;
			transform.localScale = new Vector3 (s.x - 0.3f, s.y - 0.3f, 1f);
		}

		private void Die ()
		{
			if (state == EnemyState.Dead)
				return;

			SoundEffectPlayer.Instance.PlayClip ("enemy_death");

			state = EnemyState.Dead;
			spriteAnimator.Play ("enemyDie");
			_collider2D.enabled = false;
			EnemySpawner.Instance.RegisterEnemyDestroyed (this);
			SpawnDeathObjects ();
		}

        

		private void SpawnDeathObjects (float force = 8f)
		{
			if (deathObject == null)
				return;

			int numToSpawn = Random.Range (4, 6);

			for (int i = 0; i < numToSpawn; i++) {
				float scale = Random.Range (0.15f, 0.4f);

				Vector2 offsetPosition = ((Vector2)transform.position).WithRandomOffset (-0.13f, 0.13f);

				var deathObj = ((GameObject)Instantiate (deathObject, offsetPosition, Quaternion.identity)).GetComponent<DeathObject> ();



				deathObj.transform.localScale = new Vector2 (scale, scale);
				deathObj.ChangeSpriteColour (spriteRenderer.color);

				var heading = offsetPosition - (Vector2)transform.position;
				var distance = heading.magnitude;
				var dir = heading / distance;
				deathObj.AddForce (dir * force);


			}
		}
	}
}
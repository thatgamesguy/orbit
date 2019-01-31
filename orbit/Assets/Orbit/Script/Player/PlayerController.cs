using UnityEngine;
using System.Collections.Generic;

namespace Orbit
{
	/// <summary>
	/// Controls the player. Includes rotation, shooting, and movement.
	/// </summary>
	public class PlayerController : MonoBehaviour
	{
	
		/// <summary>
		/// Movement speed.
		/// </summary>
		public float speed = 70f;

		/// <summary>
		/// The shot locations. New locations are added based on purchased upgrades.
		/// </summary>
		public Transform[] additionalShotLocations;

		/// <summary>
		/// Reference to the current players speed.
		/// </summary>
		public float currentSpeed;

		/// <summary>
		/// Delay betweens projectile shots.
		/// </summary>
		static public float timerShoot = 1f;

		/// <summary>
		/// The maximum delay between projeciles.
		/// </summary>
		public float maxTimerShoot = 1f;

		/// <summary>
		/// The prefab for the players bullet.
		/// </summary>
		public GameObject playerBullet;

		/// <summary>
		/// Reference to the ship gameobject.
		/// </summary>
		public GameObject ship;

		/// <summary>
		/// The ships animator.
		/// </summary>
		public Animator shipAnimator;

		/// <summary>
		/// The speed of the projectile.
		/// </summary>
		public float bulletSpeed = 1f;

		/// <summary>
		/// Returns true if an additional projectile location can be added.
		/// </summary>
		public bool OkToAttachAdditionalShot {
			get {
				return _currentShotsActivated < additionalShotLocations.Length - 1;
			}
		}

		private int _currentShotsActivated = -1;
		private List<Transform> _additionalShips = new List<Transform> ();
		private bool _spawned;

		void Start ()
		{
			currentSpeed = speed;
		}

		void Update ()
		{
			if (_spawned) {
				transform.RotateAround (Vector3.zero, Vector3.forward, speed * Time.deltaTime);
				timerShoot -= Time.deltaTime;
				if (timerShoot <= 0) {
					timerShoot = maxTimerShoot;
				}
				speed += (currentSpeed - speed) * Time.deltaTime * 1f;
			}



		}

		/// <summary>
		/// Begins spawn animation.
		/// </summary>
		public void Spawn ()
		{
			shipAnimator.SetTrigger ("spawn");
		}

		/// <summary>
		/// Invoked by the animator to enable movement.
		/// </summary>
		public void FinishedSpawning ()
		{
			_spawned = true;
		}

		/// <summary>
		/// Adds additional shot location. Invoked when the player purchases additional shots.
		/// </summary>
		public void AddAdditionalShot ()
		{
			_currentShotsActivated += 2;

			if (_currentShotsActivated > additionalShotLocations.Length - 1) {
				_currentShotsActivated = additionalShotLocations.Length - 1;
			}
		}

		/// <summary>
		/// Adds additional ships. Inoked when the player purchases additional ships.
		/// </summary>
		/// <param name="ship"></param>
		public void AddAdditionalShip (Transform ship)
		{
			ship.SetParent (transform);
			_additionalShips.Add (ship);
		}

		/// <summary>
		/// Changes direction and fires projecile.
		/// </summary>
		/// <param name="shootPower">The speed of the projectile.</param>
		public void ChangeDirection (float shootPower)
		{
			if (!_spawned)
				return;

			if (shootPower < 0.15f) {
				shootPower = 0.15f;
			}

			ShootBullet (shootPower);
			currentSpeed *= -1;
			speed *= -1;
			speed = currentSpeed < 0 ? speed - 20f : speed + 20f;
		}

		/// <summary>
		/// Spawns bullet.
		/// </summary>
		/// <param name="shootPower">The movement speed of the projectile.</param>
		public void ShootBullet (float shootPower)
		{
			if (!_spawned)
				return;

			SoundEffectPlayer.Instance.PlayClip ("player_shoot");

			int bulletsFired = 1;
			SpawnBullet (shootPower, transform);

			if (_currentShotsActivated >= 0) {
				for (int i = 0; i < (_currentShotsActivated + 1); i++) {
					SpawnBullet (shootPower, additionalShotLocations [i]);
					bulletsFired++;
				}
			}

			foreach (var t in _additionalShips) {
				SpawnBullet (shootPower, t);
				bulletsFired++;
			}
		}

		private void SpawnBullet (float shootPower, Transform t)
		{
			PlayerBulletController bulletController = ((GameObject)Instantiate (playerBullet, t.position, t.rotation)).GetComponent<PlayerBulletController> ();

			bulletController.speed = bulletSpeed + shootPower * 8f;

			//float sFactor = (shootPower < 0.5f) ? 0.5f : shootPower;

			//bulletController.gameObject.transform.localScale = new Vector3 (1.8f * sFactor, 2.3f * sFactor, 1f);
		}

		private Quaternion GetBulletRotation (Transform ship)
		{
			Quaternion tmpRot = ship.rotation;
			return tmpRot;
		}

		private Vector2 GetBulletLocation (Transform ship)
		{
			return ship.position;
		}
	}
}
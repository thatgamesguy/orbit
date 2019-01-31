using UnityEngine;

namespace Orbit
{
	/// <summary>
	/// Attached to collectibles. Handles collisions with player and adding to CollectibleManager.
	/// </summary>
	public class Collectible : MonoBehaviour
	{
		/// <summary>
		/// The points added on collection.
		/// </summary>
		public int increaseAmount = 1;

		private Collider2D _collider;
		private Animator _animator;

		void Awake ()
		{
			_collider = GetComponent<Collider2D> ();
			_animator = GetComponent<Animator> ();
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.CompareTag ("Player")) {
				SoundEffectPlayer.Instance.PlayClip ("collectible");
				CollectibleManager.instance.Collected (this);
				_collider.enabled = false;
				_animator.SetTrigger ("collected");
			}
		}

		/// <summary>
		/// Destroys object. Called by animation when completed.
		/// </summary>
		public void Destroy ()
		{
			Destroy (gameObject);
		}
	}
}

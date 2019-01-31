using UnityEngine;

namespace Orbit
{
	/// <summary>
	/// Responsible for the main menu. Includes pausing and resuming, and beginning the game.
	/// </summary>
	public class MainMenu : MonoBehaviour
	{
		/// <summary>
		/// Reference to the container object for the in game UI.
		/// </summary>
		public GameObject inGameUI;

		/// <summary>
		/// Reference to the container object for the menu UI.
		/// </summary>
		public GameObject menuUI;

		void Awake ()
		{
			inGameUI.SetActive (false);
		}

		/// <summary>
		/// Calls GameManager::Resume or GameManager::BeginGame depending on whether the game has already begun.
		/// Activates in game UI and deactives menu UI.
		/// </summary>
		public void StartGame ()
		{


            inGameUI.SetActive (true);
			if (GameManager.Instance.gameState == GameState.Pause) {
                SoundEffectPlayer.Instance.PlayClip("pause_pressed");
                GameManager.Instance.Resume ();
			} else {
				GameManager.Instance.BeginGame ();
			}
			menuUI.SetActive (false);
		}

		/// <summary>
		/// Enables main menu UI, Invokes GameManager::Pause and sets main menu UI active.
		/// </summary>
		public void Pause ()
		{
			SoundEffectPlayer.Instance.PlayClip ("pause_pressed");
			inGameUI.SetActive (false);
			GameManager.Instance.Pause ();

			menuUI.SetActive (true);
		}
	}
}
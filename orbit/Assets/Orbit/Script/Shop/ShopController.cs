using UnityEngine;
using UnityEngine.UI;

namespace Orbit
{
	/// <summary>
	/// Controls opening and closing of shops, and purchase attempts.
	/// </summary>
	public class ShopController : MonoBehaviour
	{
		/// <summary>
		/// The container for the shop UI.
		/// </summary>
		public GameObject buyPanelBase;

		/// <summary>
		/// The canvas for the shop.
		/// </summary>
		public Canvas buyPanel;

		/// <summary>
		/// The text that shows the current amount earned.
		/// </summary>
		public Text moneyText;

		private bool _shopOpened;

		void Awake ()
		{ 
			buyPanel.enabled = false;
		}

		/// <summary>
		/// Shows shop UI.
		/// </summary>
		public void OpenShop ()
		{
			if (_shopOpened)
				return;


            BackgroundAudio.Instance.FadeOut(0.45f);

            if (GameManager.Instance.gameState == GameState.Pause) {
				buyPanel.enabled = true;
				moneyText.text = CollectibleManager.instance.collected.ToString ();
			}

			_shopOpened = true;

		}

		/// <summary>
		/// Closes shop UI.
		/// </summary>
		public void CloseShop ()
		{
			if (!_shopOpened)
				return;


            BackgroundAudio.Instance.FadeIn(0.45f);

            buyPanel.enabled = false;

			GameManager.Instance.gameState = GameState.Play;

			_shopOpened = false;
		}

		/// <summary>
		/// Attempts purchase.
		/// </summary>
		/// <param name="purchaseAction">The purchase to attempt.</param>
		public void AttemptPurchase (ShopPurchaseAction purchaseAction)
		{
			if (!purchaseAction.IsActionable ())
				return;

			SoundEffectPlayer.Instance.PlayClip ("upgrade");

			purchaseAction.PerformAction ();

			moneyText.text = CollectibleManager.instance.collected.ToString ();

		}

	}
}
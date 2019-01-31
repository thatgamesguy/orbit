using UnityEngine;
using UnityEngine.UI;

namespace Orbit
{
    /// <summary>
    /// ATtached to UI audio toggle. Toggles background audio on/off.
    /// </summary>
    public class BackgroundAudioControl : MonoBehaviour
    {
        /// <summary>
        /// Reference to BackgroundAudio. Used to toggle audio on/off.
        /// </summary>
        public BackgroundAudio backgroundAudio;

        /// <summary>
        /// The sprite to display when audio is playing.
        /// </summary>
        public Sprite audioPlayingSprite;

        /// <summary>
        /// The sprite to show when audio is not playing.
        /// </summary>
        public Sprite audioStoppedSprite;

        /// <summary>
        /// Reference to the button image.
        /// </summary>
        public Image buttonImage;

        /// <summary>
        /// Toggles audio on/off.
        /// </summary>
        public void AudioToggle()
        {
            if (backgroundAudio.isPlaying)
            {
                backgroundAudio.shouldPlay = false;
                SoundEffectPlayer.Instance.shouldPlay = false;
                buttonImage.sprite = audioStoppedSprite;
            }
            else
            {
                backgroundAudio.shouldPlay = true;
                SoundEffectPlayer.Instance.shouldPlay = true;
                buttonImage.sprite = audioPlayingSprite;
            }
        }


    }
}
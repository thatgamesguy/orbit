using UnityEngine;
using System.Collections;

namespace Orbit
{
	/// <summary>
	/// Responsible for playing 2D audio effects.
	/// </summary>
	[RequireComponent (typeof(AudioSource))]
	public class SoundEffectPlayer : Singleton<SoundEffectPlayer>
	{
		public bool shouldPlay = true;

		private AudioSource _audioSource;

        private AssetLibrary<AudioClip> _audioEffectLibrary;

        void Awake ()
		{
			_audioSource = GetComponent<AudioSource> ();
		}

        void Start()
        {
            _audioEffectLibrary = new AssetLibrary<AudioClip>("sound_effects_list");
        }

		public void PlayClip (string name)
		{
			if (shouldPlay) {

                AudioClip clip = _audioEffectLibrary.GetAssetFromName(name);

                if (clip != null)
                {
                    _audioSource.PlayOneShot(clip);
                }
            
			}
		}
	}
}
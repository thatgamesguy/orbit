using UnityEngine;

namespace Orbit
{
    /// <summary>
    /// Responsible for playing background music.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundAudio : Singleton<BackgroundAudio>
    {
        /// <summary>
        /// Returns true if this instance if currently playing.
        /// </summary>
        public bool isPlaying { get { return _audioSource.isPlaying; } }

        /// <summary>
        /// Indicates whether this instance should play audio when requested.
        /// </summary>
        public bool shouldPlay
        {
            get
            {
                return _shouldPlay;
            }
            set
            {
                _shouldPlay = value;

                if (_shouldPlay)
                {
                    if (Time.timeScale == 0f)
                    {
                        _audioSource.volume = _defaultAudioVol;
                        _audioSource.Play();
                    }
                    else
                    {
                        FadeIn();
                    }
             
                }
                else
                {
                    if (Time.timeScale == 0f)
                    {
                        _audioSource.Stop();
                    }
                    else
                    {
                        FadeOut();
                    }
             
                }
            }
        }

        private AudioSource _audioSource;
        private float _defaultAudioVol;
        private bool _fadeIn, _fadeOut;
        private bool _shouldPlay = true;
        private float _fadeOutTarget = 0.05f;

        void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;

            _defaultAudioVol = _audioSource.volume;
        }

        /// <summary>
        /// Play the specified audio clip on a loop.
        /// </summary>
        /// <param name="clip">The audioclip to play</param>
        public void Play(AudioClip clip)
        {
            _audioSource.clip = clip;

            if (shouldPlay)
            {
                FadeIn();
            }
         
        }

        /// <summary>
        /// Stop current audio instance playing.
        /// </summary>
        public void Stop()
        {
            _audioSource.Stop();
        }

        /// <summary>
        /// Fade out current audio clip.
        /// </summary>
        public void FadeOut(float target = 0.05f)
        {
            _fadeOutTarget = target;
            _fadeOut = true;

        }

        /// <summary>
        /// Fade in current audio clip.
        /// </summary>
        public void FadeIn(float initialVolume = 0f)
        {
            _audioSource.volume = initialVolume;

            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
            _fadeIn = true;
        }

        void Update()
        {
            if (_fadeIn)
            {

                _audioSource.volume +=1.2f * Time.deltaTime;

                if (_audioSource.volume >= _defaultAudioVol)
                {
                    _audioSource.volume = _defaultAudioVol;
                    _fadeIn = false;
                }

            }
            else if (_fadeOut)
            {
                _audioSource.volume -= .7f * Time.deltaTime;

                if (_audioSource.volume <= _fadeOutTarget)
                {
                    _fadeOut = false;

                    _audioSource.volume = _fadeOutTarget;

                    if (_fadeOutTarget <= 0.05f)
                    {
                        _audioSource.Stop();
                    }
                }
            }
        }

        private void ResetVolume()
        {
            _audioSource.volume = _defaultAudioVol;
        }

    }
}
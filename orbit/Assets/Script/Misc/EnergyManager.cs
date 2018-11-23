using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Orbit
{
    /// <summary>
    /// Animates updating energy text on change.
    /// </summary>
    public class EnergyManager : MonoBehaviour
    {
        /// <summary>
        /// The energy animator.
        /// </summary>
        public Animator energyAnimator;

        private static readonly int ENERGY_CHANGED_TRIGGER = Animator.StringToHash("energyChanged");

        private bool _animationPlaying;

        /// <summary>
        /// Starts energy animation.
        /// </summary>
        public void PlayEnergyAnimation()
        {
            if (!_animationPlaying)
            {
                energyAnimator.SetTrigger(ENERGY_CHANGED_TRIGGER);
                _animationPlaying = true;
            }
        }

        /// <summary>
        /// Resets ability to play animation.
        /// </summary>
        public void AnimationFinished()
        {
            _animationPlaying = false;
        }
    }




}

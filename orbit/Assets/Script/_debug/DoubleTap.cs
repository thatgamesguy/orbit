using UnityEngine;

namespace Orbit
{
    /// <summary>
    /// Test class: when added to the scene the user can double tap to initiate a new wave.
    /// </summary>
    public class DoubleTap : MonoBehaviour
    {
        public GameManager manager;

        private float _doubleTapTimeD;
        void Update()
        {
            bool doubleTapD = false;

            if (Input.GetMouseButton(0))
            {
                if (Time.time < _doubleTapTimeD + .3f)
                {
                    doubleTapD = true;
                }
                _doubleTapTimeD = Time.time;
            }

            if (doubleTapD && manager.gameState == GameState.Play)
            {
                manager.OnWaveComplete();
            }
        }
    }
}

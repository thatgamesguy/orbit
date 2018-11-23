using UnityEngine;
using System.Collections;

namespace Orbit
{
    /// <summary>
    /// Centres health text at centre of orbit UI.
    /// </summary>
    public class CentreUITextToOrbit : MonoBehaviour
    {
        /// <summary>
        /// Orbit object to centre on.
        /// </summary>
        public Transform orbitObj;

        void Start()
        {
            RectTransform CanvasRect = GetComponent<RectTransform>();

            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(orbitObj.transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(
                ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

            CanvasRect.anchoredPosition = WorldObject_ScreenPosition;
        }

    }
}
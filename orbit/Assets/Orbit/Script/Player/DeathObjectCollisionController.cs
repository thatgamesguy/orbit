using UnityEngine;

namespace Orbit
{
    /// <summary>
    /// Responsible for handling collisons between deathobject and player line.
    /// </summary>
    public class DeathObjectCollisionController : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("deathObject"))
            {
                ScoreController.Instance.UpdateScore(1);
                other.GetComponent<DeathObject>().OnDead();
            }
        }
    }
}
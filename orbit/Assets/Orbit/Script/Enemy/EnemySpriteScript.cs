using UnityEngine;

namespace Orbit
{
    /// <summary>
    /// Called by enemy animation controller. 
    /// </summary>
    public class EnemySpriteScript : MonoBehaviour
    {
        EnemyController parentController;

        void Start()
        {
            parentController = transform.parent.GetComponent<EnemyController>();
        }

        void spawnFinished()
        {
            parentController.SpawnFinished();
        }
        void shootBullet()
        {
            parentController.ShootBullet();
        }
        void destroy()
        {
            parentController.DestroyObject();
        }
    }
}

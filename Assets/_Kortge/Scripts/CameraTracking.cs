using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kortge
{
    /// <summary>
    /// Keeps the camera at the designated location.
    /// </summary>
    public class CameraTracking : MonoBehaviour
    {
        public AudioManager audioManager;
        public Transform target;

        /// <summary>
        /// The game opens with an applause, similar to a theatre performance.
        /// </summary>
        private void Start()
        {
            audioManager.Play("Applause");
        }

        /// <summary>
        /// Runs every time the physics engine ticks to go to where its current target is at.
        /// </summary>
        void Update()
        {
            if (target)
            {
                transform.position = target.position;
            }
        }
    }
}
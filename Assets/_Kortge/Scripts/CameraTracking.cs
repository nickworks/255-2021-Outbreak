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
        public Transform target;

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
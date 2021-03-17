using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Geib
{
    public class CameraTracking : MonoBehaviour
    {
        /// <summary>
        /// This is the position for the target object that will be tracked by the camera rig.
        /// </summary>
        public Transform target;

        /// <summary>
        /// Runs every game tick
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
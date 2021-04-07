using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith
{
    public class CameraTracking : MonoBehaviour
    {
        /// <summary>
        /// Holds the transform of the player
        /// </summary>
        public Transform target;

        void LateUpdate()
        {
            if (target)
            {
                 // frame-rate independent slide
                float p = 1 - Mathf.Pow(.01f, Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, target.position, p);
            }
        }
    }
}


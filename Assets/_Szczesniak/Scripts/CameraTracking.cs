using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    /// <summary>
    /// This class is for following the target it is on (mostly player).
    /// </summary>
    public class CameraTracking : MonoBehaviour {

        /// <summary>
        /// Getting target to follow.
        /// </summary>
        public Transform target;

        /// <summary>
        /// Setting transition speed (keeping it hiden to be accesed by other scripts, but not be changed in inspector.
        /// </summary>
        [HideInInspector] public float smoothTransition = .01f;

        /// <summary>
        /// Runs everytime the physics engine ticks.
        /// </summary>
        void LateUpdate() {

            if (target) {
                //transform.position = target.position;

                //transform.position += (target.position - transform.position) * .05f;
                //transform.position = Vector3.Lerp(transform.position, target.position, .05f);
                //print = 1 - pow(amountLeftAfter1Second, deltaTime)
                //current = lerp(current, target, p);

                // framerate independent slide:

                float p = 1 - Mathf.Pow(smoothTransition, Time.deltaTime); // Getting the power 
                transform.position = Vector3.Lerp(transform.position, target.position, p); // moves camera to target using Lerp
            }

        }
    }
}
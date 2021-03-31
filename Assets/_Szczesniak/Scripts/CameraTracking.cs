using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    public class CameraTracking : MonoBehaviour {

        public Transform target;

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

                float p = 1 - Mathf.Pow(.01f, Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, target.position, p);
            }

        }
    }
}
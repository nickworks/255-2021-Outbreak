using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    public class CameraTracking : MonoBehaviour {

        public Transform target;

        /// <summary>
        /// Runs everytime the physics engine ticks.
        /// </summary>
        void Update() {

            if (target) {
                //transform.position = target.position;

                transform.position += (target.position - transform.position) * .025f;
            }

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    /// <summary>
    /// This script makes objects that are in the way of the camera to see the player
    /// be faded to see player to know what is happening
    /// </summary>
    public class ForegroundRaycaster : MonoBehaviour {

        /// <summary>
        /// declare camera
        /// </summary>
        Camera cam;

        /// <summary>
        /// Get CameraTracking script
        /// </summary>
        CameraTracking camTracker;

        // track things that are invisible...
        /// <summary>
        /// Get meshRenderer from objects touching the ray caster
        /// </summary>
        MeshRenderer hiddenObj;

        void Start() {
            cam = GetComponent<Camera>(); // get camera component
            camTracker = GetComponentInParent<CameraTracking>(); // get CameraTracking component
        }


        void Update() {
            if (hiddenObj) { // if the hiddenObj is used
                hiddenObj.material.color = new Color(1, 1, 1, 1); // sets material color
                hiddenObj = null; // makes hiddenObj null
            }
            DoRaycast(); // calls DoRaycast
        }

        /// <summary>
        /// Creates a raycast that checks to see if the raycast is off of the player or not
        /// </summary>
        void DoRaycast() {

            Vector3 vToTarget = camTracker.target.position - transform.position; // create Vector3 to get the distance
            Ray ray = new Ray(transform.position, vToTarget); // create a ray that points from it home position to the target

            if (Physics.Raycast(ray, out RaycastHit hit)) { // if the raycast hits a object
                Transform thingWeHit = hit.transform; // declares and stores the object the ray hit

                if (thingWeHit != camTracker.target) { // if the object that was hit does not equal the camTracker target
                    MeshRenderer renderer = thingWeHit.GetComponent<MeshRenderer>(); // get the object's MeshRenderer
                    //renderer.enabled = false;
                    renderer.material.color = new Color(1, 1, 1, .5f); // changes the color

                    hiddenObj = renderer; // makes the hiddenObj store the renderer
                }
            }
        }
    }
}
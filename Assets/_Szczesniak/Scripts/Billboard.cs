using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak { 

    /// <summary>
    /// This class makes a Canvas UI Rotate to face the camera it is assigned to.
    /// </summary>
    public class Billboard : MonoBehaviour {

        /// <summary>
        /// Getting Camera to make UI face at.
        /// </summary>
        private Camera cam;

        void Start() {
            // Assigning cam to player's camera
            cam = Camera.main;
        }

        void LateUpdate() {
            // This makes the UI face the camera
            transform.LookAt(transform.position + cam.transform.forward);
        }
    }
}
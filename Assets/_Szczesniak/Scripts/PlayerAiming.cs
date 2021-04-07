using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    public class PlayerAiming : MonoBehaviour {

        /// <summary>
        /// Gamera that will be used to point a ray cast
        /// </summary>
        private Camera cam;

        /// <summary>
        /// for the raycast to hit
        /// </summary>
        public Transform debugObject;

        /// <summary>
        /// if aiming with mouse
        /// </summary>
        private bool isAimingWithMouse = true;

        /// <summary>
        /// Gets the player's health
        /// </summary>
        private HealthScript playerHealth;

        void Start() {
            cam = Camera.main; // assigning the camera
            playerHealth = GetComponent<HealthScript>(); // Gets player's health information
        }

        void Update() {

            AutoDetectInput(); // checks to see if the player is using a controller or mouse

            if (isAimingWithMouse) // if aiming with mouse
                AimAtMouse();
            else // if not
                AimWithController();

        }

        /// <summary>
        /// Checks whether or not player is using a controller or mouse
        /// </summary>
        private void AutoDetectInput() {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) { // if mouse is moving
                isAimingWithMouse = true;
            }
            
            if (Input.GetAxis("Aim Horizontal") != 0 || Input.GetAxis("Aim Vertical") != 0) { // if controller is moving
                isAimingWithMouse = false;
            }
        }

        /// <summary>
        /// if the player is aiming with a controller
        /// </summary>
        private void AimWithController() {

            // -1 to 1 from controller input
            float h = Input.GetAxis("Aim Horizontal");
            float v = Input.GetAxis("Aim Vertical");

            float magSq = h * h + v * v; // magnitudeSquared
            float threshold = .5f; // threshold to not go past

            if (magSq < threshold * threshold) return; // stops running everything

            float angle = Mathf.Atan2(h, v); // gets angle

            //angle *= 180 / Mathf.PI;

            angle *= Mathf.Rad2Deg; // convert to degrees

            transform.eulerAngles = new Vector3(0, angle, 0); // makes the player move


            // goal: set transform.eulerAngles (0, y, 0);
        }

        /// <summary>
        /// When the player is using the mouse to aim
        /// </summary>
        private void AimAtMouse() {

            if (playerHealth.health <= 0 || Time.timeScale <= 0) return; // if the player is dead or the game is paused

            // make a ray and a plane
            Ray ray = cam.ScreenPointToRay(Input.mousePosition); // makes ray point 
            Plane plane = new Plane(Vector3.up, transform.position); //Vector3 up means new Vector3(0, 1, 0)

            // does the ray hit the plane?
            if (plane.Raycast(ray, out float dis))
            {

                // find point where the ray hit the plane:
                Vector3 hitPos = ray.GetPoint(dis);

                if (debugObject) debugObject.position = hitPos;

                Vector3 vectorToHitPos = hitPos - transform.position;

                float angle = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.z);
                angle /= Mathf.PI; // convert from "radians" to "half-circles"
                angle *= 180; // convert from "half-cirlce" to "degrees"

                transform.eulerAngles = new Vector3(0, angle, 0); // makes player point to cursor

            }
        }
    }
}
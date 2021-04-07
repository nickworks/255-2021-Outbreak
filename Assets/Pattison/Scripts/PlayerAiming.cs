using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pattison {
    public class PlayerAiming : MonoBehaviour {

        private Camera cam;

        public Transform debugObject;

        private bool isAimingWithMouse = true;


        void Start() {
            cam = Camera.main;
        }

        // Update is called once per frame
        void Update() {

            if (Outbreak.Game.isPaused) return;


            AutoDetectInput();

            if(isAimingWithMouse) AimAtMouse();
            else AimWithController();

        }
        void AutoDetectInput() {

            if(Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0) {
                isAimingWithMouse = true;
            }

            if (Input.GetAxisRaw("Aim Horizontal") != 0 || Input.GetAxisRaw("Aim Vertical") != 0) {
                isAimingWithMouse = false;
            }
        }


        private void AimWithController() {

            float h = Input.GetAxis("Aim Horizontal");
            float v = Input.GetAxis("Aim Vertical");

            float magSq = h * h + v * v;
            float threshold = 0.5f;

            if (magSq < threshold * threshold) return;

            float angle = Mathf.Atan2(h, v);

            //angle *= 180 / Mathf.PI;

            angle *= Mathf.Rad2Deg; // convert to degrees

            transform.eulerAngles = new Vector3(0, angle, 0);
        }

        private void AimAtMouse() {
            // make a ray and a plane:
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);

            // does the ray hit the plane?
            if (plane.Raycast(ray, out float dis)) {

                // find point where the ray hit the plane:
                Vector3 hitPos = ray.GetPoint(dis);

                if (debugObject) debugObject.position = hitPos;

                Vector3 vectorToHitPos = hitPos - transform.position;

                float angle = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.z);

                angle /= Mathf.PI; // convert from "radians" to "half-circles"
                angle *= 180; // convert from "half-circles" to "degrees"

                transform.eulerAngles = new Vector3(0, angle, 0);
            }
        }
    }
}
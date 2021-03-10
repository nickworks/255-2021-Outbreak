using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    public class PlayerAiming : MonoBehaviour {

        private Camera cam;

        public Transform debugObject;

        void Start() {
            cam = Camera.main;
        }


        void Update() {

            // make a ray and a plane
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position); //Vector3 up means new Vector3(0, 1, 0)

            // does the ray hit the plane?
            if (plane.Raycast(ray, out float dis)) {
                
                // find point where the ray hit the plane:
                Vector3 hitPos = ray.GetPoint(dis);

                if (debugObject) debugObject.position = hitPos;

            }
        }
    }
}
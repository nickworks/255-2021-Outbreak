using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hopkins
{
    public class PlayerAiming : MonoBehaviour
    {

        private Camera cam;

        public Transform debugObject;

        void Start()
        {
            cam = Camera.main;
        }

        // Update is called 1pf
        void Update()
        {
            // makes ray and a plane
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);

            // check if the ray hit the plane
            if (plane.Raycast(ray, out float dis))
            {

                // find point where the ray hit plane
                Vector3 hitPos = ray.GetPoint(dis);

                if (debugObject) debugObject.position = hitPos;

                Vector3 vectorToHitPos = hitPos - transform.position;

                float angle = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.z);

                angle /= Mathf.PI; // convertradians to half-circles
                angle *= 180; // convert half-circles" to degrees

                transform.eulerAngles = new Vector3(0, angle, 0);
            }

        }
    }
}
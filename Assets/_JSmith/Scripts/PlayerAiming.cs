using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _JSmith
{
    public class PlayerAiming : MonoBehaviour
    {


        private Camera cam;
        public Transform debugObject;

        void Start()
        {
            cam = Camera.main;
        }

        void Update()
        {
            // make a ray and a plane:
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);

            // does the ray hit th plane?
            if (plane.Raycast(ray, out float dis))
            {
                // find where the ray hit the plane:
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

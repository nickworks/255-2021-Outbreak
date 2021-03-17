using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Geib
{
    public class PlayerAiming : MonoBehaviour
    {
        /// <summary>
        /// A reference to the camera.
        /// </summary>
        private Camera cam;
        /// <summary>
        /// A debug object that displas where the mouse clicks.
        /// </summary>
        public Transform debugObject;

        void Start()
        {
            cam = Camera.main;
        }


        void Update()
        {
            //make a ray and a plane
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);

            // does the ray hit the plane?
            if (plane.Raycast(ray, out float dis))
            {
                // find where the ray hit the plane:
                Vector3 hitPos = ray.GetPoint(dis);

                if (debugObject) debugObject.position = hitPos;

                Vector3 vectorToHitPos = hitPos - transform.position;

                //Atan2 corrects for divide by 0 errors. Arguments are (numerator, denominator)
                //Atan returns in radians.
                float angle = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.z);

                angle /= Mathf.PI; // Convert from "radians" to "semi-circles"
                angle *= 180; // Converts from "semi-circles" to "degrees"

                transform.eulerAngles = new Vector3(0, angle, 0);
            }




        }
    }
}
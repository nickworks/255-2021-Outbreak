using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Velting
{
    public class PlayerAim : MonoBehaviour
    {
        private Camera cam;

        public Transform debugObject;

        private void Start()
        {
            cam = Camera.main;
        }
        private void Update()
        {
            AimAtMouse();
        }

        /// <summary>
        /// This function aims the player at the mouse.
        /// </summary>
        private void AimAtMouse()
        {
            // Create a ray and a plane.
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            Plane plane = new Plane(Vector3.up, transform.position);

            // Does the ray hit the plane?
            if (plane.Raycast(ray, out float dis))
            {
                // How far from the origin of the ray
                Vector3 hitPos = ray.GetPoint(dis);

                if (debugObject) debugObject.position = hitPos;

                Vector3 vectorToHitPos = hitPos - transform.position;

                float angleInRad = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.z);

                // Converting radians to degrees
                angleInRad /= Mathf.PI; // Convert to half circles
                angleInRad *= 180; // Converts to a full circle/degrees

                transform.eulerAngles = new Vector3(0, angleInRad, 0);
            }
        }
    }
}

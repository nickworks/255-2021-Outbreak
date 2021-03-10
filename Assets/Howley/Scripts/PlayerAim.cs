using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howley
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
            // Create a ray and a plane.
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            Plane plane = new Plane(Vector3.up, transform.position);

            // Does the ray hit the plane?
            if (plane.Raycast(ray, out float dis))
            {
                // How far from the origin of the ray
                Vector3 hitPos = ray.GetPoint(dis);

                if (debugObject) debugObject.position = hitPos;
            }
        }
    }
}

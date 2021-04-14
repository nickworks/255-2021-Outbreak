using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Miller
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

            AimAtMouse();


        }

        private void AimAtMouse()
        {
            //makes the plane and the ray
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);

            //does the ray hit the plan
            if (plane.Raycast(ray, out float dis))
            {
                //find where the ray hits the plane
                Vector3 hitPos = ray.GetPoint(dis);

                if (debugObject) debugObject.position = hitPos;

                Vector3 vectorToHitPos = hitPos - transform.position;
                float angle = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.z);

                angle /= Mathf.PI; // converts from radians to half-circles
                angle *= 180;// 

                transform.eulerAngles = new Vector3(0, angle, 0);
            }
        }
    }
}

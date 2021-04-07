using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Velting
{
    public class PlayerAim : MonoBehaviour
    {
        public Camera cam;

        public Transform debugObject;

        private void Start()
        {
            
        }
        private void Update()
        {
            //make a ray and a plane:
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position); //same as Vector3(0,1,0)

            //does the ray hit the plane?
            if(plane.Raycast(ray, out float dis))
            {
                //find where the ray hit the plane:
                Vector3 hitPos = ray.GetPoint(dis);
                if(debugObject) debugObject.position = hitPos;

                Vector3 vectorToHitPos = hitPos - transform.position;


                float angle = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.z);

                angle /= Mathf.PI; //convert from "radians" to "half-circles"
                angle *= 180; //convert from "half-circles" to "degrees"

                transform.eulerAngles = new Vector3(0, angle, 0);
            }
            
        }

        
        
    }
}

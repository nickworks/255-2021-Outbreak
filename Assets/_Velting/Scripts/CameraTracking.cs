using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Velting
{
    public class CameraTracking : MonoBehaviour
    {
        public Transform target;

        
        void LateUpdate()
        {
            if (target)
            {

               //transform.position = target.position;

               //Vector3 vToTarget = target.position - transform.position;

               // transform.position += vToTarget * .02f;

                
                //current = lerp(current, target, p)

                float p = 1 - Mathf.Pow(0.1f, Time.deltaTime);

                transform.position = Vector3.Lerp(transform.position, target.position, p);
            }
        }
    }
}

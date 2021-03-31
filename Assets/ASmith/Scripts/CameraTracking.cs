using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith
{
    public class CameraTracking : MonoBehaviour
    {
        public Transform target;

        void LateUpdate()
        {
            if (target)
            {
                //transform.position += (target.position - transform.position) * .01f;
                //transform.position = Vector3.Lerp(transform.position, target.position, .05f);

                //p = 1 - pow(amountLeftAfter1Second, Time.deltaTime)
                //current = lerp(current, target, p)

                 // frame-rate independent slide
                float p = 1 - Mathf.Pow(.01f, Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, target.position, p);
            }
        }
    }
}


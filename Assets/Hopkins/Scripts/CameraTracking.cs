using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hopkins
{
    public class CameraTracking : MonoBehaviour
    {

        public Transform target;

        /// <summary>
        /// runs whenever the physics engine does a tick
        /// </summary>
        void LateUpdate()
        {
            if (target)
            {
                //transform.position = target.position;
                //transform.position += (target.position - transform.position) * .05f;
                //transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime);

                //p = 1 - pow(amountLeftAfter1Second, deltaTime)
                //current = lerp(current, target, p)

                // framerate-independent slide:

                float p = 1 - Mathf.Pow(.01f, Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, target.position, p);

            }
        }
    }
}
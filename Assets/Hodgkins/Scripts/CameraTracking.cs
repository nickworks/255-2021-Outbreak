using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hodgkins
{
    public class CameraTracking : MonoBehaviour
    {
        public Transform target;


        // Update is called once per frame
        void LateUpdate()
        {
            if (target)
            {
                //transform.position = target.position;
                //transform.position += (target.position - transform.position) * .05f;
                //transform.position = Vector3.Lerp(transform.position, target.position, .05f);

                //p = 1 - pow(amountLeftAfter1Second, deltaTime)
                //current = lerp(current, target, print)

                float p = 1 - Mathf.Pow(.01f, Time.deltaTime);

                transform.position = Vector3.Lerp(transform.position, target.position, p);

            }
        }
    }
}
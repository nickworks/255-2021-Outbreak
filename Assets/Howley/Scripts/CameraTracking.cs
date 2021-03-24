using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howley
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
                //transform.position += vToTarget * .01f;
                //transform.position = Vector3.Lerp(transform.position, target.position, .05f);

                // Framerate independant slide
                float p = 1 - Mathf.Pow(.01f, Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, target.position, p);
            }
        }
    }
}


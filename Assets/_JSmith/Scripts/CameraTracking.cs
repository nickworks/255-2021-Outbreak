using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _JSmith
{
    public class CameraTracking : MonoBehaviour
    {

        public Transform target;

        void LateUpdate()
        {
            if (target)
            {
                //transform.position = target.position;
                //transform.position += (target.position - transform.position) * .05f;
                //transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime) * .05f;

                //framereate independent slide
                float p = 1 - Mathf.Pow(.01f, Time.deltaTime);

                transform.position = Vector3.Lerp(transform.position, target.position, p);
            }
        }
    }
}

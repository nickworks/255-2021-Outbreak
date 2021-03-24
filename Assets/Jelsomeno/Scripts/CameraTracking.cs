using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jelsomeno
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
                //Vector3 vToTarget = target.position - transform.position;
                //transform.position += vToTarget * .05f;
                //transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime);

                float p = 1 - Mathf.Pow(.01f, Time.deltaTime);

                transform.position = Vector3.Lerp(transform.position, target.position, p);





            }
        }
    }
}

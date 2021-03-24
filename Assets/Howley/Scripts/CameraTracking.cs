using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howley
{
    public class CameraTracking : MonoBehaviour
    {
        public Transform target;

        void Update()
        {
            if (target)
            {
                //transform.position = target.position;

                Vector3 vToTarget = target.position - transform.position;

                transform.position += vToTarget * .01f;
            }
        }
    }
}


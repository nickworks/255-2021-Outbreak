using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith
{
    public class CameraTracking : MonoBehaviour
    {
        public Transform target;

        void Update()
        {
            if (target)
            {
                transform.position += (target.position - transform.position) * .01f;
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Miller
{
    public class CameraTracking : MonoBehaviour
    {

        public Transform target;
        /// <summary>
        /// Runs everytime physics engine ticks
        /// </summary>
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (target)
            {
                transform.position = target.position;
            }
        }
    }
}

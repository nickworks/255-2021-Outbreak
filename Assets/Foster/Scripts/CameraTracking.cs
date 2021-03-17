using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Foster
{
    public class CameraTracking : MonoBehaviour
    {
        public Transform target;


        void Start()
        {



        }


        void Update()
        {
            if (target)
            {
                transform.position = target.position;
            }


        }
    }
}
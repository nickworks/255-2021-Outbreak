using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Foster
{
    public class CameraTracking : MonoBehaviour
    {
        public Transform target;


        void Update()
        {
            if (target)
            {
                // transform.position = target.position;

                //Vector3 vToTarget = target.position - transform.position;
                //transform.position += vToTarget * 07f * Time.deltaTime;

                //transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime);

                float p = 1 - Mathf.Pow(.01f, Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, target.position, p);
            }


        }
    }
}
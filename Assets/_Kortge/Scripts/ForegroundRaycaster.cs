using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    public class ForegroundRaycaster : MonoBehaviour
    {
        Camera cam;
        CameraTracking camTracker;
        public Transform hiddenThing;

        // Start is called before the first frame update
        void Start()
        {
            cam = GetComponent<Camera>();
            camTracker = GetComponentInParent<CameraTracking>();
        }

        // Update is called once per frame
        void Update()
        {
            if (hiddenThing)
            {
                GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
            }
            DoRaycast();
        }

        void DoRaycast()
        {
            Vector3 vToTarget = camTracker.target.position - transform.position;

            Ray ray = new Ray(transform.position, vToTarget);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Transform thingWeHit = hit.transform;

                if(thingWeHit != camTracker.target)
                {
                    var renderer = thingWeHit.GetComponent<MeshRenderer>();
                    //renderer.material.color = new Color(1, 1, 1, .5);

                    //hiddenThing = renderer;
                }
            }
        }
    }
}
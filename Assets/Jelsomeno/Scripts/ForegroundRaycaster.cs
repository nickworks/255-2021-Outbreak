using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
namespace Jelsomeno
{
    
    public class ForegroundRaycaster : MonoBehaviour
    {

        Camera cam;
        CameraTracking camTracker;

        MeshRenderer hiddenThing;

        void Start()
        {
            cam = GetComponent<Camera>();
            camTracker = GetComponentInParent<CameraTracking>();
        }

        void Update()
        {
            if (hiddenThing)
            {
                hiddenThing.material.color. = new Color(1, 1, 1, 1);
            }
            DoRayCast();
        }

        void DoRayCast()
        {
            Vector3 vToTarget = camTracker.target.position - transform.position;

            Ray ray = new Ray(transform.position, vToTarget);

            if( Physics.Raycast(ray, out RaycastHit hit))
            {
                Transform thingWeHit = hit.transform;

                if(thingWeHit != camTracker.target)
                {
                    MeshRenderer renderer = thingWeHit.GetComponent<MeshRenderer>();
                    //renderer.enabled = false;
                    renderer.material.color. = new Color(1, 1, 1, .5f);

                    hiddenThing = renderer;

                }

            }

        }

    }

}
*/
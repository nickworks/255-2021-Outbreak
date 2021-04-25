using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith
{
    public class ForegroundRayCaster : MonoBehaviour
    {
        CameraTracking camTracker;

        /// <summary>
        /// Holds the mesh of the hidden object
        /// </summary>
        MeshRenderer hiddenThing;

        /// <summary>
        /// Holds the color of whatever object was hit
        /// </summary>
        Color thingWeHitsColor;

        void Start()
        {
            camTracker = GetComponentInParent<CameraTracking>();
        }

        void Update()
        {
            if (hiddenThing)
            {
                hiddenThing.material.color = thingWeHitsColor; // resets the hit object color to normal
                hiddenThing = null;
            }
            DoRaycast();
        }

        void DoRaycast()
        {
            Vector3 vToTarget = camTracker.target.position - transform.position;
            Ray ray = new Ray(transform.position, vToTarget);

            if (Physics.Raycast(ray, out RaycastHit hit)) // If drawn ray hits an object...
            {
                Transform thingWeHit = hit.transform; // track the transform of the hit object

                if(thingWeHit != camTracker.target) // If thingwehit is NOT the player...
                {
                    MeshRenderer renderer = thingWeHit.GetComponent<MeshRenderer>(); // get the mesh renderer of the hit object
                    thingWeHitsColor = renderer.material.color; // get the color of the hit object
                    renderer.material.color = new Color(thingWeHitsColor.r, thingWeHitsColor.g, thingWeHitsColor.b, .5f); // set the transparancy of the hit object to 50%

                    hiddenThing = renderer;
                }
            }
        }
    }
}
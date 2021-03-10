using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    /// <summary>
    /// A reference to the camera.
    /// </summary>
    private Camera cam;
    /// <summary>
    /// A debug object that displas where the mouse clicks.
    /// </summary>
    public Transform debugObject;

    void Start()
    {
        cam = Camera.main;
    }


    void Update()
    {
        //make a ray and a plane
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);

        // does the ray hit the plane?
        if (plane.Raycast(ray, out float dis))
        {
            // find where the ray hit the plane:
            Vector3 hitPos = ray.GetPoint(dis);

            if (debugObject) debugObject.position = hitPos;
        }




    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    private Camera cam;

    public Transform debugObject;

    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // make a ray and plane
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);

        // does the ray hit the plane?
        if(plane.Raycast(ray, out float dis))
        {
            // find where the ray hit the plane
            Vector3 hitPos = ray.GetPoint(dis);
            if(debugObject) debugObject.position = hitPos;
        }

    }
}

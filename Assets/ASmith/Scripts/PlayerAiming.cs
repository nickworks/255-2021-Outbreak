using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    private Camera cam; // doesnt need to be public because it's called through code

    public Transform debugObject;

    void Start()
    {
        cam = Camera.main; // Gets camera at start
    }

    void Update()
    {
        // make a ray and a plane:
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);

        // does the ray hit the plane?
        if (plane.Raycast(ray, out float dis))
        {
            // find point where the ray hits the plane
            Vector3 hitPos = ray.GetPoint(dis);
            debugObject.position = hitPos;
        }
    }
}

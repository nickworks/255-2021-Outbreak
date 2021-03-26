using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Transform end;
    private LineRenderer line;
    private BoxCollider box;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        box = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        end.position += transform.forward * Time.deltaTime * speed;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, end.position);
        float boxZ = end.position.z;
        if (boxZ < 0) boxZ = -boxZ;
        box.size = new Vector3(1, 1, boxZ);
        box.center = new Vector3(0, 0, boxZ/2);
    }
}

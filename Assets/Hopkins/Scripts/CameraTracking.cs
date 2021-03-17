using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{

    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// runs when physics engine ticks
    /// </summary>
    void Update()
    {
        if (target)
        {
            transform.position = target.position;
        }
    }
}

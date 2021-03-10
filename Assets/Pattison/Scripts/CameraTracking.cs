using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{

    public Transform target;

    /// <summary>
    /// Runs every time the physics engine ticks.
    /// </summary>
    void Update() {
        if (target) {
            transform.position = target.position;
        }
    }
}

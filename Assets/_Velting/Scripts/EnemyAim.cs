using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : MonoBehaviour
{

    public Camera cam;
    
    public Transform player;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AimAtPlayer();
    }

    public void AimAtPlayer()
    {
        
        
        Vector3 hitPos = player.transform.position;
        

        Vector3 vectorToHitPos = hitPos - transform.position;


        float angle = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.z);

        angle /= Mathf.PI; //convert from "radians" to "half-circles"
        angle *= 180; //convert from "half-circles" to "degrees"

        transform.eulerAngles = new Vector3(0, angle, 0);
        
    }
}

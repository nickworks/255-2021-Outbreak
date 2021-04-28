using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    /// <summary>
    /// How long the projectile should live in seconds
    /// </summary>
    private float lifespan = 3;

    /// <summary>
    /// How many seconds this projectile has been alive
    /// </summary>
    private float age = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        age += Time.deltaTime;
        if (age > lifespan)
        {
            Destroy(gameObject);
        }
    }
}

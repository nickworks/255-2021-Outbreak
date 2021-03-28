using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maiden : MonoBehaviour
{
    public Rigidbody rosePrefab;
    public bool roseThrown = false;
    private Rigidbody thrownRose;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (thrownRose = null) roseThrown = false;
    }

    public void ThrowRose()
    {
        thrownRose = Instantiate(rosePrefab, transform.position, transform.rotation);
        thrownRose.AddForce(thrownRose.transform.forward * 20, ForceMode.Impulse);
        roseThrown = true;
    }
}

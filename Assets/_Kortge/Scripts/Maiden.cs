using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maiden : MonoBehaviour
{
    public Rigidbody rosePrefab;
    private Rigidbody thrownRose;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ThrowRose()
    {
        thrownRose = Instantiate(rosePrefab, transform.position + (transform.right * Random.Range(-4, 4)), transform.rotation);
        thrownRose.AddForce(thrownRose.transform.forward * Random.Range(1, 10), ForceMode.Impulse);
    }
}

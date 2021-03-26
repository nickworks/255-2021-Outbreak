using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Walker : MonoBehaviour
{
    private NavMeshAgent nav;
    public Transform attackTarget;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();

        if (attackTarget != null) nav.SetDestination(attackTarget.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (attackTarget != null) nav.SetDestination(attackTarget.position);
    }
}

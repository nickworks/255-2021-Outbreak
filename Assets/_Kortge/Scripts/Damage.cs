using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    public class Damage : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Health victim = collision.GetComponent<Health>();
            victim.health--;
            Boss boss = victim.GetComponent<Boss>();
            if (boss != null) boss.hit = true;
        }
    }
}
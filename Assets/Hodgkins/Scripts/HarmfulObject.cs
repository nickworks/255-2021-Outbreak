using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hodgkins
{
    public class HarmfulObject : MonoBehaviour
    {
        public float damageAmount = 20;
        public void OnTriggerEnter(Collider other)
        {
            HealthSystem health = other.GetComponent<HealthSystem>();
            PlayerMovement pm = other.GetComponent<PlayerMovement>();

            if (health)
            {
                health.TakeDamage(damageAmount);
            }

            //Vector3 vToPlayer = (be.transform.position - this.transform.position).normalized;

            //be.LaunchPlayer(vToPlayer * 15);

            SoundEffectBoard.PlayHit();
        }
    }
}
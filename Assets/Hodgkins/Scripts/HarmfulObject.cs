using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hodgkins
{
    public class HarmfulObject : MonoBehaviour
    {
        public float damageAmount = 40;
        public void OnOverlap(EnemyBasicController be)
        {
            HealthSystem health = be.GetComponent<HealthSystem>();

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
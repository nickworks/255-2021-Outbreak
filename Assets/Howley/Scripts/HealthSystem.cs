using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howley
{
    public class HealthSystem : MonoBehaviour
    {
        public float health { get; private set; }

        public float healthMax = 100;

        private float damageCooldown = 0;

        private void Start()
        {
            health = healthMax;
        }

        private void Update()
        {
            if (damageCooldown > 0) damageCooldown -= Time.deltaTime;
        }

        public void Damage(float dmgAmount)
        {
            if (dmgAmount < 0) dmgAmount = 0;
            health -= dmgAmount;
            if (health <= 0) Death();
        }

        public void Death()
        {

        }
    }
}


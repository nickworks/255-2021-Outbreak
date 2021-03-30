using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    public class HealthScript : MonoBehaviour {

        public float health { get; private set; }

        public float maxHealth = 100;

        void Start() {
            health = maxHealth;
        }


        public void DamageTaken(float damage) {
            health -= damage;

            if (health <= 0) {
                Destroy(this.gameObject);
            }
        }
    }
}
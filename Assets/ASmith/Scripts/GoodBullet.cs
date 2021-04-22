using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith {
    public class GoodBullet : MonoBehaviour {
        /// <summary>
        /// Current speed and direction the player is moving
        /// </summary>
        private Vector3 velocity;

        /// <summary>
        /// How long the projectile should live, in seconds
        /// </summary>
        private float lifespan = 3;

        /// <summary>
        /// How long the projectile has been alive, in seconds
        /// </summary>
        private float age = 0;

        public static float damageAmount = 10;

        public GameObject boss;
        public GameObject turret;

        private SphereCollider bossCollider;
        private CapsuleCollider turretCollider;

        void Start()
        {
            bossCollider = boss.GetComponent<SphereCollider>();
            turretCollider = turret.GetComponent<CapsuleCollider>();
        }

        public void InitBullet(Vector3 vel)
        {
            velocity = vel;
        }

        void Update()
        {
            age += Time.deltaTime; // Tracks the age of the bullet
            if (age > lifespan)
            { // if age is greater than lifespan....
                Destroy(gameObject); // destroy bullet
            }

            RaycastCheck();

            // euler physics integration
            transform.position += velocity * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Enemy") // If triggered object is an enemy...
            {
                EnemyHealth health = other.GetComponentInParent<EnemyHealth>(); // Gets a reference to the EnemyHealth class for access to the health variable
                Destroy(gameObject); // Destroy bullet on collision with enemy
                if (health.health > 0) // if enemy has health
                {
                    health.TakeDamage(damageAmount); // Calls the TakeDamage function in the EnemyHealth script to deal damage to the enemy
                }
            }
            if (other.gameObject.tag != "Player")
            {
                Destroy(gameObject); // Destroy bullet on collision
            }
        }

        private void RaycastCheck()
        {
            Ray ray = new Ray(transform.position, velocity * Time.deltaTime); // creates a ray on bullet instantiation

            Debug.DrawRay(ray.origin, ray.direction); // draws ray in front of bullet

            if(Physics.Raycast(ray, out RaycastHit hit, ray.direction.magnitude))
            {
                if (hit.transform.tag == "Wall") // if ray hits an object with "Wall" tag...
                {
                    Vector3 normal = hit.normal;
                    normal.y = 0; // no vertical bouncing

                    Vector3 random = Random.onUnitSphere;
                    random.y = 0;

                    // blend the normal with the random:
                    normal += random * .3f;

                    normal.Normalize(); // makes unit vector

                    float alignment = Vector3.Dot(velocity, normal);
                    Vector3 reflection = velocity - 2 * alignment * normal;

                    reflection = Vector3.Lerp(reflection, Random.onUnitSphere, 0.5f);

                    velocity = reflection;
                    transform.position = hit.point;
                }
            }
        }
    }
}
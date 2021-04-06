using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith
{
    public class Turret : MonoBehaviour
    {
        public Transform barrel1;
        public Transform barrel2;
        public Transform barrel3;
        public Transform barrel4;

        //public GameObject bullet;

        public Projectile prefabProjectile;

        public float cooldownShoot = 0;
        public float roundsPerSecond = 6;

        void Update()
        {
            if (cooldownShoot > 0)
            {
                cooldownShoot -= Time.deltaTime;
            }

            SpawnProjectile();
        }

        private void SpawnProjectile()
        {
            if (cooldownShoot > 0) return; // still on cooldown

            Projectile p1 = Instantiate(prefabProjectile, barrel1.transform.position, barrel1.rotation);
            Projectile p2 = Instantiate(prefabProjectile, barrel2.transform.position, barrel2.rotation);
            Projectile p3 = Instantiate(prefabProjectile, barrel3.transform.position, barrel3.rotation);
            Projectile p4 = Instantiate(prefabProjectile, barrel4.transform.position, barrel4.rotation);

            p1.InitBullet(barrel1.transform.forward * 10);
            p2.InitBullet(barrel2.transform.forward * 10);
            p3.InitBullet(barrel3.transform.forward * 10);
            p4.InitBullet(barrel4.transform.forward * 10);

            cooldownShoot = 1 / roundsPerSecond;
        }
    }
}


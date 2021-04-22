using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith
{
    public class Turret : MonoBehaviour
    {
        /// <summary>
        /// Holds the transforms for each barrel of the turret
        /// </summary>
        public Transform barrel1;
        public Transform barrel2;
        public Transform barrel3;
        public Transform barrel4;

        /// <summary>
        /// Holds the information for the BadBullet used in the turret
        /// </summary>
        public BadBullet prefabBadBullet;

        /// <summary>
        /// Cooldown timer, set in the SpawnBadBullet method
        /// </summary>
        public float cooldownShoot = 0;
        /// <summary>
        /// Sets how fast the turret fires per second
        /// </summary>
        public float roundsPerSecond = 6;

        void Update()
        {
            if (cooldownShoot > 0)
            {
                cooldownShoot -= Time.deltaTime; // counts down the cooldown timer
            }

            SpawnBadBullet();
        }

        private void SpawnBadBullet()
        {
            if (cooldownShoot > 0) return; // still on cooldown

            SoundBoard.PlayTurretShoot();

            BadBullet p1 = Instantiate(prefabBadBullet, barrel1.transform.position, barrel1.rotation); // instantiates a bullet on each barrel
            BadBullet p2 = Instantiate(prefabBadBullet, barrel2.transform.position, barrel2.rotation);
            BadBullet p3 = Instantiate(prefabBadBullet, barrel3.transform.position, barrel3.rotation);
            BadBullet p4 = Instantiate(prefabBadBullet, barrel4.transform.position, barrel4.rotation); 

            p1.InitBullet(barrel1.transform.forward * 10); // launches the bullet on each barrel forward
            p2.InitBullet(barrel2.transform.forward * 10);
            p3.InitBullet(barrel3.transform.forward * 10);
            p4.InitBullet(barrel4.transform.forward * 10);

            cooldownShoot = 1 / roundsPerSecond; // resets the cooldown timer
        }
    }
}


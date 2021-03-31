using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform barrel;

    public GameObject bullet;

    public float cooldownShoot = 0;
    public float roundsPerSecond = 6;

    void Update()
    {
        if (cooldownShoot > 0)
        {
            cooldownShoot -= Time.deltaTime;
        }

        ShootPlayer();
    }

    private void ShootPlayer()
    {
        if (cooldownShoot > 0) return; // still on cooldown

        cooldownShoot = 1 / roundsPerSecond;

        Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
    }
}

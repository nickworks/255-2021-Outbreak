using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletWipe : MonoBehaviour
{
    public Text wipeCount;
    public float bulletWipes = 3;
    void Start()
    {
        wipeCount.text = "3";
    }

    void Update()
    {
        if (Input.GetButtonDown("BulletWipe") && bulletWipes > 0)
        {
            bulletWipes--;
            wipeCount.text = bulletWipes.ToString();
            GameObject[] BadBullets = GameObject.FindGameObjectsWithTag("BadBullet");
            SoundBoard.PlayPlayerWipe();

            foreach (GameObject BadBullet in BadBullets)
            {
                Destroy(BadBullet);                
            }
        }
        else if (Input.GetButtonDown("BulletWipe") && bulletWipes <= 0)
        {
            SoundBoard.PlayPlayerNoAmmo();
        }
    }
}

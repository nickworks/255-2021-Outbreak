using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kortge
{
    public class Health : MonoBehaviour
    {
        public int health;
        private SpriteRenderer sprite;
        private float invulnerabilityTime = 0.2f;
        public bool postHit;
        private bool transparent;
        private Color color;
        public bool vulnerable;
        public GameObject rose1;
        public GameObject rose2;
        public GameObject rose3;
        public GameObject rose4;
        public GameObject rose5;

        // Start is called before the first frame update
        void Start()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            color = sprite.color;
        }

        // Update is called once per frame
        void Update()
        {
            switch (health)
            {
                case 4:
                    rose5.SetActive(false);
                    break;
                case 3:
                    rose4.SetActive(false);
                    break;
                case 2:
                    rose3.SetActive(false);
                    break;
                case 1:
                    rose2.SetActive(false);
                    break;
                case 0:
                    rose1.SetActive(false);
                    Destroy(gameObject);
                    break;
            }
            if (postHit)
            {
                invulnerabilityTime -= Time.deltaTime;
                if (transparent)
                {
                    sprite.color = color;
                    transparent = false;
                }
                else
                {
                    sprite.color = Color.clear;
                    transparent = true;
                }
                if (invulnerabilityTime <= 0)
                {
                    sprite.color = color;
                    invulnerabilityTime = 0.2f;
                    postHit = false;
                }
                print(invulnerabilityTime);
            }
        }

        public void Damage()
        {
            if (!postHit && vulnerable)
            {
                health--;
                postHit = true;
                Boss boss = GetComponent<Boss>();
                if (boss != null) boss.hit = true;
            }
        }
    }
}
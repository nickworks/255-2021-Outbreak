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
        public ParticleSystem particles;
        private ParticleSystem blood;

        // Start is called before the first frame update
        void Start()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            color = sprite.color;
            blood = GetComponentInChildren<ParticleSystem>();
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
                    break;
            }
            if (postHit && health > 0)
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
            }
            if (health <= 0)
            {
                Instantiate(particles, transform.position, transform.rotation);
                Destroy(gameObject);
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
                blood.Play();
            }
        }
    }
}
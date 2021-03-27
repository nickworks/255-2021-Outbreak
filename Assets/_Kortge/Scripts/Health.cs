using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kortge
{
    public class Health : MonoBehaviour
    {
        public int health;
        private SpriteRenderer sprite;
        private float invulnerabilityTime = 0.25f;
        public bool invulnerable;
        private bool transparent;
        private Color color;
        // Start is called before the first frame update
        void Start()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            color = sprite.color;
        }

        // Update is called once per frame
        void Update()
        {
            if (health <= 0) Destroy(gameObject);
            if (invulnerable)
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
                    invulnerabilityTime = 0.25f;
                    invulnerable = false;
                }
            }
        }

        public void Damage()
        {
            if (!invulnerable)
            {
                health--;
                invulnerable = true;
                print("Ow!");
            }
        }
    }
}
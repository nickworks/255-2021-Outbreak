using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    private SpriteRenderer sprite;
    private float invulnerabilityTime = 0.25f;
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
    }

    public void Damage()
    {
        health--;
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
        }
    }
}

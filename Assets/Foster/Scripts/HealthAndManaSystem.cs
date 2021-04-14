using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthAndManaSystem : MonoBehaviour
{
    
    public static int health { get; set; }
    public int healthMax = 100;
    

    public float manaRegenTimer = 0f;

    public static int mana { get; set; }
    public int manaMax = 100;
  
    private void Start()
    {
        health = healthMax;
        mana = manaMax;
    }
    public void Update()
    {
       // print(mana);
        ManaRegen();
        if (manaRegenTimer >= 0) manaRegenTimer -= Time.deltaTime;
        
    }

    // makes the bullet injure the enemy
    public void OnTriggerEnter(Collider other)
    {
        if (this.tag == ("Enemy") & other.tag == ("Bullet")) TakeDamage(10);
    }


    //used for when the player or boss uses mana energy
    public void UseMana(int amt)
    {
        if (amt <= 0) return;

        mana -= amt;

    }

    //called when the player/boss takes damage
    public void TakeDamage(int amt)
    {
        if (amt <= 0) return;
        health -= amt;

        if (health <= 0) Die();
    }

    //mana regeneration over time when the player/boss uses spells 
    public void ManaRegen()
    {


        if (mana <= 5)
        {
            if (manaRegenTimer <= 0)
            {
                mana += 1;
                manaRegenTimer = .75f;
            }
        }

        if (mana == 6) return;

    }

    public void Die()
    {
        if (this.tag == ("Player"))
        {
            Game.GameOver();
        }
        if (this.tag == ("Enemy"))
        {
            Game.GotoNextLevel();
        }
    }

}
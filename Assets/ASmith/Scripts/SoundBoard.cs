using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBoard : MonoBehaviour
{
    public static SoundBoard main;

    #region List of Audio Clips and their Audio Source
    public AudioClip soundPlayerShoot;
    public AudioClip soundPlayerWipe;
    public AudioClip soundPlayerDash;
    public AudioClip soundPlayerDamage;
    public AudioClip soundPlayerShieldOn;
    public AudioClip soundPlayerShieldOff;
    public AudioClip soundPlayerNoAmmo;
    public AudioClip soundPlayerDie;
    public AudioClip soundTurretShoot;
    public AudioClip soundTurretDie;
    public AudioClip soundBossRage;
    public AudioClip soundBossDie;
    public AudioClip soundBossSummonTurret;
    public AudioClip soundBossFightMusic;
    public AudioClip soundBossRageMusic;
    public AudioClip soundAmbientMusic;

    public AudioSource player;
    #endregion

    void Start()
    {
        if (main == null)
        {
            main = this;
            player = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(this.gameObject);
        }

        PlayAmbientMusic(); // Plays ambient music on start
    }

    public static void PlayPlayerShoot() // Audio for the player shooting
    {
        main.player.PlayOneShot(main.soundPlayerShoot);
    }

    public static void PlayPlayerWipe() // Audio for the player's bullet wipe ability
    {
        main.player.PlayOneShot(main.soundPlayerWipe);
    }

    public static void PlayPlayerDash() // Audio for the player's dash ability
    {
        main.player.PlayOneShot(main.soundPlayerDash);
    }

    public static void PlayPlayerDamage() // Audio for when the player takes damage
    {
        main.player.PlayOneShot(main.soundPlayerDamage);
    }

    public static void PlayPlayerShieldOn() // Audio for when the player turn's ON their shield
    {
        main.player.PlayOneShot(main.soundPlayerShieldOn);
    }

    public static void PlayPlayerShieldOff() // Audio for when the player tuns OFF their shield
    {
        main.player.PlayOneShot(main.soundPlayerShieldOff);
    }

    public static void PlayPlayerNoAmmo() // Audio for when the player tries to fire with no ammo
    {
        main.player.PlayOneShot(main.soundPlayerNoAmmo);
    }

    public static void PlayPlayerDie() // Audio for when the player dies
    {
        main.player.PlayOneShot(main.soundPlayerDie);
    }

    public static void PlayTurretShoot() // Audio for when a turret fires
    {
        main.player.PlayOneShot(main.soundTurretShoot);
    }

    public static void PlayTurretDie() // Audio for when a turret dies
    {
        main.player.PlayOneShot(main.soundTurretDie);
    }

    public static void PlayBossRage() // Audio for when boss enters rage mode
    {
        main.player.PlayOneShot(main.soundBossRage);
    }

    public static void PlayBossDie() // Audio for when boss dies
    {
        main.player.PlayOneShot(main.soundBossDie);
    }

    public static void PlayBossSummonTurret() // Audio for when the boss summons a turret
    {
        main.player.PlayOneShot(main.soundBossSummonTurret);
    }

    public static void PlayBossFightMusic() // Music for when the boss battle begins
    {
        main.player.Stop();
        main.player.PlayOneShot(main.soundBossFightMusic);
    }

    public static void PlayBossRageMusic() // Music for when the boss enters rage mode
    {
        main.player.Stop();
        main.player.PlayOneShot(main.soundBossRageMusic);
    }

    public static void PlayAmbientMusic() // Music for before the boss battle begins
    {
        main.player.Stop();
        main.player.PlayOneShot(main.soundAmbientMusic);
    }
}

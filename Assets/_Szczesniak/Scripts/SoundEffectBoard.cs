using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    
    /// <summary>
    /// This class if for when the player, minion, and boss does sound effect actions
    /// </summary>
    public class SoundEffectBoard : MonoBehaviour {

        /// <summary>
        /// This is a Singleton!
        /// </summary>
        public static SoundEffectBoard main;

        /// <summary>
        /// When the player shoots
        /// </summary>
        public AudioClip shooting;

        /// <summary>
        /// Sound when the player dies
        /// </summary>
        public AudioClip soundDie;

        /// <summary>
        /// Sound when the player shoots the rocket
        /// </summary>
        public AudioClip rocket;

        /// <summary>
        /// Sound when the player hurts themself
        /// </summary>
        public AudioClip playerHurt;

        /// <summary>
        /// Sound when the player dashes
        /// </summary>
        public AudioClip soundDash;

        /// <summary>
        /// Sound when the boss dies
        /// </summary>
        public AudioClip bossDeath;

        /// <summary>
        /// Sound when the boss is shooting
        /// </summary>
        public AudioClip bossShooting;

        /// <summary>
        /// Creating audio source to play sounds on
        /// </summary>
        private AudioSource player;

        // Start is called before the first frame update
        void Start() {

            if (main == null) {
                main = this;
                player = GetComponent<AudioSource>();
            } else {
                Destroy(this.gameObject);
            }

        }

        /// <summary>
        /// Plays when the player shoots
        /// </summary>
        public static void PlayerShooting() {
            main.player.PlayOneShot(main.shooting);
        }

        /// <summary>
        /// Sound when the boss shoots
        /// </summary>
        public static void BossShooting() {
            main.player.PlayOneShot(main.bossShooting);
        }

        /// <summary>
        /// Plays when the player shoots the rocket
        /// </summary>
        public static void RocketMissileSound() {
            main.player.PlayOneShot(main.rocket);
        }

        /// <summary>
        /// Plays the sound death file when the player dies
        /// </summary>
        public static void PlayDeathSound() {
            main.player.PlayOneShot(main.soundDie);
        }

        /// <summary>
        /// Plays the sound when the player takes damage
        /// </summary>
        public static void PlayerDamaged() {
            main.player.PlayOneShot(main.playerHurt);
        }

        /// <summary>
        /// Plays when the player is dashing
        /// </summary>
        public static void DashSound() {
            main.player.PlayOneShot(main.soundDash);
        }

        /// <summary>
        /// Plays when the boss dies
        /// </summary>
        public static void BossDeathSound() {
            main.player.PlayOneShot(main.bossDeath);
        }
    }
}
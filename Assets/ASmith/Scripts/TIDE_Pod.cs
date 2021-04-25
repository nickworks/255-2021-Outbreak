using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

namespace ASmith
{
    public class TIDE_Pod : MonoBehaviour
    {
        /// <summary>
        /// Singleton
        /// </summary>
        public static TIDE_Pod main;

        public enum PodState
        {
            Regular, // 0
            Shooting, // 1
            Charging, // 2
            Off // 3
        }

        PodState currentPodState = PodState.Off;

        /// <summary>
        /// Variable that declares the maximum battery life
        /// </summary>
        private float batteryMax = 25;

        /// <summary>
        /// Variable that declares the minimum battery life
        /// </summary>
        private float batteryMin = 0;

        /// <summary>
        /// Variable that declares the current battery life
        /// </summary>
        private float currBattery;

        /// <summary>
        /// Variable that countsdown to whether or not another bullet can be fired
        /// </summary>
        private float timerSpawnBullet = 0;

        /// <summary>
        /// Variable that tracks how fast the gun can be fired
        /// </summary>
        private float roundsPerSecond = 5;

        /// <summary>
        /// Variable that references the nav mesh in the scene
        /// </summary>
        private NavMeshAgent nav;

        /// <summary>
        /// Variable that references the prefab for the GoodBullet 
        /// </summary>
        public GoodBullet prefabGoodBullet;

        /// <summary>
        /// Variable that references the player Game Object
        /// </summary>
        public GameObject player;

        /// <summary>
        /// Variable that references the player's location in the scene
        /// </summary>
        public Transform playerLocation;

        /// <summary>
        /// Variable that tracks the location and rotation of the barrl on the TIDE_Pod
        /// </summary>
        public Transform barrel;

        /// <summary>
        /// Variable that tracks the location of the debug object for aiming
        /// </summary>
        public Transform debugObject;

        /// <summary>
        /// Variable that references the camera in the scene
        /// </summary>
        private Camera cam;

        /// <summary>
        /// Variable that references the fill image for the battery in the UI
        /// </summary>
        public Image batteryFillImage;

        /// <summary>
        /// Variable that references the text used to communicate the pods status to the UI
        /// </summary>
        public Text podStatus;

        public float batteryValue
        {
            get
            {
                return currBattery;
            }
            set
            {
                // Clamps the passed value within min/max range
                currBattery = Mathf.Clamp(value, batteryMin, batteryMax);

                // Calculates the current fill percentage and displays it
                float fillPercentage = currBattery / batteryMax;
                batteryFillImage.fillAmount = fillPercentage;
            }
        }

        void Start()
        {
            nav = GetComponent<NavMeshAgent>(); // Gets a reference to the Nav Mesh in the scene

            currBattery = batteryMax; // Sets the current battery level to the maximum at the start of the level

            cam = Camera.main;
        }

        void Update()
        {
            FollowPlayer();
            AimAtMouse();

            batteryValue = currBattery; // Sets the batteryValue to the tracked current battery

            if (timerSpawnBullet > 0) timerSpawnBullet -= Time.deltaTime; // if the timer is GREATER THAN 0, countdown the timer

            switch (currentPodState) // State Switcher
            {
                case PodState.Regular: // Regular State: Follows player, depletes battery
                    // Behavior
                    currBattery -= Time.deltaTime; // Battery drops over time
                    podStatus.text = ("ONLINE"); // UI tells player the pod is online

                    // Transition
                    if (Input.GetButton("Fire1")) { currentPodState = PodState.Shooting; } // If player presses "Left Click", switch to Shooting state
                    if (Input.GetButtonDown("FollowerPower"))  // If player presses "F"... 
                    {
                        SoundBoard.PlayPlayerShieldOff(); // Play Player Shield Off Sound
                        currentPodState = PodState.Charging; // Switch to charging state
                    }

                    if (currBattery <= batteryMin) // if current battery is LESS THAN or EQUAL TO minimum battery...
                    {
                        SoundBoard.PlayPlayerShieldOff(); // Play player shield off sound
                        currentPodState = PodState.Charging; // switch to charging state
                    }

                    break;

                case PodState.Shooting: // Shooting State: Follows player, Shoots when player shoots, depletes battery
                    // Behavior
                    currBattery -= Time.deltaTime; // battery drops over time
                    SpawnGoodBullet(); 
                    podStatus.text = ("FIRING"); // UI tells player the pod is firing

                    // Transition
                    if (!Input.GetButton("Fire1")) { currentPodState = PodState.Regular; } // If player is not left clicking, switch to regular state

                    if (currBattery <= batteryMin) // if current battery is LESS THAN or EQUAL TO minimum battery...
                    {
                        SoundBoard.PlayPlayerShieldOff(); // Player player shield off sound
                        currentPodState = PodState.Charging; // switch to charging state
                    }

                    break;

                case PodState.Charging: // Charging State: Follows player, charges battery
                    // Behavior
                    currBattery += Time.deltaTime * 1.5f; // current battery increases over time
                    podStatus.text = ("CHARGING"); // UI tells player the pod is charging

                    // Transition
                    if (Input.GetButtonDown("FollowerPower")) // If player presses "F"
                    {
                        SoundBoard.PlayPlayerShieldOn(); // Play player shield on sound
                        currentPodState = PodState.Regular; // Switch to regular state
                    }

                    if (currBattery >= batteryMax) { currentPodState = PodState.Off; } // If current battery is GREATER THAN or EQUAL TO maximum battery, switch to Off state 

                    break;

                case PodState.Off: // Off State: Do nothing
                    // Behavior
                    podStatus.text = ("OFFLINE"); // UI tells player the pod is offline

                    // Transition
                    if (Input.GetButtonDown("FollowerPower")) // If player presses "F"
                    {
                        SoundBoard.PlayPlayerShieldOn(); // Play player shield on sound
                        currentPodState = PodState.Regular; // Switch to regular state
                    }

                    break;
            }
        }

        private void SpawnGoodBullet() // Method to spawn GoodBullets 
        {
            if (timerSpawnBullet > 0) return; // If still on cooldown, don't spawn a bullet

            SoundBoard.PlayPlayerShoot(); // Play player shoot sound
            GoodBullet p = Instantiate(prefabGoodBullet, barrel.transform.position, Quaternion.identity); // Instantiate a bullet at the barrel of the pod
            p.InitBullet(transform.forward * 20); // Sest the velocity of the bullet

            timerSpawnBullet = 1 / roundsPerSecond; // Sets the cooldown before another bullet can spawn
        }

        private void AimAtMouse()
        {
            // make a ray and a plane:
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, transform.position);

            // does the ray hit the plane?
            if (plane.Raycast(ray, out float dis))
            {
                // find point where the ray hits the plane
                Vector3 hitPos = ray.GetPoint(dis);

                if (debugObject) debugObject.position = hitPos;

                Vector3 vectorToHitPos = hitPos - transform.position;

                float angle = Mathf.Atan2(vectorToHitPos.x, vectorToHitPos.z);

                angle /= Mathf.PI; // converts from "radians" to "half-circles"
                angle *= 180; // converts form "half-circles" to degrees

                transform.eulerAngles = new Vector3(0, angle, 0);
            }
        }

        public void FollowPlayer()
        {
            if (playerLocation != null) nav.SetDestination(playerLocation.position); // Gets the player's location and sets it as the pod's destination
        }
    }
}
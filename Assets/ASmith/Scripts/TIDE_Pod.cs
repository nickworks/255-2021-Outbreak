using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        PodState currentPodState = PodState.Regular;

        private float batteryMax = 100;
        private float batteryMin = 0;
        private float currBattery;
        private float rechargeTimer;
        private float timerSpawnBullet = 0;
        private float roundsPerSecond = 5;

        private bool batteryOn = false;

        public GoodBullet prefabGoodBullet;

        public GameObject player;

        public Transform barrel;

        public Transform debugObject;

        private Camera cam;

        public Image batteryFillImage;

        public Text batteryText;

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
                batteryText.text = (fillPercentage * 100).ToString("0") + "sec";
            }
        }

        void Start()
        {
            currBattery = batteryMax;

            cam = Camera.main;
        }

        void Update()
        {
            //FollowPlayer();
            AimAtMouse();

            //print("Follower Battery: " + currBattery);
            print("Follower State: " + currentPodState);

            if (timerSpawnBullet > 0) timerSpawnBullet -= Time.deltaTime;

            switch (currentPodState)
            {
                case PodState.Regular:
                    // Behavior
                    currBattery -= Time.deltaTime;

                    // Transition
                    if (Input.GetKeyDown("Fire1")) { currentPodState = PodState.Shooting; }
                    if (Input.GetKeyDown("FollowerPower")) { currentPodState = PodState.Charging; }
                    if (currBattery <= batteryMin) { currentPodState = PodState.Charging; }

                    break;

                case PodState.Shooting:
                    // Behavior
                    currBattery -= Time.deltaTime;
                    SpawnGoodBullet();

                    // Transition
                    if (!Input.GetKeyDown("Fire1")) { currentPodState = PodState.Regular; }
                    if (currBattery <= batteryMin) { currentPodState = PodState.Charging; }

                    break;

                case PodState.Charging:
                    // Behavior
                    currBattery++;

                    // Transition
                    if (Input.GetKeyDown("FollowerPower")) { currentPodState = PodState.Regular; }
                    if (currBattery >= batteryMax) { currentPodState = PodState.Off; }

                    break;

                case PodState.Off:
                    // Transition
                    if (Input.GetKeyDown("FollowerPower")) { currentPodState = PodState.Regular; }

                    break;
            }
        }

        private void SpawnGoodBullet()
        {
            if (timerSpawnBullet > 0) return;

            GoodBullet p = Instantiate(prefabGoodBullet, transform.position, Quaternion.identity);
            p.InitBullet(transform.forward * 20);

            timerSpawnBullet = 1 / roundsPerSecond;
            print("bullet spawned");
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

            //private void FollowPlayer()
            //{
            //    // TODO: Create logic to follow player
            //}
        }
    }
}
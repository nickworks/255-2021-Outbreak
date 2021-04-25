using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {

    public class HealthItem : MonoBehaviour {

        public float healthIncreaseAmt = 10;

        private Vector3 healthItemPos;

        public float rotationSpeed = 20;

        public float bounceAmt = 2;
        private float setAmountBounce;

        Transform parentObj;

        void Start() {
            healthItemPos = transform.localPosition;
            setAmountBounce = bounceAmt;
            parentObj = GetComponentInParent<Transform>();
        }


        void Update() {
            transform.Rotate(0, rotationSpeed, 0);

            if (transform.localPosition.y <= 0) {
                bounceAmt = setAmountBounce;
            }

            if (transform.localPosition.y >= .5f) {
                bounceAmt = -setAmountBounce;
            }

            healthItemPos.y += Mathf.Sin(Time.deltaTime * bounceAmt) * .05f;

            healthItemPos.y = Mathf.Clamp(healthItemPos.y, -1, 1);
            transform.localPosition += healthItemPos * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other) {
            HealthScript playerHealth = other.GetComponent<HealthScript>();
            if (playerHealth && other.tag == "Player") {
                playerHealth.HealingItemEffect(healthIncreaseAmt);
                Destroy(parentObj.gameObject);
            }
        }
    }
}
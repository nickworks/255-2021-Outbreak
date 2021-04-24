using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {

    public class HealthItem : MonoBehaviour {

        public int healthIncreaseAmt = 10;

        private Vector3 healthItemPos;

        public float rotationSpeed = 20;

        public float bounceAmt = 2;
        private float setAmountBounce;

        void Start() {
            healthItemPos = transform.localPosition;
            setAmountBounce = bounceAmt;
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
    }
}
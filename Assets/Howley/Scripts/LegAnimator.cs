using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Howley
{
    public class LegAnimator : MonoBehaviour
    {
        /// <summary>
        /// This variable is applied to time to set leg rotation apart.
        /// </summary>
        public float reverse = 0;

        private Vector3 startingPos;
        private Vector3 targetPos;

        private Quaternion startingRot;
        private Quaternion targetRot;

        BossStates boss;

        void Start()
        {
            // Set the position and rotation to where they begin
            startingPos = transform.localPosition;
            startingRot = transform.localRotation;

            // Hold reference to the BossStates class
            boss = GetComponentInParent<BossStates>();
        }

        void AnimateWalking()
        {
            Vector3 finalPos = startingPos;

            // Move the final position
            float time = (Time.deltaTime + reverse) * boss.moveSpeed;

            float legRotation = Mathf.Sin(time);
            finalPos += boss.moveDir * legRotation * boss.stepLength.z;

            finalPos.y += Mathf.Cos(time) * boss.stepLength.y;

            bool isGrounded = (finalPos.y < startingPos.y);
            if (isGrounded) finalPos.y = startingPos.y;

            float percent = 1 - Mathf.Abs(legRotation);
            float pitch = isGrounded ? 0 : -percent * 20;

            transform.localPosition = finalPos;

            targetRot = transform.parent.rotation * startingRot * Quaternion.Euler(0, 0, pitch);

            targetPos = transform.TransformPoint(finalPos);
        }
    }

}

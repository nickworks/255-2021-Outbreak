using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public static class AnimMath
    {

        /// <summary>
        /// This function lerps between two floats.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="p"></param>
        /// <param name="allowExtrapolation"></param>
        /// <returns></returns>
        public static float Lerp(float min, float max, float p, bool allowExtrapolation = true)
        {
            if (!allowExtrapolation)
            {
                if (p < 0) p = 0;
                if (p > 1) p = 1;
            }
            return (max - min) * p + min;
        }

        /// <summary>
        /// This function lerps between two vector3's
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="p"></param>
        /// <param name="allowExtrapolation"></param>
        /// <returns></returns>
        public static Vector3 Lerp(Vector3 min, Vector3 max, float p, bool allowExtrapolation = true)
        {
            if (!allowExtrapolation)
            {
                if (p < 0) p = 0;
                if (p > 1) p = 1;
            }
            return (max - min) * p + min;
        }

        /// <summary>
        /// This function lerps between two quaternions.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="p"></param>
        /// <param name="allowExtrapolation"></param>
        /// <returns></returns>
        public static Quaternion Lerp(Quaternion min, Quaternion max, float p, bool allowExtrapolation = true)
        {
            //Quaternion rot = Quaternion.identity; // A quaternion with no rotation value

            // X, Y, Z, represent the vector direction, W represents the roll.
            //rot.x = Lerp(min.x, max.x, p, allowExtrapolation);
            //rot.y = Lerp(min.y, max.y, p, allowExtrapolation);
            //rot.z = Lerp(min.z, max.z, p, allowExtrapolation);
            //rot.w = Lerp(min.w, max.w, p, allowExtrapolation);

            // Spherical Lerp
            //Quaternion.Slerp(min, max, p);

            // Unity's built in Quaternion lerp.
            Quaternion rot = Quaternion.Lerp(min, max, p);

            return rot;
        }

        /// <summary>
        /// This function eases to a target with two floats.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="percentLeftAfter1Second"></param>
        /// <returns></returns>
        public static float Slide(float current, float target, float percentLeftAfter1Second)
        {
            float p = 1 - Mathf.Pow(percentLeftAfter1Second, Time.deltaTime);
            return AnimMath.Lerp(current, target, p);
        }

        /// <summary>
        /// This function eases between two Vector3's.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="percentLeftAfter1Second"></param>
        /// <returns></returns>
        public static Vector3 Slide(Vector3 current, Vector3 target, float percentLeftAfter1Second)
        {
            float p = 1 - Mathf.Pow(percentLeftAfter1Second, Time.deltaTime);
            return AnimMath.Lerp(current, target, p);
        }

        /// <summary>
        /// This function eases between two quaternions.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="percentLeftAfter1Second"></param>
        /// <returns></returns>
        public static Quaternion Slide(Quaternion current, Quaternion target, float percentLeftAfter1Second = .05f)
        {
            float p = 1 - Mathf.Pow(percentLeftAfter1Second, Time.deltaTime);
            return AnimMath.Lerp(current, target, p);
        }

    }



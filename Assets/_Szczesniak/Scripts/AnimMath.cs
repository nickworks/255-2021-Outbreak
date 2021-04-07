using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Math that is used for our mechanics
/// </summary>
/// 
namespace Szczesniak {
    /// <summary>
    /// Calculates Lerp and Slide
    /// </summary>
    public static class AnimMath {
        /// <summary>
        /// Calculates the Lerp
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="p"></param>
        /// <param name="allowExtrapolation"></param>
        /// <returns></returns>
        public static float Lerp(float min, float max, float p, bool allowExtrapolation = true) {
            if (!allowExtrapolation) {
                if (p < 0) p = 0;
                if (p > 1) p = 1;
            }

            return (max - min) * p + min;
        }

        public static Vector3 Lerp(Vector3 min, Vector3 max, float p, bool allowExtrapolation = true) {
            if (!allowExtrapolation) {
                if (p < 0) p = 0;
                if (p > 1) p = 1;
            }

            return (max - min) * p + min;

            //float x = Lerp(min.x, max.x, p);
            //float y = Lerp(min.y, max.y, p);
            //float z = Lerp(min.z, max.z, p);
            //return new Vector3(x, y, z);
        }


        public static Quaternion Lerp(Quaternion min, Quaternion max, float p, bool allowExtrapolation = true) {

            if (!allowExtrapolation) {
                if (p < 0) p = 0;
                if (p > 1) p = 1;
            }

            return Quaternion.Lerp(min, max, p);
        }

        /// <summary>
        /// Calculates the slide to have a smooth effect
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="percentLeftAfter1Second"></param>
        /// <returns></returns>
        public static float Slide(float current, float target, float percentLeftAfter1Second) {
            float p = 1 - Mathf.Pow(percentLeftAfter1Second, Time.deltaTime);
            return AnimMath.Lerp(current, target, p);
        }

        public static Vector3 Slide(Vector3 current, Vector3 target, float percentLeftAfter1Second) {
            float p = 1 - Mathf.Pow(percentLeftAfter1Second, Time.deltaTime);
            return AnimMath.Lerp(current, target, p);
        }

        public static Quaternion Slide(Quaternion current, Quaternion target, float percentLeftAfter1Second = .05f) {

            float p = 1 - Mathf.Pow(percentLeftAfter1Second, Time.deltaTime);
            return AnimMath.Lerp(current, target, p);
        }


    }
}
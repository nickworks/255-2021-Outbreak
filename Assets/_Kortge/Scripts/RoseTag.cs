using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kortge
{
    /// <summary>
    /// Text that hovers over roses to hint the player that it is meant to be picked up.
    /// </summary>
    public class RoseTag : MonoBehaviour
    {
        /// <summary>
        /// The rose this tag is meant to follow.
        /// </summary>
        public Transform rose;

        // Update is called once per frame
        void Update()
        {
            if (rose == null) ColorText(Color.clear);
            else EnableTag();
        }

        /// <summary>
        /// Makes the text appear so that it can follow a rose.
        /// </summary>
        private void EnableTag()
        {
            ColorText(Color.blue);
            FollowRose();
        }

        /// <summary>
        /// Sets the location of the text to be the same as the rose.
        /// </summary>
        private void FollowRose()
        {
            Vector3 newPosition;
            newPosition.x = rose.position.x;
            newPosition.x = Mathf.Clamp(newPosition.x, -5f, 5f);
            newPosition.y = rose.position.z;
            newPosition.y = Mathf.Clamp(newPosition.y, -5f, 2.5f);
            newPosition.z = 0;
            RectTransform position = GetComponent<RectTransform>();
            position.anchoredPosition = newPosition;
        }

        /// <summary>
        /// Changes the color of the text.
        /// </summary>
        /// <param name="color"></param>
        private void ColorText(Color color)
        {
            Text text = GetComponent<Text>();
            color.a /= 2;
            text.color = color;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    /// <summary>
    /// Damaging clones the boss leaves behind while dashing.
    /// </summary>
    public class AfterImage : MonoBehaviour
    {
        /// <summary>
        /// The amount of time this afterImage has been active.
        /// </summary>
        private float activeTime;
        /// <summary>
        /// The amount that this afterImage has faded.
        /// </summary>
        private float fade;
        /// <summary>
        /// The sprite used to represent the afterImage.
        /// </summary>
        private SpriteRenderer sprite;
        /// <summary>
        /// How long it takes for the afterimage to be destroyed.
        /// </summary>
        public float destroyTime;

        // Start is called before the first frame update
        /// <summary>
        /// Gets the sprite component in the child object.
        /// </summary>
        void Start()
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
        }

        // Update is called once per frame
        /// <summary>
        /// Fades the sprite out until it is completely transparent, then destroys itself.
        /// </summary>
        void Update()
        {
            activeTime += Time.deltaTime;
            fade = activeTime / destroyTime;
            sprite.color = Color.Lerp(Color.red, Color.clear, fade);
            if (fade >= 1) Destroy(gameObject);
        }
    }
}
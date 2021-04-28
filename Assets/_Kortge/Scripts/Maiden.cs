using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    /// <summary>
    /// Throws a rose power-up when signaled to.
    /// </summary>
    public class Maiden : MonoBehaviour
    {
        /// <summary>
        /// The rose that was thrown.
        /// </summary>
        private Rigidbody thrownRose;
        /// <summary>
        /// The prefab representing the rose.
        /// </summary>
        public Rigidbody rosePrefab;
        /// <summary>
        /// The tag meant to follow the rose.
        /// </summary>
        public RoseTag roseTag;
        /// <summary>
        /// Instantiates a rose and launches it into the arena.
        /// </summary>
        public void ThrowRose()
        {
            thrownRose = Instantiate(rosePrefab, transform.position + (transform.right * Random.Range(-4, 4)), transform.rotation);
            thrownRose.AddForce(thrownRose.transform.forward * Random.Range(1, 10), ForceMode.Impulse);
            roseTag.rose = thrownRose.transform;
        }
    }
}
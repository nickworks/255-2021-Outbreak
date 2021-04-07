using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kortge
{
    /// <summary>
    /// Contains information for the zone to display on the main menu.
    /// </summary>
    public class Zone : MonoBehaviour
    {
        static public ZoneInfo info = new ZoneInfo()
        {
            zoneName = "The Duel of the Phantoms",
            creator = "Tyler Kortge",
            sceneFile = "_KortgeScene"
        };
    }
}
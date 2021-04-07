using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Szczesniak {
    /// <summary>
    /// This class is for setting up you game's name, who created it, and the scene name.
    /// </summary>
    public class Zone : Outbreak.Zone {

        /// <summary>
        /// information on the game
        /// </summary>
        new static public ZoneInfo info = new ZoneInfo() {
            zoneName = "Retro Warfare",
            creator = "Gavin Szczesniak",
            sceneFile = "Szczesniak"
        };
    }
}
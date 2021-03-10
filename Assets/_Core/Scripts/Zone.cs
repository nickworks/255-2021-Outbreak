using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Meta information about a zone in Outbreak.
/// </summary>
public struct ZoneInfo {
    /// <summary>
    /// What is your zone called? This is how it will appear in the game.
    /// </summary>
    public string zoneName;
    /// <summary>
    /// What is your name? This is how you will be credited in the game.
    /// </summary>
    public string creator;
    /// <summary>
    /// That is the scene filename of your level?
    /// </summary>
    public string sceneFile;
}
namespace Outbreak {
    public class Zone : MonoBehaviour {
        /// <summary>
        /// Meta-info about this zone.
        /// </summary>
        static public ZoneInfo info = new ZoneInfo() {
            zoneName = "",
            creator = "",
            sceneFile = ""
        };
        
    } // end class
} // end namespace
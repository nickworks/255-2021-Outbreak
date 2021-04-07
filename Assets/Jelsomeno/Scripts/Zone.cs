using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Jelsomeno
{
    /// <summary>
    /// this class is meant to get access to the main menu
    /// </summary>
    public class Zone : Outbreak.Zone
    {

        new static public ZoneInfo info = new ZoneInfo()
        {
            zoneName = "Tank Buster",
            creator = "Rocco Jelsomeno",
            sceneFile = "Jelsomeno"
        };

    }

}

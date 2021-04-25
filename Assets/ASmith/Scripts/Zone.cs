using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith
{
    public class Zone : Outbreak.Zone
    {
        new static public ZoneInfo info = new ZoneInfo()
        {
            zoneName = "Triple B's Lair", // The name of the level
            creator = "Antonio Smith", // The creator of the level
            sceneFile = "ASmithLevel" // The file name for the level
        };
    }
}
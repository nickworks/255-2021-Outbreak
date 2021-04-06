using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith
{
    public class Zone : Outbreak.Zone
    {
        new static public ZoneInfo info = new ZoneInfo()
        {
            zoneName = "Triple B's Lair",
            creator = "Antonio Smith",
            sceneFile = "ASmithLevel"
        };
    }
}
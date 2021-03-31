using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASmith
{
    public class Zone : Outbreak.Zone
    {
        new static public ZoneInfo info = new ZoneInfo()
        {
            zoneName = "Mystic Trials",
            creator = "Antonio Smith",
            sceneFile = "ASmithLevel"
        };
    }
}
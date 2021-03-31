using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hodgkins
{
    public class Zone : Outbreak.Zone
    {
        new static public ZoneInfo info = new ZoneInfo()
        {
            zoneName = "Tom's Zone",
            creator = "Tom Hodgkins",
            sceneFile = "HodgkinsLevel"
        };

    }
}
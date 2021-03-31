using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foster
{
    public class Zone : Outbreak.Zone 
    {
       new static public ZoneInfo info = new ZoneInfo()
        {
            zoneName = "",
            creator = "Emily Foster",
            sceneFile = "Foster_Scene"
        };
    }
}
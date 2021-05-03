using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Velting
{
    public class Zone : Outbreak.Zone
    {
        new static public ZoneInfo info = new ZoneInfo()
        {
            zoneName = "Corona Attack Much Bad",
            creator = "William Velting",
            sceneFile = "VeltingScene"
        };

    }
}

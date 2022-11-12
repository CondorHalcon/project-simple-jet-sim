using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarSight : Target
{
    public override void Start() {
        base.Start();
        weaponTrigger = true;
    }
}

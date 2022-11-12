using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RadarSignature
{
    public Vector3 position;
    public float age;

    public RadarSignature(Vector3 p, float a = 3f) {
        position = p;
        age = a;
    }
}

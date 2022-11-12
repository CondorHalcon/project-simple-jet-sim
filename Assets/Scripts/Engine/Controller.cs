using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Aircraft plane;

    void Start() {
        if (plane != null) { plane.gameObject.SendMessage("SetPilot", this); }
    }
}

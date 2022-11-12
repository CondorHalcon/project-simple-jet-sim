using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject projectile;
    public bool fire = false;
    public float rpm = 1200;
    public float velocity = 1000;
    public float fireRange = 1000;

    private float coolDownTime = 1;
    private float coolDown;

    public void Start() {
        coolDownTime = 1f / (rpm / 60f);
        coolDown = coolDownTime;
    }

    public void FixedUpdate() {
        coolDown -= Time.fixedDeltaTime;
        if (fire && coolDown <= 0) {
            coolDown = coolDownTime;
            // fire weapon
        }
    }
}

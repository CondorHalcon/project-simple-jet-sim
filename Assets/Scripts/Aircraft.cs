using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class Aircraft : Target
{
    public Controller pilot { get; private set; }
    [Header("")]
    public string designation = "Fighter";

    new public Rigidbody rigidbody { get; private set; }
    private static Vector3 aerofoil = new Vector3(1, 36, .5f);

    public Rail[] rails = new Rail[6];

    [Header("Physics"), Tooltip("-in tonne(s)")]
    public float mass = 11;
    public Transform centerOfMass;

    [Header("Parts")]
    public Transform engineNozzle;
    public Transform rightAileron;
    public Transform leftAileron;
    public Transform rightVertStable;
    public Transform leftVertStable;
    public Transform horStable;
    public Transform rudder;
    public WheelCollider steerWheel;
    public WheelCollider rightWheel;
    public WheelCollider leftWheel;

    [Header("Specs")] 
    public float health;
    public float surfaceMaxAngle = 15;
    public float fightAngle = 5;
    [Tooltip("-in kNm")] public float thrust = 130;
    public float wheelTorque = 100;
    public Vector3 dragArea = new Vector3(8, 8, 3);
    public Vector3 dragCoeffitient = new Vector3(.5f, .5f, .15f);
    public Vector3 wingArea = new Vector3(1, 28, 1);
    public Vector3 horStableArea = new Vector3(5, 0, 0);
    public Vector3 aileronArea = new Vector3(0, .5f, 0);
    public Vector3 vertStableArea = new Vector3(0, 1, 0);
    public Vector3 rudderArea = new Vector3(1, 0, 0);

    // inputs
    [HideInInspector] public float gas;
    [HideInInspector] public float pitch;
    [HideInInspector] public float yaw;
    [HideInInspector] public float roll;
    [HideInInspector] public bool fire;

    public override void Start() {
        base.Start();
        weaponTrigger = true;

        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = centerOfMass.localPosition;
        rigidbody.useGravity = false;
        rigidbody.mass = mass * 1000; // local mass is in tonnes

        WheelForce(wheelTorque, 0);

        StartCoroutine(Radar());
    }

    void Update() {
        // clear old radar signatures
        RadarUpdate();

        // rotate control surfaces
        engineNozzle.localEulerAngles = new Vector3(fightAngle, 0, 0);

        rightVertStable.localEulerAngles = new Vector3((pitch * surfaceMaxAngle) + fightAngle, 0, 0);
        leftVertStable.localEulerAngles = new Vector3((pitch * surfaceMaxAngle) + fightAngle, 0, 0);

        rudder.localEulerAngles = new Vector3(0, -yaw * surfaceMaxAngle , 0);
        steerWheel.steerAngle = yaw * surfaceMaxAngle;

        rightAileron.localEulerAngles = new Vector3(roll * surfaceMaxAngle, 0, 0);
        leftAileron.localEulerAngles = new Vector3(-roll * surfaceMaxAngle, 0, 0);
    }

    void FixedUpdate() {
        WheelForce(wheelTorque * gas, 0);

        Vector3 drag = Library.GetForce(transform.InverseTransformDirection(rigidbody.velocity), dragArea, dragCoeffitient);
        Vector3 lift = Library.GetForce(transform.InverseTransformDirection(rigidbody.velocity), wingArea, aerofoil);
        Vector3 gravity = transform.InverseTransformDirection(new Vector3(0, -9.81f * mass * 1000, 0));

        float rightVertStableTorque = (rightVertStable.localPosition.z - centerOfMass.localPosition.z) * Library.GetForce(rightVertStable.InverseTransformDirection(rigidbody.velocity), vertStableArea, aerofoil).y;
        float leftVertStableTorque = (leftVertStable.localPosition.z - centerOfMass.localPosition.z) * Library.GetForce(leftVertStable.InverseTransformDirection(rigidbody.velocity), vertStableArea, aerofoil).y;
        float rudderTorque = rudder.localPosition.z * Library.GetForce(rudder.InverseTransformDirection(rigidbody.velocity), rudderArea, aerofoil).x * -1;
        float rightAileronTorque = rightAileron.localPosition.x * Library.GetForce(rightAileron.InverseTransformDirection(rigidbody.velocity), aileronArea, aerofoil).y * -1;
        float leftAileronTorque = leftAileron.localPosition.x * Library.GetForce(leftAileron.InverseTransformDirection(rigidbody.velocity), aileronArea, aerofoil).y * -1;

        float horStableTorque = horStable.localPosition.z * Library.GetForce(horStable.InverseTransformDirection(rigidbody.velocity), horStableArea, aerofoil).x * -1;
        

        Vector3 centerForce = drag + lift + gravity + (Vector3.forward * thrust * 1000 * gas);
        rigidbody.AddRelativeForce(centerForce);
        //rigidbody.AddForceAtPosition(engineNozzle.forward * thrust * 1000 * gas, engineNozzle.position); // thrust is in kNm
        Vector3 torque = new Vector3((rightVertStableTorque + leftVertStableTorque), rudderTorque + horStableTorque, rightAileronTorque + leftAileronTorque);
        rigidbody.AddRelativeTorque(-torque);
    }

    #region Class Methods

    public void SetPilot(Controller _pilot = null) {
        pilot = _pilot;
    }
    void WheelForce(float torque, float brake) {
        if (steerWheel != null) {
            steerWheel.brakeTorque = brake;
            steerWheel.motorTorque = torque;
        }
        if (rightWheel != null) {
            rightWheel.brakeTorque = brake;
            rightWheel.motorTorque = torque;
        }
        if (leftWheel != null) {
            leftWheel.brakeTorque = brake;
            leftWheel.motorTorque = torque;
        }
    }

    #endregion
}

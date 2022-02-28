using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class Aircraft : MonoBehaviour
{
    public string designation = "Fighter";

    new Rigidbody rigidbody;
    private static Vector3 aerofoil = new Vector3(1, 36, .5f);

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
    [Header("Inputs")]
    /*[HideInInspector]*/ public float gas;
    /*[HideInInspector]*/ public float pitch;
    /*[HideInInspector]*/ public float yaw;
    /*[HideInInspector]*/ public float roll;
    /*[HideInInspector]*/ public bool fire;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = centerOfMass.localPosition;
        rigidbody.useGravity = false;
        rigidbody.mass = mass * 1000; // local mass is in tonnes

        WheelForce(wheelTorque, 0);
    }

    void Update() {
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

        Vector3 drag = GetForce(transform.InverseTransformDirection(rigidbody.velocity), dragArea, dragCoeffitient);
        Vector3 lift = GetForce(transform.InverseTransformDirection(rigidbody.velocity), wingArea, aerofoil);
        Vector3 gravity = transform.InverseTransformDirection(new Vector3(0, -9.81f * mass * 1000, 0));

        float rightVertStableTorque = (rightVertStable.localPosition.z - centerOfMass.localPosition.z) * GetForce(rightVertStable.InverseTransformDirection(rigidbody.velocity), vertStableArea, aerofoil).y;
        float leftVertStableTorque = (leftVertStable.localPosition.z - centerOfMass.localPosition.z) * GetForce(leftVertStable.InverseTransformDirection(rigidbody.velocity), vertStableArea, aerofoil).y;
        float rudderTorque = rudder.localPosition.z * GetForce(rudder.InverseTransformDirection(rigidbody.velocity), rudderArea, aerofoil).x * -1;
        float rightAileronTorque = rightAileron.localPosition.x * GetForce(rightAileron.InverseTransformDirection(rigidbody.velocity), aileronArea, aerofoil).y * -1;
        float leftAileronTorque = leftAileron.localPosition.x * GetForce(leftAileron.InverseTransformDirection(rigidbody.velocity), aileronArea, aerofoil).y * -1;

        float horStableTorque = horStable.localPosition.z * GetForce(horStable.InverseTransformDirection(rigidbody.velocity), horStableArea, aerofoil).x * -1;
        

        Vector3 centerForce = drag + lift + gravity + (Vector3.forward * thrust * 1000 * gas);
        rigidbody.AddRelativeForce(centerForce);
        //rigidbody.AddForceAtPosition(engineNozzle.forward * thrust * 1000 * gas, engineNozzle.position); // thrust is in kNm
        Vector3 torque = new Vector3((rightVertStableTorque + leftVertStableTorque), rudderTorque + horStableTorque, rightAileronTorque + leftAileronTorque);
        rigidbody.AddRelativeTorque(-torque);
    }

    #region Class Methods

    Vector3 GetForce(Vector3 relativeVelocity, Vector3 area, Vector3 coeffitients) { //L=Ci * ((r * V^2)/2) * A
        Vector3 forceVectors = new Vector3(
            coeffitients.x * ((relativeVelocity.x * relativeVelocity.x * ((relativeVelocity.x>0)?-1:1))/2) * area.x,
            coeffitients.y * ((relativeVelocity.y * relativeVelocity.y * ((relativeVelocity.y>0)?-1:1))/2) * area.y,
            coeffitients.z * ((relativeVelocity.z * relativeVelocity.z * ((relativeVelocity.z>0)?-1:1))/2) * area.z
        );
        return forceVectors;
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

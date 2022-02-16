using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class Aircraft : MonoBehaviour
{
    public string designation = "Fighter";

    new Rigidbody rigidbody;

    [Header("Physics"), Tooltip("-in tonne(s)")]
    public float mass = 11;
    public Transform centerOfMass;

    [Header("Parts")]
    public Transform engineNozzle;
    public Transform rightAileron;
    public Transform leftAileron;
    public Transform rightVertStable;
    public Transform leftVertStable;
    public Transform rudder;

    [Header("Specs")] 
    public float health;
    public float surfaceMaxAngle = 15;
    [Tooltip("-in kNm")] public float thrust = 130;
    public Vector3 dragArea = new Vector3(8, 8, 3);
    public Vector3 dragCoeffitient = new Vector3(.5f, .5f, .15f);
    public Vector3 wingArea = new Vector3(1, 28, 1);
    public Vector3 horStableArea = new Vector3(10, 0, 0);
    public Vector3 aileronArea = new Vector3(0, 5, 0);
    public Vector3 vertStableArea = new Vector3(0, 5, 0);
    public Vector3 rudderArea = new Vector3(5, 0, 0);

    // inputs
    /*[HideInInspector]*/ public float gas;
    /*[HideInInspector]*/ public float pitch;
    /*[HideInInspector]*/ public float yaw;
    /*[HideInInspector]*/ public float roll;
    /*[HideInInspector]*/ public bool fire;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.mass = mass * 1000;
        if (centerOfMass != null) { rigidbody.centerOfMass = centerOfMass.localPosition; }
    }

    void Update() {
        rightAileron.localEulerAngles = new Vector3(surfaceMaxAngle * roll, 0, 0);
        leftAileron.localEulerAngles = new Vector3(surfaceMaxAngle * -roll, 0, 0);
        Vector3 vertStableAngle = new Vector3(surfaceMaxAngle * pitch + .1f, 0, 0);
        rightVertStable.localEulerAngles = vertStableAngle;
        leftVertStable.localEulerAngles = vertStableAngle;
        rudder.localEulerAngles = new Vector3(0, surfaceMaxAngle * yaw, 0);
    }

    void FixedUpdate() {
        Vector3 thrustForce = new Vector3(0, 0, Mathf.Abs(thrust) * 1000 * gas);
        Vector3 dragForce = GetForce(transform.InverseTransformDirection(rigidbody.velocity), dragArea, dragCoeffitient);
        Vector3 wingForce = GetForce(transform.InverseTransformDirection(rigidbody.velocity), wingArea, Vector3.one);
        Vector3 horStableForce = GetForce(transform.InverseTransformDirection(rigidbody.velocity), horStableArea, Vector3.one);
        Vector3 aileronForceR = GetForce(rightAileron.InverseTransformDirection(rigidbody.velocity), aileronArea, Vector3.one);
        Vector3 aileronForceL = GetForce(leftAileron.InverseTransformDirection(rigidbody.velocity), aileronArea, Vector3.one);
        Vector3 vertStableForce = GetForce(rightVertStable.InverseTransformDirection(rigidbody.velocity), vertStableArea, Vector3.one);
        Vector3 rudderForce = GetForce(rudder.InverseTransformDirection(rigidbody.velocity), rudderArea, Vector3.one);

        rigidbody.AddForce(transform.TransformDirection(wingForce));
        rigidbody.AddForceAtPosition(engineNozzle.TransformDirection(thrustForce), engineNozzle.position);
        Vector3 tailForces = horStableForce + vertStableForce + rudderForce;
        Vector3 tailPosition = new Vector3(0, 0, rudder.localPosition.z);
        rigidbody.AddForceAtPosition(transform.TransformDirection(tailForces), tailPosition);
        rigidbody.AddForceAtPosition(aileronForceR, rightAileron.position);
        rigidbody.AddForceAtPosition(aileronForceL, leftAileron.position);
    }

    #region Class Methods

    Vector3 GetForce(Vector3 relativeVelocity, Vector3 area, Vector3 coeffitients) { //L=Ci * ((r * V^2)/2) * A
        Vector3 forceVectors = new Vector3(
            coeffitients.x * ((relativeVelocity.x * relativeVelocity.x * -1)/2) * area.x,
            coeffitients.y * ((relativeVelocity.y * relativeVelocity.y * -1)/2) * area.y,
            coeffitients.z * ((relativeVelocity.z * relativeVelocity.z * -1)/2) * area.z
        );
        return forceVectors;
    }

    #endregion
}
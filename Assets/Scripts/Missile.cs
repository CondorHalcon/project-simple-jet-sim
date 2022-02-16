using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class Missile : MonoBehaviour
{
    public string designation = "Fox Two Missile";
    public MissileType type = MissileType.FoxTwo;
    public ParticleSystem emitor;
    public Transform target;

    new Rigidbody rigidbody;
    bool isFired = true;

    [Header("Physics")]
    public float mass = 100;
    public float thrust;
    public float dragArea = .1f;
    public float wingArea = 1;

    [Header("Timeings")]
    public float igniteWait = 0;
    public float thrustTime = 5;
    public float noTargetTime = 5;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        /*if (transform.root == transform) {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        } else { rigidbody.constraints  = RigidbodyConstraints.None; }*/
        rigidbody.mass = mass;
    }

    void FixedUpdate() {
        Rigidbody r = transform.root.GetComponent<Rigidbody>();

        Vector3 relativeVelocity = transform.InverseTransformDirection(r.velocity);
        Vector3 aeroForce = new Vector3(
            ((relativeVelocity.x * relativeVelocity.x * -1)/2) * wingArea/2,
            ((relativeVelocity.y * relativeVelocity.y * -1)/2) * wingArea/2,
            ((relativeVelocity.z * relativeVelocity.z * -1)/2) * dragArea
        );
        Vector3 thrustForce = new Vector3(0, 0, (isFired) ? thrust : 0);

        r.AddForce(transform.TransformDirection(aeroForce + thrustForce));

        TrackTarget();
    }

    #region Class Region

    public void Fire() {
        transform.SetParent(transform);
        rigidbody.constraints  = RigidbodyConstraints.None;
        isFired = true;
        if (emitor != null) { /*emitor.emission.enabled = true;*/ }
    }
    void TrackTarget() {
        if (target != null) { transform.LookAt(target); }
    }

    #endregion
}
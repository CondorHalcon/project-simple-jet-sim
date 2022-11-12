using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class Missile : Target
{
    public string designation = "Fox Two Missile";
    public MissileType type = MissileType.FoxTwo;
    public ParticleSystem emitor;
    public Transform target;

    new Rigidbody rigidbody;
    bool isFired = false;

    [Header("Physics")]
    public float mass = 100;
    public float thrust;
    public Vector3 dragArea = new Vector3(5, 5, 1);
    public float wingArea = 1;

    [Header("Timeings")]
    public float igniteWait = 0;
    public float thrustTime = 5;
    public float noTargetTime = 5;

    public override void Start() {
        base.Start();

        rigidbody = GetComponent<Rigidbody>();
        /*if (transform.root == transform) {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        } else { rigidbody.constraints  = RigidbodyConstraints.None; }*/
        rigidbody.mass = mass;
    }

    void FixedUpdate() {
        Rigidbody r = transform.root.GetComponent<Rigidbody>();

        Vector3 relativeVelocity = transform.InverseTransformDirection(r.velocity);
        Vector3 aeroForce = Library.GetForce(relativeVelocity, dragArea, new Vector3(1, 1, .2f));
        Vector3 thrustForce = new Vector3(0, 0, (isFired) ? thrust : 0);

        r.AddRelativeForce(aeroForce + thrustForce);

        TrackTarget();
    }

    #region Class Region

    public void Fire(float intitalVelocity) {
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
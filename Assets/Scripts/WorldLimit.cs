using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WorldLimit : MonoBehaviour
{
    public enum LimitType {Wall, Roof, Floor}

    public LimitType type = LimitType.Wall;
    new BoxCollider collider;

    void Start() {
        collider = GetComponent<BoxCollider>();
        if (type == LimitType.Wall) { collider.isTrigger = true; }
    }

    void OnTriggerEnter(Collider other) {
        if (other.attachedRigidbody != null) {
            other.transform.root.LookAt(other.transform.root.position + (other.transform.root.forward * -5));
            other.attachedRigidbody.velocity *= -1;
        } else { Destroy(other); }
    }
}
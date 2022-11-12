using UnityEngine;

public class CamFollow : MonoBehaviour {
    public Transform target;
    public Vector3 targetPos = new Vector3(0, 5, -10);
    public Vector3 minPos = new Vector3(0, 3, -20);
    public Vector3 maxPos = new Vector3(0, 10, -5);
    public float lookAheadDistance = 10;
    public float smoothTime = .3f;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate() {
        // position above and behind the target transform (global space)
        Vector3 deltaTargetPos = target.TransformPoint(targetPos);
        // position to smoothly move the camera towards that target position (target local space)
        Vector3 position = target.InverseTransformPoint(Vector3.SmoothDamp(transform.position, deltaTargetPos, ref velocity, smoothTime));
        // clamp axis (target local space)
        position.x = 0;
        position.y = Mathf.Clamp(position.y, minPos.y, maxPos.y);
        position.z = Mathf.Clamp(position.z, minPos.z, maxPos.z);
        // apply position (global space)
        transform.position = target.TransformPoint(position);

        // look at a positon in front of the target
        transform.LookAt(target.position + target.forward * lookAheadDistance, target.up);
    }
}
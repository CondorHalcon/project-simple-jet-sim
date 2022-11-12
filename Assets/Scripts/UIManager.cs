using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Aircraft plane;

    [Header("Flight")]
    public Text speedText;
    public Text thrustText;
    public Text altitudeText;

    [Header("Radar")]
    public Transform radarObjectParent;
    public GameObject pingPrefab;
    public GameObject findPrefab;

    void Update() {
        if (plane == null) { return; }

        float speed = plane.transform.InverseTransformDirection(plane.rigidbody.velocity).z;
        speedText.text = $"{Mathf.RoundToInt(speed*3.6f).ToString()}km/h\n{(speed*3.6f/1225).ToString("F2")}mach";

        float alt = plane.transform.position.y;
        altitudeText.text = $"{Mathf.RoundToInt(alt).ToString()}m\n{Mathf.RoundToInt(alt*3.28084f).ToString()}ft";
        
        thrustText.text = $"{(plane.thrust * plane.gas).ToString("F2")}kNm";

        int x = radarObjectParent.childCount;
        for (int i = 1; i < x+1; i++) { Destroy(radarObjectParent.GetChild(x-i).gameObject); }
        foreach (RadarSignature item in plane.radarPings) {
            Vector3 dirToSignature = (plane.transform.position - item.position).normalized;
            float angle = Vector3.SignedAngle(-plane.transform.forward, dirToSignature, Vector3.up);
            
            GameObject obj = Instantiate(pingPrefab, radarObjectParent);
            obj.transform.localPosition = new Vector3(Mathf.Clamp(angle/plane.radarFOV, -1, 1)*150,150,0);
        }
        foreach (RadarSignature item in plane.radarFinds) {
            Vector3 v = (plane.transform.position - item.position); v.y *= 0;
            float dst = v.magnitude;
            Vector3 dirToSignature = (plane.transform.position - item.position).normalized;
            float angle = Vector3.SignedAngle(-plane.transform.forward, dirToSignature, Vector3.up);
            
            GameObject obj = Instantiate(findPrefab, radarObjectParent);
            obj.transform.localPosition = new Vector3(Mathf.Clamp(angle/plane.radarFOV, -1, 1)*150,Mathf.Clamp(dst/plane.radarRange, 0, 1)*300,0);
        }
    }
}
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Aircraft plane;
    public Rigidbody rb;

    [Header("Outputs")]
    public Text speedText;
    public Text thrustText;

    void Update() {
        Transform t = rb.GetComponent<Transform>();
        float speed = t.InverseTransformDirection(rb.velocity).z;
        speedText.text = $"{Mathf.RoundToInt(speed*3.6f).ToString()}km/h\n{(speed*3.6f/1225).ToString("F2")}mach";
        
        thrustText.text = $"{(plane.thrust * plane.gas).ToString("F2")}kNm";
    }
}
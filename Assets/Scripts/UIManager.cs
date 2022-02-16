using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Rigidbody rb;
    public Text txt;

    void Update() {
        txt.text = $"{(rb.velocity.magnitude*3.6f).ToString()}";
    }
}
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Aircraft aircraft;

    void Update() {
        aircraft.gas = Mathf.Clamp(aircraft.gas + Input.GetAxis("Thrust"), 0, 1);
        aircraft.pitch = Input.GetAxis("Vertical");
        aircraft.roll = Input.GetAxis("Horizontal");
    }
}
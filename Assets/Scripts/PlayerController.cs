using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Aircraft aircraft;

    void Update() {
        aircraft.gas = Mathf.Clamp(aircraft.gas + Input.GetAxis("Thrust"), 0, 1);
        aircraft.gas = Mathf.Clamp(aircraft.gas + (Input.GetAxis("Thrust2") * Time.deltaTime * 1), 0, 1);
        aircraft.pitch = Input.GetAxis("Vertical");
        aircraft.roll = Input.GetAxis("Horizontal");
        aircraft.yaw = Input.GetAxis("HorizontalTwo");

        if (Input.GetButtonDown("Submit")) { GameManager.instance.Reset(); }
        if (Input.GetButtonDown("Cancel")) { GameManager.instance.Quit(); }
    }
}
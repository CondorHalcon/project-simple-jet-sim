using UnityEngine;

public class PlayerController : Controller
{
    void Update() {
        plane.gas = Mathf.Clamp(plane.gas + Input.GetAxis("Thrust"), 0, 1);
        plane.gas = Mathf.Clamp(plane.gas + (Input.GetAxis("Thrust2") * Time.deltaTime * 1), 0, 1);
        plane.pitch = Input.GetAxis("Vertical");
        plane.roll = Input.GetAxis("Horizontal");
        plane.yaw = Input.GetAxis("HorizontalTwo");

        if (Input.GetButtonDown("Submit")) { GameManager.instance.Reset(); }
        if (Input.GetButtonDown("Cancel")) { GameManager.instance.Quit(); }
    }
}
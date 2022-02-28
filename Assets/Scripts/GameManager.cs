using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;
    void Awake() {
        GameManager.instance = this;
    }

    #endregion

    public Transform spawner;
    public Aircraft plane;

    void Start() {
        Reset();
    }

    public void Reset() {
        Rigidbody rb = plane.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        plane.gas = 0;
        plane.transform.position = spawner.position;
        plane.transform.rotation = spawner.rotation;
    }
    public void Quit() {
        Debug.Log("GameManager.instance.Quit() :: Closing application.", this);
        Application.Quit();
    }
}
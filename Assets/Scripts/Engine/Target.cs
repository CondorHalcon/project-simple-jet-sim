using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Target Info")] 
    public bool weaponTrigger = false;
    [Range(.01f, 1)] public float radarCross = 1;
    [Range(-20,200)] public float thermalSignature = 20;

    [Header("Radar")]
    public float radarRange = 0f;
    [Range(1,360)] public int radarFOV = 90;
    public float radarStrength = 1f;
    public List<RadarSignature> radarFinds { get; private set; }
    public List<RadarSignature> radarPings { get; private set; }

    public virtual void Start() {
        radarFinds = new List<RadarSignature>();
        radarPings = new List<RadarSignature>();
    }

    public void RadarPing(RadarSignature s) {
        Vector3 dirToSignature = (transform.position - s.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToSignature);

        foreach (RadarSignature rs in radarPings) {
            Vector3 dirToOldSignature = (transform.position - rs.position).normalized;
            float oldAngle = Vector3.Angle(transform.forward, dirToOldSignature);

            if (Mathf.Abs(angle - oldAngle) < 1) { rs.age = 3; return; }
        }

        radarPings.Add(s);
    }

    public virtual void RadarUpdate() {
        foreach (RadarSignature rs in radarPings) {
            rs.age -= Time.deltaTime;
            if (rs.age <= 0) { radarPings.Remove(rs); }
        }
        foreach (RadarSignature rs in radarFinds) {
            rs.age -= Time.deltaTime;
            if (rs.age <= 0) { radarFinds.Remove(rs); }
        }
    }
    public virtual void DoDamage(float value) {}

    #region Coroutines

    public IEnumerator Radar() {
        while (true) {
            List<RadarSignature> signatures = Library.GetRadarSignatures(transform, radarRange, radarFOV, GameManager.instance.targetMask, radarStrength);

            foreach (RadarSignature rs in signatures) {
                bool found = false;
                foreach (RadarSignature ors in radarFinds)
                {
                    if (Vector3.Distance(rs.position, ors.position) < 10) { 
                        ors.position = rs.position;
                        ors.age = 3f;
                        found = true;
                    }
                }
                if (!found) { radarFinds.Add(rs); }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    #endregion
}

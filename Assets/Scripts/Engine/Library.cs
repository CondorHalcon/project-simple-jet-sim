using UnityEngine;
using System.Collections.Generic;

public class Library
{
    public static Vector3 GetForce(Vector3 relativeVelocity, Vector3 area, Vector3 coeffitients) { //L=Ci * ((r * V^2)/2) * A
        Vector3 forceVectors = new Vector3(
            coeffitients.x * ((relativeVelocity.x * relativeVelocity.x * ((relativeVelocity.x>0)?-1:1))/2) * area.x,
            coeffitients.y * ((relativeVelocity.y * relativeVelocity.y * ((relativeVelocity.y>0)?-1:1))/2) * area.y,
            coeffitients.z * ((relativeVelocity.z * relativeVelocity.z * ((relativeVelocity.z>0)?-1:1))/2) * area.z
        );
        return forceVectors;
    }

    public static List<RadarSignature> GetRadarSignatures(Transform source, float range, int fov, LayerMask mask, float strength = 1f, bool msg = true) {
        List<RadarSignature> t = new List<RadarSignature>();
        Collider[] inRange = Physics.OverlapSphere(source.position, range, mask);

        for (int i = 0; i < inRange.Length; i++) {
            Transform targetT = inRange[i].transform.root;
            float dstToTarget = Vector3.Distance(source.position, targetT.position);
            if (dstToTarget < 1) { continue; }
            Vector3 dirToTarget = (targetT.position - source.position).normalized;
            if (Vector3.Angle(source.forward, dirToTarget) > fov/2) { continue; }
            if (Physics.Raycast(source.position, dirToTarget, dstToTarget - 3f, Physics.AllLayers)) { continue; }

            Target target = targetT.GetComponent<Target>();
            if (msg) { target.gameObject.SendMessage("RadarPing", new RadarSignature(source.position)); }
            float signal = dstToTarget / (target.radarCross * strength) * .00025f;
            if (signal > 1) { continue; } // WARNING: Unintuitive.

            t.Add(new RadarSignature(target.transform.position));
        }

        return t;
    }
}

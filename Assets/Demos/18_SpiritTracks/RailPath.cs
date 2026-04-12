using UnityEngine;
using System.Collections.Generic;

public class RailPath : MonoBehaviour
{
    public List<Transform> nodes = new List<Transform>();
    public bool isLoop = false;
    public int resolution = 10; // For visual smoothness in editor

    // The Magic Formula: Catmull-Rom Spline
    public Vector3 GetPointOnCurve(int segmentIndex, float t)
    {
        int count = nodes.Count;
        if (count < 2) return Vector3.zero;

        // Get the 4 points needed for the curve (p0, p1, p2, p3)
        // p1 and p2 are the start/end of the current segment
        Transform p0 = nodes[Mathf.Clamp(segmentIndex - 1, 0, count - 1)];
        Transform p1 = nodes[segmentIndex];
        Transform p2 = nodes[Mathf.Clamp(segmentIndex + 1, 0, count - 1)];
        Transform p3 = nodes[Mathf.Clamp(segmentIndex + 2, 0, count - 1)];

        // Catmull-Rom Math
        return 0.5f * (
            (2f * p1.position) +
            (-p0.position + p2.position) * t +
            (2f * p0.position - 5f * p1.position + 4f * p2.position - p3.position) * t * t +
            (-p0.position + 3f * p1.position - 3f * p2.position + p3.position) * t * t * t
        );
    }

    private void OnDrawGizmos()
    {
        if (nodes.Count < 2) return;
        Gizmos.color = Color.cyan;

        for (int i = 0; i < nodes.Count - 1; i++)
        {
            Vector3 lastPoint = nodes[i].position;
            for (int j = 1; j <= resolution; j++)
            {
                float t = j / (float)resolution;
                Vector3 nextPoint = GetPointOnCurve(i, t);
                Gizmos.DrawLine(lastPoint, nextPoint);
                lastPoint = nextPoint;
            }
        }
    }
}
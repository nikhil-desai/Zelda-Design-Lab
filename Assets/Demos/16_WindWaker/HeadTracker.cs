using UnityEngine;
using System.Linq;

public class HeadTracker : MonoBehaviour
{
    [Header("Detection")]
    public float visionRadius = 8f;
    public float visionAngle = 90f;
    public LayerMask poiLayer;

    [Header("Ref (The Head Bone)")]
    public Transform headBone; // If no rig, use the Visual Child itself
    
    private Transform _targetPOI;
    private Quaternion _originalRotation;

    void LateUpdate()
    {
        FindClosestPOI();

        if (_targetPOI != null)
        {
            LookAtTarget();
        }
        else
        {
            ResetRotation();
        }
    }

    void FindClosestPOI()
    {
        // 1. Get all potential hits
        Collider[] hits = Physics.OverlapSphere(transform.position, visionRadius, poiLayer);
        
        if (hits.Length == 0)
        {
            _targetPOI = null;
            return;
        }

        // 2. Filter by Vision Cone and map to a sorting-friendly list
        var validPOIs = hits
            .Where(h => IsInVisionCone(h.transform))
            .Select(h => new {
                Transform = h.transform,
                // If the object doesn't have POIProperty, treat priority as 0
                Priority = h.GetComponent<POIProperty>()?.priority ?? 0,
                Distance = Vector3.Distance(transform.position, h.transform.position)
            })
            .ToList();

        if (validPOIs.Count == 0)
        {
            _targetPOI = null;
            return;
        }

        // 3. SORTING LOGIC: 
        // First: Look at the highest Priority.
        // Second: If priorities are equal, look at the closest one.
        var bestTarget = validPOIs
            .OrderByDescending(p => p.Priority)
            .ThenBy(p => p.Distance)
            .First();

        _targetPOI = bestTarget.Transform;
    }

    bool IsInVisionCone(Transform target)
    {
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToTarget);
        return angle < (visionAngle / 2f);
    }

    void LookAtTarget()
    {
        Vector3 targetDir = _targetPOI.position - headBone.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        
        // Smoothly rotate the head towards the target
        headBone.rotation = Quaternion.Slerp(headBone.rotation, targetRotation, Time.deltaTime * 5f);
    }

    void ResetRotation()
    {
        // Smoothly return to looking forward (relative to the body)
        headBone.localRotation = Quaternion.Slerp(headBone.localRotation, Quaternion.identity, Time.deltaTime * 5f);
    }
}
using UnityEngine;

public class SideScrollCamera : MonoBehaviour
{
    [Header("Tracking")]
    public Transform target;
    public float smoothness = 5f;

    [Header("Position Properties")]
    // Using your specific good-looking coordinates
    public float fixedZ = -10f;
    public float idealY = 6.67f;
    public float offsetX = 0f;

    [Header("Constraints")]
    public float minYThreshold = 6.67f; // The "Floor" for the camera

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Calculate the raw desired position
        float targetX = target.position.x + offsetX;
        float targetY = target.position.y + (idealY - target.position.y); 
        
        // Actually, for a clean side-scroller, we usually want to follow the player's 
        // X but keep the Y dampened or clamped.
        
        float goalX = target.position.x + offsetX;
        float goalY = Mathf.Max(target.position.y, minYThreshold);

        // 2. Linear Interpolation for smoothness
        Vector3 currentPos = transform.position;
        Vector3 nextPos = new Vector3(goalX, goalY, fixedZ);
        
        transform.position = Vector3.Lerp(currentPos, nextPos, Time.deltaTime * smoothness);

        // 3. Final Correction: Ensure rotation never drifts from the side-view
        transform.rotation = Quaternion.identity;
    }
}
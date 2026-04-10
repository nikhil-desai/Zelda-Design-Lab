using UnityEngine;

public class MinishCamera : MonoBehaviour
{
    [Header("Targeting")]
    public Transform target;       // Drag Player_Root here
    public GameObject visualModel; // Drag the 'Visuals' child here

    // Hard-coded Zelda values (Forces the camera to behave)
    private readonly Vector3 _baseOffset = new Vector3(0, 14, -10); 
    private float _smoothTime = 0.05f; // Very fast follow to stop the "top of screen" lag
    private Vector3 _currentVelocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null || visualModel == null) return;

        // 1. Get the current scale (1.0 to 0.2)
        float scale = visualModel.transform.localScale.x;

        // 2. Calculate the zoomed offset
        // When scale is 1.0, multiplier is 1.0. When scale is 0.2, multiplier is 0.3.
        float zoomMultiplier = Mathf.Lerp(0.3f, 1.0f, (scale - 0.2f) / 0.8f);
        Vector3 targetOffset = _baseOffset * zoomMultiplier;

        // 3. Calculate Target Position
        Vector3 targetPosition = target.position + targetOffset;

        // 4. Move Camera (SmoothDamp is better than Lerp for high speeds)
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, _smoothTime);

        // 5. THE FIX FOR 90 DEGREES: Force the camera to look at the player
        // We look at the player's position + a small height offset based on scale
        transform.LookAt(target.position + (Vector3.up * scale));
    }
}
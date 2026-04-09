using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Drag your Player object here
    public float smoothSpeed = 10f; // How "snappy" the camera is

    void LateUpdate()
    {
        if (target != null)
        {
            // We only copy the position, NOT the rotation
            transform.position = Vector3.Lerp(transform.position, target.position, smoothSpeed * Time.deltaTime);
        }
    }
}
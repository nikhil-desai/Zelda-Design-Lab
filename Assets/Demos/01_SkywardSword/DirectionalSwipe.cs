using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionalSwipe : MonoBehaviour
{
    public float minSwipeDistance = 50f; // Minimum distance to count as a "slash"
    
    private Vector2 _startPos;
    private bool _isSwiping = false;

    public System.Action<Vector2> OnSlashDetected; // Event to tell the sword where to go

    void Update()
    {
        // We'll use Right-Click (or a specific key) to initiate a slash
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            _startPos = Mouse.current.position.ReadValue();
            _isSwiping = true;
        }

        if (_isSwiping && Mouse.current.rightButton.wasReleasedThisFrame)
        {
            Vector2 endPos = Mouse.current.position.ReadValue();
            Vector2 swipeVector = endPos - _startPos;

            if (swipeVector.magnitude > minSwipeDistance)
            {
                OnSlashDetected?.Invoke(swipeVector.normalized);
            }
            _isSwiping = false;
        }
    }
}
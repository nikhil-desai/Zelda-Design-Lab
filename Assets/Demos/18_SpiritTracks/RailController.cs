using UnityEngine;
using UnityEngine.InputSystem;

public class RailController : MonoBehaviour
{
    public RailPath currentPath;
    public float moveSpeed = 7f;
    public bool isOnRail = false;

    private CharacterController _controller;
    private TopDownController _baseController;
    
    private int _currentSegment = 0;
    private float _segmentProgress = 0f; // 0 to 1 along the current segment

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _baseController = GetComponent<TopDownController>();
    }

    void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame) isOnRail = !isOnRail;

        if (isOnRail && currentPath != null)
        {
            _baseController.canMove = false;
            HandleSplineMovement();
        }
        else
        {
            _baseController.canMove = true;
        }
    }

    void HandleSplineMovement()
    {
        float input = 0;
        if (Keyboard.current.wKey.isPressed) input = 1;
        if (Keyboard.current.sKey.isPressed) input = -1;

        if (Mathf.Abs(input) > 0.01f)
        {
            // 1. Calculate how much 't' to add based on world speed
            // We divide by distance to keep speed constant even on long/short segments
            float segmentLength = Vector3.Distance(currentPath.nodes[_currentSegment].position, 
                                                   currentPath.nodes[Mathf.Clamp(_currentSegment + 1, 0, currentPath.nodes.Count-1)].position);
            
            float speedModifier = input * (moveSpeed / segmentLength) * Time.deltaTime;
            _segmentProgress += speedModifier;

            // 2. Handle Segment Switching
            if (_segmentProgress > 1f)
            {
                if (_currentSegment < currentPath.nodes.Count - 2)
                {
                    _currentSegment++;
                    _segmentProgress = 0f;
                }
                else _segmentProgress = 1f; // End of line
            }
            else if (_segmentProgress < 0f)
            {
                if (_currentSegment > 0)
                {
                    _currentSegment--;
                    _segmentProgress = 1f;
                }
                else _segmentProgress = 0f; // Start of line
            }

            // 3. Set Position
            Vector3 nextPos = currentPath.GetPointOnCurve(_currentSegment, _segmentProgress);
            
            // Look Ahead for rotation (to face where we're going)
            Vector3 lookAheadPos = currentPath.GetPointOnCurve(_currentSegment, Mathf.Clamp01(_segmentProgress + 0.01f * input));
            Vector3 moveDir = (lookAheadPos - transform.position).normalized;

            if (moveDir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * 10f);
            }

            // Warp to the spline point (using CharacterController.Move would cause jitter here)
            transform.position = nextPos;
        }
    }
}
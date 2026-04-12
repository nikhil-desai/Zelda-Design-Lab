using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class SkywardSwordController : MonoBehaviour
{
    [Header("Hierarchy")]
    public Transform handPivot; // The empty parent at (1.5, 2, 0)
    
    [Header("Sword Stats")]
    public float swingSmoothness = 20f;
    public float lungeDistance = 0.8f;
    public float minSwipeDistance = 50f;

    // Your "Source of Truth" Base Pose
    private Vector3 _handHomePos = new Vector3(1.5f, 2f, 0f);
    private Vector3 _handHomeRot = new Vector3(45f, 45f, 160f);

    private Vector2 _swipeStart;
    private bool _isSwiping = false;
    private bool _isSlashing = false;
    private Quaternion _idleRotation;
    private Quaternion _targetRotation;

    void Start()
    {
        _idleRotation = Quaternion.Euler(_handHomeRot);
        _targetRotation = _idleRotation;
        handPivot.localPosition = _handHomePos;
    }

    void Update()
    {
        HandleInput();
        // Smoothly return to the hand's natural pose
        handPivot.localRotation = Quaternion.Slerp(handPivot.localRotation, _targetRotation, Time.deltaTime * swingSmoothness);
    }

    private void HandleInput()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            _swipeStart = Mouse.current.position.ReadValue();
            _isSwiping = true;
        }

        if (_isSwiping && Mouse.current.rightButton.wasReleasedThisFrame)
        {
            Vector2 swipeEnd = Mouse.current.position.ReadValue();
            Vector2 dir = swipeEnd - _swipeStart;

            if (dir.magnitude > minSwipeDistance)
            {
                StartCoroutine(ExecuteSlash(dir.normalized));
            }
            _isSwiping = false;
        }
    }

    IEnumerator ExecuteSlash(Vector2 swipeDir)
    {
        if (_isSlashing) yield break;
        _isSlashing = true;

        // 1. Calculate the rotation angle of the swipe
        float swipeAngle = Mathf.Atan2(swipeDir.y, swipeDir.x) * Mathf.Rad2Deg;
        _targetRotation = _idleRotation * Quaternion.Euler(0, 0, swipeAngle);

        // 2. Perform the physical lunge
        Vector3 impactPos = _handHomePos + (handPivot.forward * lungeDistance);
        
        // 3. DETECTION: Procedural Slicing Plane
        // We define a plane where the 'Normal' is the side of the blade
        RaycastHit hit;
        if (Physics.SphereCast(transform.position + Vector3.up, 0.5f, transform.forward, out hit, 2.5f))
        {
            var cuttable = hit.collider.GetComponent<ICuttable>();
            if (cuttable != null)
            {
                // The Normal of the cut is the Sword's "Up" or "Right"
                // This ensures the cut is aligned with the blade's flat edge
                cuttable.OnCut(hit.point, handPivot.up);
            }
        }

        // Move hand forward
        handPivot.localPosition = impactPos;
        yield return new WaitForSeconds(0.15f);

        // Reset
        handPivot.localPosition = _handHomePos;
        _targetRotation = _idleRotation;
        _isSlashing = false;
    }
}

// Modular Interface
public interface ICuttable
{
    void OnCut(Vector3 hitPoint, Vector3 normal);
}
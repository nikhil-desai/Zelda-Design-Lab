using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class ZTargeter : MonoBehaviour
{
    [Header("Detection Settings")]
    public float lockRange = 12f;
    public LayerMask targetLayer;
    public string enemyTag = "Enemy";

    [Header("References")]
    public GameObject reticlePrefab;
    
    // Internal State
    private Transform _currentTarget;
    private GameObject _activeReticle;
    private CharacterController _characterController;
    private TopDownController _baseController;

    public bool IsLocked => _currentTarget != null;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _baseController = GetComponent<TopDownController>();
    }

    void Update()
    {
        // 1. TOGGLE LOCK (Using 'F' - Respecting the No-R Rule)
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (IsLocked) ClearTarget();
            else AttemptLock();
        }

        // 2. STATE OVERRIDE
        if (IsLocked)
        {
            // Stop the base controller from doing its own movement/rotation
            _baseController.canMove = false;
            
            HandleLockOnLogic();
        }
        else
        {
            // Ensure the base controller is allowed to move when not locked
            _baseController.canMove = true;
        }
    }

    private void AttemptLock()
    {
        // Find all colliders in range
        Collider[] hits = Physics.OverlapSphere(transform.position, lockRange, targetLayer);
        
        if (hits.Length == 0) return;

        // Pick the closest target
        _currentTarget = hits
            .OrderBy(h => Vector3.Distance(transform.position, h.transform.position))
            .First().transform;

        // Visual Feedback: Reticle
        if (reticlePrefab) 
        {
            _activeReticle = Instantiate(reticlePrefab);
        }

        // CAMERA HACK: Swap tag to "Player" so ZeldaUniversalCamera includes it in the group
        if (_currentTarget.CompareTag(enemyTag))
        {
            _currentTarget.tag = "Player";
        }
    }

    private void ClearTarget()
    {
        if (_currentTarget != null && _currentTarget.CompareTag("Player"))
        {
            _currentTarget.tag = enemyTag; // Revert tag
        }

        if (_activeReticle) Destroy(_activeReticle);
        _currentTarget = null;
    }

    private void HandleLockOnLogic()
    {
        // A. ROTATION: Always face the target (Y-axis only)
        Vector3 dirToTarget = (_currentTarget.position - transform.position).normalized;
        dirToTarget.y = 0;
        Quaternion lookRot = Quaternion.LookRotation(dirToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 10f);

        // B. STRAFE MOVEMENT: Re-calculate inputs for relative movement
        Vector2 input = GetInput();
        
        // Move relative to Player's Forward/Right (which is now locked to the enemy)
        Vector3 moveDir = (transform.forward * input.y + transform.right * input.x).normalized;

        if (moveDir.magnitude >= 0.1f)
        {
            _characterController.Move(moveDir * _baseController.moveSpeed * Time.deltaTime);
        }

        // C. RETICLE POSITION
        if (_activeReticle)
        {
            _activeReticle.transform.position = _currentTarget.position + Vector3.up * 1.5f;
            _activeReticle.transform.LookAt(Camera.main.transform);
        }

        // D. DISTANCE CHECK (Auto-break if too far)
        if (Vector3.Distance(transform.position, _currentTarget.position) > lockRange + 2f)
        {
            ClearTarget();
        }
    }

    // Helper to read keyboard without polluting logic
    private Vector2 GetInput()
    {
        Vector2 input = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) input.y = 1;
        if (Keyboard.current.sKey.isPressed) input.y = -1;
        if (Keyboard.current.aKey.isPressed) input.x = -1;
        if (Keyboard.current.dKey.isPressed) input.x = 1;
        return input.normalized;
    }
}
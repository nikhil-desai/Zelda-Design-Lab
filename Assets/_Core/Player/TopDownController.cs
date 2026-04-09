using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(CharacterController))]
public class TopDownController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float turnSpeed = 720f;

    [Header("Stuck Detection")]
    public bool isStuck = false;
    public float checkRadius = 0.45f; // Slightly smaller than capsule radius (0.5)
    public Vector3 checkOffset = new Vector3(0, 1, 0); // Check from the middle of the capsule
    public LayerMask obstacleLayers; // Set this to "Default" in the Inspector

    private CharacterController _controller;
    private Vector2 _inputVector;
    private Renderer _renderer;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _renderer = GetComponent<Renderer>();
        
        // Default to checking everything if no layer is selected
        if (obstacleLayers == 0) obstacleLayers = ~0; 
    }

    void Update()
    {
        // --- 1. THE STUCK CHECK ---
        // We check a sphere at the player's position + offset to see if it overlaps a collider
        isStuck = Physics.CheckSphere(transform.position + checkOffset, checkRadius, obstacleLayers);

        if (isStuck)
        {
            // Visual feedback: Turn Red when trapped in a wall
            _renderer.material.color = Color.red;
            
            // We stop the function here so no movement can happen
            return; 
        }
        else
        {
            _renderer.material.color = Color.white;
        }

        // --- 2. INPUT HANDLING ---
        if (Keyboard.current != null)
        {
            Vector2 move = Vector2.zero;
            if (Keyboard.current.wKey.isPressed) move.y = 1;
            if (Keyboard.current.sKey.isPressed) move.y = -1;
            if (Keyboard.current.aKey.isPressed) move.x = -1;
            if (Keyboard.current.dKey.isPressed) move.x = 1;
            _inputVector = move.normalized;
        }

        // --- 3. WORLD-SPACE MOVEMENT ---
        // This vector ignores the player's rotation. 
        // W/S affects Z (Forward/Back), A/D affects X (Left/Right)
        Vector3 moveDirection = new Vector3(_inputVector.x, 0, _inputVector.y);

        if (moveDirection.magnitude >= 0.1f)
        {
            // Move the character relative to the WORLD
            _controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Rotate the character to FACE the direction they are moving
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        // --- 4. GRAVITY ---
        if (!_controller.isGrounded)
        {
            _controller.Move(Vector3.down * 9.81f * Time.deltaTime);
        }
    }

    // This helps you see the "Stuck Check" sphere in the Scene view while you aren't playing
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + checkOffset, checkRadius);
    }
}
using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(CharacterController))]
public class TopDownController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float turnSpeed = 720f;
    
    // This allows other scripts to stop the player
    public bool canMove = true;

    private CharacterController _controller;
    private Vector2 _inputVector;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. INPUT HANDLING
        if (Keyboard.current != null)
        {
            Vector2 move = Vector2.zero;
            if (Keyboard.current.wKey.isPressed) move.y = 1;
            if (Keyboard.current.sKey.isPressed) move.y = -1;
            if (Keyboard.current.aKey.isPressed) move.x = -1;
            if (Keyboard.current.dKey.isPressed) move.x = 1;
            _inputVector = move.normalized;
        }

        // 2. WORLD-SPACE MOVEMENT
        Vector3 moveDirection = new Vector3(_inputVector.x, 0, _inputVector.y);

        // Only move if the "canMove" toggle is on
        if (canMove && moveDirection.magnitude >= 0.1f)
        {
            _controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        // 3. GRAVITY (Keep this outside canMove so they still fall if paralyzed)
        if (!_controller.isGrounded)
        {
            _controller.Move(Vector3.down * 9.81f * Time.deltaTime);
        }
    }
}
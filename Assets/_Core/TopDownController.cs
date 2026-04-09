using UnityEngine;
// 1. Add this line at the top!
using UnityEngine.InputSystem; 

[RequireComponent(typeof(CharacterController))]
public class TopDownController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 720f;

    private CharacterController _controller;
    private Vector2 _inputVector; // New Input System uses Vector2 for sticks/WASD

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 2. The New Way to get WASD/Stick input
        // This looks for any keyboard or gamepad input automatically
        if (Keyboard.current != null)
        {
            Vector2 move = Vector2.zero;
            if (Keyboard.current.wKey.isPressed) move.y = 1;
            if (Keyboard.current.sKey.isPressed) move.y = -1;
            if (Keyboard.current.aKey.isPressed) move.x = -1;
            if (Keyboard.current.dKey.isPressed) move.x = 1;
            _inputVector = move.normalized;
        }

        // 3. Convert that Vector2 (X, Y) to a Vector3 (X, 0, Z) for 3D movement
        Vector3 moveDirection = new Vector3(_inputVector.x, 0, _inputVector.y);

        if (moveDirection.magnitude >= 0.1f)
        {
            _controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        if (!_controller.isGrounded)
        {
            _controller.Move(Vector3.down * 9.81f * Time.deltaTime);
        }
    }
}
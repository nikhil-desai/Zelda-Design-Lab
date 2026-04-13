using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SideScrollController : MonoBehaviour
{
    [Header("References")]
    public MonoBehaviour topDownController; // The original script to disable
    
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    public LayerMask groundLayer;
    
    private Rigidbody rb;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Lock rotation and Z-axis movement for 2D Side-scrolling
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        
        if (topDownController != null) topDownController.enabled = false;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        Move(h);

        if (Input.GetButtonDown("Jump") && CheckGrounded())
        {
            Jump();
        }
    }

    void Move(float dir)
    {
        // We preserve velocity.y to allow for natural gravity falls
        rb.linearVelocity = new Vector3(dir * moveSpeed, rb.linearVelocity.y, 0);
        
        // Flip Sprite/Model based on direction
        if (dir != 0) transform.forward = new Vector3(dir, 0, 0);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, 0); // Reset vertical momentum
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool CheckGrounded()
    {
        // Adjust 1.1f based on your player's height. 
        // If your player is 2 units tall, and the pivot is in the center, 
        // you need at least 1.05f to reach the floor.
        float rayLength = 1.1f; 
        
        bool hit = Physics.Raycast(transform.position, Vector3.down, rayLength, groundLayer);
        
        // VISUAL DEBUG: This draws a red/green line in the Scene View
        Debug.DrawRay(transform.position, Vector3.down * rayLength, hit ? Color.green : Color.red);
        
        return hit;
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemManager : MonoBehaviour
{
    [Header("References")]
    public GameObject bombPrefab;
    public Transform holdPoint;

    private CharacterController _characterController;

    [Header("Throw Settings")]
    public float throwForwardForce = 8f;
    public float throwUpwardForce = 3f;

    private GameObject _heldBomb;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            if (_heldBomb == null)
            {
                PickUpBomb();
            }
            else
            {
                ThrowBomb();
            }
        }
    }

    void PickUpBomb()
    {
        // 1. Spawn the bomb at the hold point
        _heldBomb = Instantiate(bombPrefab, holdPoint.position, holdPoint.rotation);

        // 2. Parent it so it moves with the player
        _heldBomb.transform.SetParent(holdPoint);

        // 3. Disable Physics while held so it doesn't jitter or push the player
        Rigidbody rb = _heldBomb.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; 
        }

        Collider col = _heldBomb.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false; // Prevent clipping while held
        }
        
        // Note: The BombLogic's fuse starts ticking the moment it's Instantiated!
    }

    void ThrowBomb()
    {
        if (_heldBomb == null) return;

        // 1. Unparent immediately
        _heldBomb.transform.SetParent(null);

        // 2. Grab the physics components
        Rigidbody rb = _heldBomb.GetComponent<Rigidbody>();
        Collider col = _heldBomb.GetComponent<Collider>();

        if (rb != null)
        {
            // IMPORTANT: Re-enable physics and gravity
            rb.isKinematic = false;
            rb.useGravity = true;
            if (col != null) col.enabled = true;

            // 3. MOMENTUM MATH
            // Get the player's actual world velocity from the CharacterController
            Vector3 playerVelocity = _characterController.velocity;
            
            // Calculate the base throw (Direction * Strength + Upward Arc)
            Vector3 throwVector = (transform.forward * throwForwardForce) + (transform.up * throwUpwardForce);

            // Final Force = Base Throw + Player's Current Momentum
            Vector3 finalForce = throwVector + playerVelocity;

            // 4. FIRE!
            rb.AddForce(finalForce, ForceMode.Impulse);
        }

        _heldBomb = null;
    }
    // Safety check: If the bomb explodes in our hands, clear the reference
    public void NotifyBombExploded()
    {
        _heldBomb = null;
    }
}
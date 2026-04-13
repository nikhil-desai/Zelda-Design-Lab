using UnityEngine;
using System.Collections.Generic;

public class CombatFriction : MonoBehaviour
{
    private Rigidbody rb;
    public float recoilStrength = 15f;
    public float frictionDamping = 5f;

    void Awake() => rb = GetComponent<Rigidbody>();

    // This is called by an external Hitbox script
    public void ApplyRecoil(Vector3 sourcePosition)
    {
        // Calculate 1D direction (Left or Right only)
        float recoilDir = transform.position.x > sourcePosition.x ? 1f : -1f;
        
        // Apply a sudden burst of force
        Vector3 force = new Vector3(recoilDir * recoilStrength, 2f, 0); 
        rb.linearVelocity = Vector3.zero; // Clear current movement for "stutter" effect
        rb.AddForce(force, ForceMode.Impulse);
        
        Debug.Log("<color=red>💥 Combat Friction:</color> Recoil applied to " + gameObject.name);
    }

    void FixedUpdate()
    {
        // Manual friction to prevent sliding on ice
        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(0, rb.linearVelocity.y, 0), Time.fixedDeltaTime * frictionDamping);
        }
    }
}
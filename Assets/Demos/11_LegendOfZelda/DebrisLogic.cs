using UnityEngine;

public class DebrisLogic : MonoBehaviour
{
    public float force = 5f;
    public float lifetime = 2f;

    void Start()
    {
        // Explode outward from the center
        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rbs)
        {
            Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f));
            rb.AddForce(randomDir * force, ForceMode.Impulse);
        }
        
        // Auto-cleanup the debris after 2 seconds
        Destroy(gameObject, lifetime);
    }
}
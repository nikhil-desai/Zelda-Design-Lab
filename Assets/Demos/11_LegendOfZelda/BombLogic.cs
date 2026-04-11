using UnityEngine;

public class BombLogic : MonoBehaviour
{
    public float fuseTime = 2.0f;
    public float explosionRadius = 3.0f;
    public GameObject explosionVisual;

    void Start()
    {
        Invoke("Explode", fuseTime);
    }

    void Explode()
    {

        // Find the player and tell them the bomb is gone
        ItemManager manager = FindFirstObjectByType<ItemManager>();
        if (manager != null) manager.NotifyBombExploded();

        if (explosionVisual) Instantiate(explosionVisual, transform.position, Quaternion.identity);

        // Check for SecretReceivers in the blast zone
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            SecretReceiver receiver = hit.GetComponent<SecretReceiver>();
            if (receiver != null)
            {
                receiver.ReceiveInteraction(InteractionType.Bomb);
            }
        }

        Destroy(gameObject); // The bomb is spent
    }
}
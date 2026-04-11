using UnityEngine;

public class BombLogic : MonoBehaviour
{
    public float fuseTime = 2.0f;
    public float explosionRadius = 3.0f;
    public GameObject explosionVisual;
    public AudioClip explosionSound; // Drag your sound here!

    void Start()
    {
        Invoke("Explode", fuseTime);
    }

    void Explode()
    {
        // 1. Notify the Player
        ItemManager manager = FindFirstObjectByType<ItemManager>();
        if (manager != null) manager.NotifyBombExploded();

        // 2. Play Sound (PlayClipAtPoint handles its own cleanup)
        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        }

        // 3. Visuals
        if (explosionVisual) Instantiate(explosionVisual, transform.position, Quaternion.identity);

        // 4. Secret Logic
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            SecretReceiver receiver = hit.GetComponent<SecretReceiver>();
            if (receiver != null)
            {
                receiver.ReceiveInteraction(InteractionType.Bomb);
            }
        }

        Destroy(gameObject); 
    }
}
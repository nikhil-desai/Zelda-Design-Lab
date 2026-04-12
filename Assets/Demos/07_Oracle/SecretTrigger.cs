using UnityEngine;

public class SecretTrigger : MonoBehaviour
{
    public string secretID; // Unique name for this secret (e.g., "Forest_Key")
    public SecretDatabase database;
    public GameObject visualFeedback; // e.g., an icon that disappears when learned

    private void Start()
    {
        // On scene load, if the secret is already known, hide the trigger
        if (database.IsSecretUnlocked(secretID))
        {
            gameObject.SetActive(false);
        }
    }

    public void UnlockSecret()
    {
        database.RegisterSecret(secretID);
        if (visualFeedback != null) visualFeedback.SetActive(false);
        
        // Logic for cross-scene transition could go here
        Debug.Log($"Secret {secretID} learned. This will now persist in Scene 2.");
    }

    // Triggered by your existing Player Interaction system
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UnlockSecret();
        }
    }
}
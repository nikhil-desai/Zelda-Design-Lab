using UnityEngine;
using UnityEngine.Events;

// Defined globally so other scripts (Bomb, Fire, Magic) can see it
public enum InteractionType { Sword, Fire, Bomb, Magic, Any }

public class SecretReceiver : MonoBehaviour
{
    [Header("Requirements")]
    public InteractionType requiredType = InteractionType.Bomb;

    [Header("Audio & Visuals")]
    [Tooltip("The iconic 'Discovery' chime.")]
    public AudioClip secretSound;
    public GameObject revealEffect; // Particle system prefab
    public GameObject debrisPrefab; // Physics chunks prefab

    [Header("Scene Integration")]
    [Tooltip("Use this to trigger other objects (e.g., Door.Open(), Bridge.Extend())")]
    public UnityEvent onSecretRevealed;

    private bool _isRevealed = false;

    /// <summary>
    /// Called by tools (Bombs, Torches, etc.) when they hit this object.
    /// </summary>
    public void ReceiveInteraction(InteractionType incomingType)
    {
        if (_isRevealed) return;

        // Check if the tool matches the requirement
        if (incomingType == requiredType || requiredType == InteractionType.Any)
        {
            Reveal();
        }
    }

    private void Reveal()
    {
        _isRevealed = true;

        // 1. Play the Audio Cue
        // PlayClipAtPoint creates a temporary object so the sound doesn't 
        // get cut off when this GameObject is disabled.
        if (secretSound != null)
        {
            AudioSource.PlayClipAtPoint(secretSound, transform.position);
        }

        // 2. Spawn Visuals
        if (revealEffect != null)
        {
            Instantiate(revealEffect, transform.position, Quaternion.identity);
        }

        // 3. Spawn Debris
        if (debrisPrefab != null)
        {
            Instantiate(debrisPrefab, transform.position, transform.rotation);
        }

        // 4. Invoke Global Events
        onSecretRevealed.Invoke();

        // 5. Deactivate the "Secret" container (the wall/bush)
        // We often use SetActive(false) so the 'Hidden' object behind it is revealed.
        gameObject.SetActive(false);
    }
}
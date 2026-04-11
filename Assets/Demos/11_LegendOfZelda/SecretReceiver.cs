using UnityEngine;
using UnityEngine.Events;

public enum InteractionType { Sword, Fire, Bomb, Any }

public class SecretReceiver : MonoBehaviour
{
    [Header("Requirements")]
    public InteractionType requiredType = InteractionType.Bomb;
    
    [Header("Effects")]
    public GameObject revealEffect; // Particles or dust
    public UnityEvent onSecretRevealed; // Drag & Drop events in Inspector!

    private bool _isRevealed = false;

    public void ReceiveInteraction(InteractionType incomingType)
    {
        if (_isRevealed) return;

        if (incomingType == requiredType || requiredType == InteractionType.Any)
        {
            Reveal();
        }
    }

    private void Reveal()
    {
        _isRevealed = true;
        Debug.Log("🎶 Secret Sound Effect Plays! 🎶");

        if (revealEffect) Instantiate(revealEffect, transform.position, Quaternion.identity);

        // This allows us to disable the wall, open a door, or play an animation
        onSecretRevealed.Invoke();
        
        // Classic Zelda behavior: The wall just disappears
        gameObject.SetActive(false); 
    }
}
using UnityEngine;

public class CoopTrigger : MonoBehaviour
{
    [Header("Security Settings")]
    public GameObject targetPlayer; // Drag the specific Link (Green or Red) here
    public Color activeColor = Color.yellow; // The color when the right player is on it

    public bool isPressed = false;
    private Color _originalColor;
    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null) _originalColor = _renderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only trigger if the object entering is the EXACT GameObject we assigned
        if (other.gameObject == targetPlayer)
        {
            isPressed = true;
            UpdateVisuals();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == targetPlayer)
        {
            isPressed = false;
            UpdateVisuals();
        }
    }

    void UpdateVisuals()
    {
        if (_renderer == null) return;
        _renderer.material.color = isPressed ? activeColor : _originalColor;
    }
}
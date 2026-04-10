using UnityEngine;

public class StuckDetector : MonoBehaviour
{
    [Header("Settings")]
    public float checkRadius = 0.45f;
    public Vector3 checkOffset = new Vector3(0, 1, 0);
    public LayerMask obstacleLayers;

    [Header("References")]
    public Renderer targetRenderer; // Drag your "Visuals" child here!

    private TopDownController _movement;
    private Color originalColor; 

    void Start()
    {
        _movement = GetComponent<TopDownController>();
        
        // If you forgot to drag the renderer in, try to find it automatically
        if (targetRenderer == null)
        {
            targetRenderer = GetComponentInChildren<Renderer>();
        }

        // Save original color from the CORRECT renderer
        if (targetRenderer != null) 
        {
            originalColor = targetRenderer.material.color;
        }
        
        if (obstacleLayers == 0) obstacleLayers = ~0; 
    }

    void Update()
    {
        bool isStuck = Physics.CheckSphere(transform.position + checkOffset, checkRadius, obstacleLayers);

        if (_movement != null)
        {
            _movement.canMove = !isStuck;
        }

        // Apply color to the CHILD'S renderer
        if (targetRenderer != null)
        {
            targetRenderer.material.color = isStuck ? Color.red : originalColor;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + checkOffset, checkRadius);
    }
}
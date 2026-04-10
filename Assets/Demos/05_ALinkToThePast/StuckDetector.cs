using UnityEngine;

public class StuckDetector : MonoBehaviour
{
    [Header("Settings")]
    public float checkRadius = 0.45f;
    public Vector3 checkOffset = new Vector3(0, 1, 0);
    public LayerMask obstacleLayers;

    private TopDownController _movement;
    private Renderer _renderer;
    private Color originalColor; 

    void Start()
    {
        _movement = GetComponent<TopDownController>();
        _renderer = GetComponent<Renderer>();
        
        // Save original color to prevent "Silver Link"
        if (_renderer != null) 
        {
            originalColor = _renderer.material.color;
        }
        
        // Default to checking everything if no layer is selected
        if (obstacleLayers == 0) obstacleLayers = ~0; 
    }

    void Update()
    {
        // 1. Physical overlap check
        bool isStuck = Physics.CheckSphere(transform.position + checkOffset, checkRadius, obstacleLayers);

        // 2. Enable/Disable movement script
        if (_movement != null)
        {
            _movement.canMove = !isStuck;
        }

        // 3. Visual feedback
        if (_renderer != null)
        {
            _renderer.material.color = isStuck ? Color.red : originalColor;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + checkOffset, checkRadius);
    }
}
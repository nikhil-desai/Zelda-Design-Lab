using UnityEngine;
using UnityEngine.InputSystem;

public class MinishAbility : MonoBehaviour
{
    [Header("Setup")]
    public GameObject visualModel; // Drag the 'Visuals' child capsule here
    public float smallScale = 0.2f;
    public float transitionSpeed = 5f;

    [Header("Resource Cost")]
    public float shrinkCost = 15f;

    private TopDownController _move;
    private CharacterController _controller;
    private bool _isSmall = false;
    private float _targetScale = 1.0f;
    private float _originalSpeed;

    void Start()
    {
        // Cache our components from the Root object
        _move = GetComponent<TopDownController>();
        _controller = GetComponent<CharacterController>();
        
        // Store the speed so we can restore it later
        if (_move != null) _originalSpeed = _move.moveSpeed;

        // Safety check for the visual model
        if (visualModel == null && transform.childCount > 0)
            visualModel = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        // 1. Listen for the 'F' key
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            TryToggleScale();
        }

        // 2. Smoothly calculate the new scale
        float currentS = visualModel.transform.localScale.x;
        float newS = Mathf.MoveTowards(currentS, _targetScale, Time.deltaTime * transitionSpeed);
        
        // 3. Apply the logic
        ApplyMinishLogic(newS);
    }

    private void TryToggleScale()
    {
        if (!_isSmall)
        {
            // Try to shrink (Costs Magic)
            if (MagicManager.Instance != null && MagicManager.Instance.UseMagic(shrinkCost))
            {
                _isSmall = true;
                _targetScale = smallScale;
            }
            else if (MagicManager.Instance == null)
            {
                Debug.LogWarning("No MagicManager found in scene! Shrinking anyway for demo.");
                _isSmall = true;
                _targetScale = smallScale;
            }
        }
        else
        {
            // Grow back (Free)
            _isSmall = false;
            _targetScale = 1.0f;
        }
    }

    private void ApplyMinishLogic(float currentScale)
    {
        if (visualModel == null || _controller == null || _move == null) return;

        // 1. Scale the visuals
        visualModel.transform.localScale = Vector3.one * currentScale;

        // 2. THE FIX: Scale Radius first, then Height
        // Default radius is 0.5. At scale 0.2, radius becomes 0.1
        _controller.radius = 0.5f * currentScale; 
        _controller.height = 2.0f * currentScale;
        
        // 3. Anchor feet to pivot
        _controller.center = new Vector3(0, _controller.height / 2f, 0);

        // 4. Adjust Speed
        _move.moveSpeed = _originalSpeed * currentScale;
        
        // 5. Shrink the Skin Width
        // This is crucial for fitting through tight gaps!
        _controller.skinWidth = Mathf.Clamp(0.08f * currentScale, 0.001f, 0.08f);
    }
}
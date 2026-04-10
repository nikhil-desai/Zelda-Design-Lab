using UnityEngine;
using UnityEngine.InputSystem;

public class TransformationAbility : MonoBehaviour
{
    [Header("Visual References")]
    public GameObject humanModel;
    public GameObject wolfModel;

    [Header("Wolf Stats")]
    public float wolfSpeedMultiplier = 1.5f;
    public float wolfStepOffset = 0.8f; // Wolf can climb steeper things?

    private TopDownController _move;
    private CharacterController _controller;
    private bool _isWolf = false;
    private float _humanSpeed;
    private float _humanStepOffset;

    void Start()
    {
        _move = GetComponent<TopDownController>();
        _controller = GetComponent<CharacterController>();
        
        _humanSpeed = _move.moveSpeed;
        _humanStepOffset = _controller.stepOffset;

        // Ensure we start in the correct state
        UpdateState();
    }

    void Update()
    {
        // Toggle with 'T' for Transform
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            _isWolf = !_isWolf;
            UpdateState();
        }
    }

    void UpdateState()
    {
        // 1. Swap Visuals
        humanModel.SetActive(!_isWolf);
        wolfModel.SetActive(_isWolf);

        // 2. Adjust Physics Collider
        // Human is tall (2.0), Wolf is short and squat (1.0)
        _controller.height = _isWolf ? 1.0f : 2.0f;
        _controller.center = new Vector3(0, _controller.height / 2f, 0);

        // 3. Adjust Stats
        _move.moveSpeed = _isWolf ? _humanSpeed * wolfSpeedMultiplier : _humanSpeed;
        _controller.stepOffset = _isWolf ? wolfStepOffset : _humanStepOffset;

        Debug.Log(_isWolf ? "Wolf Form Active!" : "Human Form Active!");
    }
}
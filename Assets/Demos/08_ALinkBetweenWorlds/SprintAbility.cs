using UnityEngine;
using UnityEngine.InputSystem;

public class SprintAbility : MonoBehaviour
{
    public float sprintSpeed = 10f;
    public float magicCostPerSecond = 20f;
    
    private TopDownController _moveScript;
    private float _originalSpeed;

    void Start()
    {
        _moveScript = GetComponent<TopDownController>();
        _originalSpeed = _moveScript.moveSpeed;
    }

    void Update()
    {
        // Check if Shift is held AND we have magic
        if (Keyboard.current.leftShiftKey.isPressed)
        {
            // Calculate cost for THIS frame
            float costThisFrame = magicCostPerSecond * Time.deltaTime;

            if (MagicManager.Instance.UseMagic(costThisFrame))
            {
                _moveScript.moveSpeed = sprintSpeed;
                return; // Exit early to stay in sprint mode
            }
        }

        // If not shifting or out of magic, go back to normal speed
        _moveScript.moveSpeed = _originalSpeed;
    }
}
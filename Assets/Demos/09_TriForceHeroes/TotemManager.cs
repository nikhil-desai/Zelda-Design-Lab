using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TotemMember))]
public class TotemManager : MonoBehaviour
{
    [Header("State")]
    public bool isPossessed = false; 

    [Header("Detection")]
    public float pickupRange = 1.5f;
    public LayerMask heroLayer;

    private TotemMember _self;
    private CharacterController _controller;
    private PlayerInput _input;
    private MonoBehaviour _moveScript;

    void Awake()
    {
        _self = GetComponent<TotemMember>();
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInput>();
        _moveScript = GetComponent("TopDownController") as MonoBehaviour;
    }

    void Update()
    {
        if (!isPossessed) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (_self.carriedBy != null) JumpOff();
            else TryPickUp();
        }
    }

    // --- THE FIX: THIS FUNCTION MUST EXIST ---
    public TotemMember GetStackTop()
    {
        TotemMember current = _self;
        while (current.carrying != null)
        {
            current = current.carrying;
        }
        return current;
    }

    private void TryPickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, out hit, pickupRange, heroLayer))
        {
            TotemMember targetHero = hit.collider.GetComponent<TotemMember>();
            if (targetHero != null && targetHero != _self && !targetHero.IsInTotem)
            {
                StackHero(targetHero);
            }
        }
    }

    private void StackHero(TotemMember other)
    {
        // 1. Find the current highest point of the totem
        TotemMember topMember = GetStackTop();

        // 2. Establish links
        topMember.carrying = other;
        other.carriedBy = topMember;

        // 3. Parent to the TOP person, not the base
        other.transform.SetParent(topMember.transform);
        other.transform.localPosition = new Vector3(0, _self.stackHeight, 0);
        other.transform.localRotation = Quaternion.identity;

        // --- MODULAR SHUTDOWN ---
        if (other.GetComponent<CharacterController>()) 
            other.GetComponent<CharacterController>().enabled = false;

        var otherInput = other.GetComponent<PlayerInput>();
        if (otherInput != null) otherInput.enabled = false;

        var otherMove = other.GetComponent("TopDownController") as MonoBehaviour;
        if (otherMove != null) otherMove.enabled = false;
    }

    private void JumpOff()
    {
        TotemMember baseHero = _self.carriedBy;
        if (baseHero == null) return;

        baseHero.carrying = null;
        _self.carriedBy = null;
        transform.SetParent(null);

        if (_controller != null) _controller.enabled = true;
        if (_input != null) _input.enabled = isPossessed;
        if (_moveScript != null) _moveScript.enabled = isPossessed;

        transform.position += transform.forward * 1.5f + Vector3.up * 0.5f;
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class HeroSwitcher : MonoBehaviour
{
    public List<GameObject> heroes = new List<GameObject>();
    private int _activeIndex = 0;

    void Start()
    {
        UpdateHeroStates();
    }

    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            _activeIndex = (_activeIndex + 1) % heroes.Count;
            UpdateHeroStates();
        }
    }

    void UpdateHeroStates()
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            bool isPossessed = (i == _activeIndex);
            var totem = heroes[i].GetComponent<TotemMember>();
            
            // --- ADD THIS HANDSHAKE ---
            var manager = heroes[i].GetComponent<TotemManager>();
            if (manager != null) manager.isPossessed = isPossessed;
            // ---------------------------

            var move = heroes[i].GetComponent("TopDownController") as MonoBehaviour;
            if (move != null)
            {
                move.enabled = (isPossessed && totem.carriedBy == null);
            }

            var input = heroes[i].GetComponent<PlayerInput>();
            if (input != null) input.enabled = isPossessed;
        }
    }
}
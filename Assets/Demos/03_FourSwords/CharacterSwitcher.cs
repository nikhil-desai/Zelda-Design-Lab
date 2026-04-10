using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    public GameObject[] characters; 
    private int _currentIndex = 0;

    [Header("Camera Reference")]
    public ZeldaUniversalCamera universalCam; 

    void Start()
    {
        // Initial setup: Make sure only Link #1 is the "Player"
        for (int i = 0; i < characters.Length; i++)
        {
            bool isFirst = (i == _currentIndex);
            UpdateCharacterState(i, isFirst);
        }

        RefreshCamera();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchCharacter();
        }
    }

    void SwitchCharacter()
    {
        // 1. Current Link becomes a "Background" object
        UpdateCharacterState(_currentIndex, false);

        // 2. Move to the next index
        _currentIndex = (_currentIndex + 1) % characters.Length;

        // 3. New Link becomes the "Player"
        UpdateCharacterState(_currentIndex, true);

        // 4. Force camera to re-scan the scene
        RefreshCamera();
    }

    void UpdateCharacterState(int index, bool isActive)
    {
        // Toggle movement
        characters[index].GetComponent<TopDownController>().enabled = isActive;
        
        // Toggle Tag: This is what the camera looks for!
        // Make sure "Untagged" is spelled correctly (it's a default Unity tag)
        characters[index].tag = isActive ? "Player" : "Untagged";
    }

    void RefreshCamera()
    {
        if (universalCam != null)
        {
            universalCam.FindAllPlayers();
        }
    }
}
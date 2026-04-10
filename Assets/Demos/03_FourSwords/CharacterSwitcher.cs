using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    public TopDownController[] characters;
    private Color[] originalColors; 
    private int currentIndex = 0;

    // We store a reference to the camera script so we don't have to "find" it every frame
    private CameraFollow _cameraScript;

    void Start()
    {
        // 1. Find the CameraFollow script in the scene
        _cameraScript = Object.FindFirstObjectByType<CameraFollow>();

        // 2. Save original colors
        originalColors = new Color[characters.Length];
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].GetComponent<Renderer>() != null)
                originalColors[i] = characters[i].GetComponent<Renderer>().material.color;
        }

        SetTargetCharacter(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentIndex = (currentIndex + 1) % characters.Length;
            SetTargetCharacter(currentIndex);
            
            if(SoundManager.Instance != null) SoundManager.Instance.PlaySwitch();
        }
    }

    void SetTargetCharacter(int index)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            // Toggle movement
            characters[i].enabled = (i == index);
            
            // Visual Cue: Dim the inactive player
            Renderer rend = characters[i].GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = (i == index) ? originalColors[i] : originalColors[i] * 0.4f;
            }
        }

        // 3. THE CAMERA FIX: Tell the camera to follow the NEW active character
        if (_cameraScript != null)
        {
            _cameraScript.target = characters[index].transform;
        }
    }
}
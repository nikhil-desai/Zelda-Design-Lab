using UnityEngine;
using UnityEngine.Rendering; // Required for Volumes

public class PaletteManager : MonoBehaviour
{
    [Header("Profiles")]
    public Volume globalVolume;
    public VolumeProfile normalProfile;
    public VolumeProfile retroProfile;

    private bool _isRetro = false;

    void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.gKey.wasPressedThisFrame)
        {
            TogglePalette();
        }
    }

    void TogglePalette()
    {
        _isRetro = !_isRetro;
        
        if (_isRetro)
        {
            globalVolume.profile = retroProfile;
            Debug.Log("Switching to Retro Palette (Link's Awakening)");
        }
        else
        {
            globalVolume.profile = normalProfile;
            Debug.Log("Switching to Modern Palette");
        }
    }
}
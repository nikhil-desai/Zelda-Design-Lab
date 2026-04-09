using UnityEngine;

public class DimensionManager : MonoBehaviour
{
    [Header("World References")]
    public GameObject lightWorld;
    public GameObject darkWorld;

    private bool isDarkWorld = false;

    void Start()
    {
        // Default to Light World
        UpdateWorlds();
    }

    void Update()
    {
        // Space to Shift
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isDarkWorld = !isDarkWorld;
            UpdateWorlds();

            // Talk to our Global Systems!
            if (SoundManager.Instance != null) {
                SoundManager.Instance.PlaySwitch();
            }
        }
    }

    void UpdateWorlds()
    {
        lightWorld.SetActive(!isDarkWorld);
        darkWorld.SetActive(isDarkWorld);
    }
}
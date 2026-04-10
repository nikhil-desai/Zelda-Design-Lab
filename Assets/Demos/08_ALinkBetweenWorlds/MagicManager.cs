using UnityEngine;
using UnityEngine.UI;

public class MagicManager : MonoBehaviour
{
    public static MagicManager Instance; // The "Global" link

    [Header("Values")]
    public float maxMagic = 100f;
    public float currentMagic;
    public float regenRate = 10f; // Points per second
    public float regenDelay = 1.5f; // Wait before starting to regen

    [Header("UI")]
    public Slider magicSlider;

    private float _nextRegenTime;

    void Awake()
    {
        // Setup Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        currentMagic = maxMagic;
    }

    void Update()
    {
        // Handle Regeneration
        if (Time.time >= _nextRegenTime && currentMagic < maxMagic)
        {
            currentMagic = Mathf.MoveTowards(currentMagic, maxMagic, regenRate * Time.deltaTime);
        }

        // Update the Slider
        if (magicSlider != null)
        {
            magicSlider.maxValue = maxMagic;
            magicSlider.value = currentMagic;
        }
    }

    // This is the "Flexible" part. Any script can call this.
    public bool UseMagic(float amount)
    {
        if (currentMagic >= amount)
        {
            currentMagic -= amount;
            _nextRegenTime = Time.time + regenDelay; // Reset the delay
            return true; // We had enough magic!
        }
        
        return false; // Not enough magic!
    }
}
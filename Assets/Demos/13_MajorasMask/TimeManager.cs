using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Settings")]
    public float dayLengthInSeconds = 60f; // A "Day" is 60 seconds
    public float currentTime = 0f;

    void Awake() => Instance = this;

    void Update()
    {
        currentTime += Time.deltaTime;

        // Loop the clock back to 0 when the day ends
        if (currentTime >= dayLengthInSeconds)
        {
            currentTime = 0;
            Debug.Log("Dawn of a New Day");
        }

        RenderSettings.ambientLight = Color.Lerp(Color.white, Color.red, GetTimePercent());
    }

    // A helper to get the 0-1 percentage of the day (useful for lighting!)
    public float GetTimePercent() => currentTime / dayLengthInSeconds;
}
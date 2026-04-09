using UnityEngine;
using TMPro; // Make sure you have TextMeshPro installed!

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public string gameTitle;
    public string designTopic;

    void Start()
    {
        // Set the text at the start of the scene
        titleText.text = $"<b>Game:</b> {gameTitle}\n<b>Topic:</b> {designTopic}";
    }
}
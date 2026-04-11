using UnityEngine;

public class AsymmetricObserver : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gbaScreenUI; // The RawImage window
    public Camera gbaCamera;       // The camera rendering to the Render Texture

    private void OnTriggerEnter(Collider healthcare)
    {
        if (healthcare.CompareTag("Player"))
        {
            // Transition info to the "Handheld" screen
            gbaScreenUI.SetActive(true);
            Debug.Log("Entering Interior: Check your Sub-Screen!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gbaScreenUI.SetActive(false);
        }
    }
}
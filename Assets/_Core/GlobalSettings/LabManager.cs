using UnityEngine;
using UnityEngine.SceneManagement;

public class LabManager : MonoBehaviour
{
    void Update()
    {
        // 'N' for Next Demo
        if (Input.GetKeyDown(KeyCode.N)) NextScene();

        // 'R' for Restart Demo
        if (Input.GetKeyDown(KeyCode.R)) RestartScene();

        // 'Escape' to Quit (or go to Menu)
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit(); 
    }

    public void NextScene()
    {
        int nextIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextIndex);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
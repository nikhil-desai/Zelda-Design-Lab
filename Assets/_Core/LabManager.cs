using UnityEngine;
using UnityEngine.SceneManagement;

public class LabManager : MonoBehaviour
{
    void Update()
    {
        // Pressing 'N' loads the next demo in the Build Settings list
        if (Input.GetKeyDown(KeyCode.N))
        {
            int nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(nextSceneIndex);
        }

        // Pressing 'R' restarts the current demo
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class OraclePersistenceManager : MonoBehaviour
{
    public SecretDatabase database;
    public MonoBehaviour coreMovementScript; // For the "Modular MF" hijacking

    void Update()
    {
        // Press 'R' to reload the scene and test persistence
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Press 'Delete' to wipe the "Save" for testing
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            database.ClearDatabase();
            Debug.Log("<color=red>🗑️ Data Wiped:</color> Database cleared.");
        }
    }
}
using UnityEngine;

public class AnnotationClearer : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GameObject[] notes = GameObject.FindGameObjectsWithTag("Annotation");
            foreach (var note in notes) Destroy(note);
            Debug.Log("<color=orange>🧹 Lab Cleaned:</color> Annotations cleared.");
        }
    }
}
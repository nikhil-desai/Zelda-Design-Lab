using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SecretMaterialReacter : MonoBehaviour
{
    [Header("Data Source")]
    public SecretDatabase database;
    public string secretToMatch;

    [Header("Visuals")]
    public Material unlockedMaterial;
    
    private Material m_OriginalMaterial;
    private Renderer m_Renderer;

    private void Awake()
    {
        m_Renderer = GetComponent<Renderer>();
        m_OriginalMaterial = m_Renderer.sharedMaterial;
    }

    private void Start()
    {
        UpdateAppearance();
    }

    // This can be called by Start or by a Global Event if the secret is found in-scene
    public void UpdateAppearance()
    {
        if (database == null)
        {
            Debug.LogError($"[Lab Error] No Database assigned to {gameObject.name}");
            return;
        }

        if (database.IsSecretUnlocked(secretToMatch))
        {
            m_Renderer.material = unlockedMaterial;
            Debug.Log($"<color=cyan>🧪 Material Swapped:</color> {gameObject.name} is now using Unlocked Material.");
        }
        else
        {
            m_Renderer.material = m_OriginalMaterial;
        }
    }
}
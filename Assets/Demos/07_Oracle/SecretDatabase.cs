using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SecretDatabase", menuName = "Lab/Oracle/SecretDatabase")]
public class SecretDatabase : ScriptableObject
{
    // A simple list of strings representing unlocked secrets
    [SerializeField] private List<string> m_UnlockedSecrets = new List<string>();

    public void RegisterSecret(string secretID)
    {
        if (!m_UnlockedSecrets.Contains(secretID))
        {
            m_UnlockedSecrets.Add(secretID);
            Debug.Log($"<color=green>💾 Data Persisted:</color> Secret '{secretID}' saved to ScriptableObject.");
        }
    }

    public bool IsSecretUnlocked(string secretID) => m_UnlockedSecrets.Contains(secretID);

    public void ClearDatabase() => m_UnlockedSecrets.Clear();
}
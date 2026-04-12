using UnityEngine;
using System.Collections.Generic;

public class EchoManager : MonoBehaviour {
    public static EchoManager Instance;
    
    [Header("Settings")]
    public int maxSlots = 3; 
    public List<GameObject> activeEchoes = new List<GameObject>();
    private Dictionary<GameObject, Stack<GameObject>> pool = new Dictionary<GameObject, Stack<GameObject>>();

    void Awake() => Instance = this;

    public void RequestEcho(GameObject prefab, Vector3 pos, Quaternion rot) {
        // 1. Manage Capacity (FIFO)
        if (activeEchoes.Count >= maxSlots) {
            GameObject oldest = activeEchoes[0];
            activeEchoes.RemoveAt(0);
            Despawn(oldest);
        }

        // 2. Pooling Logic
        if (!pool.ContainsKey(prefab)) pool[prefab] = new Stack<GameObject>();
        
        GameObject echo = pool[prefab].Count > 0 ? pool[prefab].Pop() : Instantiate(prefab);
        echo.transform.SetPositionAndRotation(pos, rot);
        echo.SetActive(true);
        
        activeEchoes.Add(echo);
        echo.GetComponent<IEchoable>()?.OnSpawn();
    }

    public void Despawn(GameObject obj) {
        obj.SetActive(false);
        // We assume for the demo that 'obj' knows its own prefab origin or we just let it sit
        // To keep it simple for the demo, we'll just re-stack it.
        // In a pro build, you'd store the prefab ref on the object.
    }
}
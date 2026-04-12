using UnityEngine;

public class EchoIdentity : MonoBehaviour, IEchoable {
    public GameObject prefabOrigin; // Drag the PREFAB version of this object here
    public GameObject PrefabOrigin => prefabOrigin;

    public void OnSpawn() => Debug.Log("Echo Created");
    public void OnRecycle() => Debug.Log("Echo Recycled");
}
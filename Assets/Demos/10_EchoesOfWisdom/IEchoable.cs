using UnityEngine;

public interface IEchoable {
    GameObject PrefabOrigin { get; }
    void OnSpawn();
    void OnRecycle();
}
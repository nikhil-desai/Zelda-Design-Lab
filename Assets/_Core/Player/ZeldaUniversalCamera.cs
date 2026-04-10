using UnityEngine;
using System.Collections.Generic;

public class ZeldaUniversalCamera : MonoBehaviour
{
    [Header("Targeting")]
    public string playerTag = "Player";
    private List<Transform> _players = new List<Transform>();
    private List<CharacterController> _controllers = new List<CharacterController>();

    [Header("Zelda Perspective")]
    public Vector3 baseOffset = new Vector3(0, 14, -10); 
    public float smoothTime = 0.08f; 
    
    [Header("Dynamic Scaling")]
    public bool autoZoomOnScale = true;
    public float minZoomMultiplier = 0.4f;

    private Vector3 _currentVelocity = Vector3.zero;

    void Start() { FindAllPlayers(); }

    void LateUpdate()
    {
        if (_players.Count == 0) { FindAllPlayers(); return; }

        // 1. Calculate Midpoint and Average Height
        Vector3 centerPoint = GetCenterPoint();
        float avgHeight = GetAverageHeight();

        // 2. Adjust Zoom based on Scale (Visuals)
        float zoomFactor = 1.0f;
        if (autoZoomOnScale)
        {
            // Use height as a proxy for "size"
            zoomFactor = Mathf.Lerp(minZoomMultiplier, 1.0f, (avgHeight / 2.0f));
        }

        // 3. Move Camera
        Vector3 targetPosition = centerPoint + (baseOffset * zoomFactor);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);

        // 4. THE FIX: Look at the center of the body, not the feet
        // We look at the midpoint + half the average height
        transform.LookAt(centerPoint + (Vector3.up * (avgHeight * 0.5f)));
    }

    public void FindAllPlayers()
    {
        _players.Clear();
        _controllers.Clear();
        GameObject[] targets = GameObject.FindGameObjectsWithTag(playerTag);
        foreach (GameObject t in targets)
        {
            _players.Add(t.transform);
            var cc = t.GetComponent<CharacterController>();
            if (cc != null) _controllers.Add(cc);
        }
    }

    Vector3 GetCenterPoint()
    {
        if (_players.Count == 0) return Vector3.zero;
        Vector3 sum = Vector3.zero;
        foreach (var p in _players) sum += p.position;
        return sum / _players.Count;
    }

    float GetAverageHeight()
    {
        if (_controllers.Count == 0) return 2.0f;
        float totalH = 0;
        foreach (var cc in _controllers) totalH += cc.height;
        return totalH / _controllers.Count;
    }
}
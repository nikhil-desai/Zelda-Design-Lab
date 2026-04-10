using UnityEngine;
using System.Collections.Generic;

public class ZeldaUniversalCamera : MonoBehaviour
{
    [Header("Targeting")]
    public string playerTag = "Player";
    private List<Transform> _players = new List<Transform>();

    [Header("Zelda Perspective")]
    public Vector3 baseOffset = new Vector3(0, 14, -10); 
    public float smoothTime = 0.12f; 
    
    [Header("Dynamic Scaling")]
    public bool autoZoomOnScale = true;
    public float minZoomMultiplier = 0.35f;

    private Vector3 _currentVelocity = Vector3.zero;

    void Start()
    {
        FindAllPlayers();
    }

    void LateUpdate()
    {
        if (_players.Count == 0) 
        {
            FindAllPlayers();
            return;
        }

        // 1. Calculate the Center Point and Average Scale
        Vector3 centerPoint = GetCenterPoint();
        float averageScale = GetAverageScale();

        // 2. Calculate Offset (Standard or Zoomed)
        Vector3 targetOffset = baseOffset;
        if (autoZoomOnScale)
        {
            float zoomFactor = Mathf.Lerp(minZoomMultiplier, 1.0f, (averageScale - 0.2f) / 0.8f);
            targetOffset *= zoomFactor;
        }

        // 3. Position the Camera relative to the Midpoint
        Vector3 targetPosition = centerPoint + targetOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);

        // 4. Look at the Midpoint
        transform.LookAt(centerPoint + (Vector3.up * averageScale));
    }

    public void FindAllPlayers()
    {
        _players.Clear();
        GameObject[] targets = GameObject.FindGameObjectsWithTag(playerTag);
        foreach (GameObject t in targets)
        {
            _players.Add(t.transform);
        }
    }

    Vector3 GetCenterPoint()
    {
        if (_players.Count == 1) return _players[0].position;

        var bounds = new Bounds(_players[0].position, Vector3.zero);
        for (int i = 0; i < _players.Count; i++)
        {
            bounds.Encapsulate(_players[i].position);
        }

        return bounds.center;
    }

    float GetAverageScale()
    {
        float totalScale = 0;
        foreach (Transform p in _players)
        {
            // Tries to find the scale of the "Visuals" child
            if (p.childCount > 0) totalScale += p.GetChild(0).localScale.x;
            else totalScale += 1.0f;
        }
        return totalScale / _players.Count;
    }
}
using UnityEngine;
using System.Collections.Generic;

public class PhantomAnnotationManager : MonoBehaviour
{
    [Header("References")]
    public MonoBehaviour coreMovementScript;
    public GameObject linePrefab; // Prefab with a LineRenderer component
    public RectTransform drawingArea;

    [Header("Settings")]
    public float depthOffset = 10f; // Distance from camera
    public float minVertexDistance = 0.1f;

    private LineRenderer m_CurrentLine;
    private List<Vector3> m_Points = new List<Vector3>();
    private bool m_IsDrawing = false;

    void Update()
    {
        // Toggle Drawing Mode (Hold Right Mouse Button)
        if (Input.GetMouseButtonDown(1)) ToggleDrawingMode(true);
        if (Input.GetMouseButtonUp(1)) ToggleDrawingMode(false);

        if (m_IsDrawing)
        {
            HandleDrawing();
        }
    }

    void ToggleDrawingMode(bool active)
    {
        m_IsDrawing = active;
        if (coreMovementScript != null) coreMovementScript.enabled = !active;
        
        if (active) StartNewLine();
    }

    void StartNewLine()
    {
        GameObject go = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        m_CurrentLine = go.GetComponent<LineRenderer>();
        m_Points.Clear();
    }

    void HandleDrawing()
    {
        if (Input.GetMouseButton(0)) // Left Click to draw
        {
            // Convert Screen Space to World Space
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = depthOffset; // Critical: Constant distance from lens
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            if (m_Points.Count == 0 || Vector3.Distance(m_Points[m_Points.Count - 1], worldPos) > minVertexDistance)
            {
                m_Points.Add(worldPos);
                m_CurrentLine.positionCount = m_Points.Count;
                m_CurrentLine.SetPositions(m_Points.ToArray());
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            m_CurrentLine = null; // Break the line
        }
    }
}
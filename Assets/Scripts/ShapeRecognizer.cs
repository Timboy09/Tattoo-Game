using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ShapeRecognizer : MonoBehaviour
{

    public LineRenderer lineRendererPrefab; // Prefab with Line Renderer component
    public float cornerThreshold = 0.1f; // Threshold for detecting corners
    public float minDistance = 0.01f; // Minimum distance between points (smoother lines)
    public float maxDistance = 0.5f; // Maximum distance for scattered effect
    public float speedThreshold = 5f; // Speed threshold to switch between smooth and scattered
    public Material fastDrawingMaterial; // Material for fast drawing
    public Material slowDrawingMaterial; // Material for slow drawing

    private List<Vector3> points = new List<Vector3>();
    private List<LineRenderer> slowLineRenderers = new List<LineRenderer>();
    private List<LineRenderer> fastLineRenderers = new List<LineRenderer>();
    private List<Vector3> corners = new List<Vector3>(); // List to store detected corners
    private LineRenderer currentLineRenderer;
    private Material currentMaterial;

    private Vector3 lastPoint;
    private float lastTime;

    void Start()
    {
        lastPoint = Vector3.zero;
        lastTime = Time.time;
        CreateNewLineRenderer(slowDrawingMaterial); // Start with slow drawing material
    }


    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame) // Start a new stroke
        {
            points.Clear(); // Clear the points for the new stroke
            CreateNewLineRenderer(slowDrawingMaterial); // Start with slow drawing material
        }

        if (Mouse.current.leftButton.isPressed) // Drawing with the left mouse button
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, Camera.main.nearClipPlane));
            mousePos.z = 0;

            float distance = Vector3.Distance(lastPoint, mousePos);
            float timeElapsed = Time.time - lastTime;
            float speed = distance / timeElapsed;

            if (points.Count == 0 || ShouldAddPoint(speed, distance))
            {
                points.Add(mousePos);
                Debug.Log($"Point added: {mousePos}");
                UpdateLineRenderer();
                lastPoint = mousePos;
                lastTime = Time.time;

                // Switch to fast material if speed exceeds threshold
                if (speed > speedThreshold && currentMaterial != fastDrawingMaterial)
                {
                    CreateNewLineRenderer(fastDrawingMaterial);
                }
            }
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame) // End of drawing
        {
            if (currentMaterial == slowDrawingMaterial)
            {
                RecognizeShape(); // Shape recognition for slow-drawn lines only
            }
            ClearAllLines(); // Clear all lines for next drawing session
        }
    }

    void CreateNewLineRenderer(Material material)
    {
        if (currentLineRenderer != null && currentMaterial == material && material == slowDrawingMaterial)
        {
            return; // Don't create a new LineRenderer if the material is still slow-drawing
        }

        GameObject newLineObj = Instantiate(lineRendererPrefab.gameObject);
        currentLineRenderer = newLineObj.GetComponent<LineRenderer>();
        currentLineRenderer.positionCount = 0;
        currentLineRenderer.material = material;
        currentMaterial = material;

        if (material == fastDrawingMaterial)
        {
            fastLineRenderers.Add(currentLineRenderer);
        }
        else
        {
            slowLineRenderers.Add(currentLineRenderer);
        }
    }

    bool ShouldAddPoint(float speed, float distance)
    {
        if (speed > speedThreshold)
        {
            return distance > maxDistance; // For fast lines, ensure the distance between points is larger
        }
        else
        {
            return distance > minDistance; // For slow lines, smaller distance for smooth drawing
        }
    }

    void ClearAllLines()
    {
        foreach (var lineRenderer in slowLineRenderers)
        {
            Destroy(lineRenderer.gameObject);
        }
        slowLineRenderers.Clear();

        foreach (var lineRenderer in fastLineRenderers)
        {
            Destroy(lineRenderer.gameObject);
        }
        fastLineRenderers.Clear();
    }

    void UpdateLineRenderer()
    {
        currentLineRenderer.positionCount = points.Count;
        currentLineRenderer.SetPositions(points.ToArray());
    }
    void RecognizeShape()
    {
        if (points.Count < 3)
        {
            Debug.Log("Not enough points to recognize a shape.");
            return;
        }

        corners.Clear();
        DetectCorners();

        int cornerCount = corners.Count;
        Debug.Log($"Detected corners: {cornerCount}");

        if (cornerCount == 0)
        {
            Debug.Log("Shape recognized as Circle.");
        }
        else if (cornerCount == 1)
        {
            Debug.Log("Shape recognized as Circle.");
        }
        else if (cornerCount == 3)
        {
            Debug.Log("Shape recognized as Triangle.");
        }
        else if (cornerCount == 4)
        {
            Debug.Log("Shape recognized as Square or Rectangle.");
        }
        else if (cornerCount > 4)
        {
            Debug.Log("Shape recognized as Star.");
        }
        else
        {
            Debug.Log("Shape not recognized.");
        }
    }

    void DetectCorners()
    {
        corners.Clear();
        float threshold = cornerThreshold * 180f; // Convert fraction to angle in degrees
        for (int i = 1; i < points.Count - 1; i++)
        {
            Vector3 p1 = points[i - 1];
            Vector3 p2 = points[i];
            Vector3 p3 = points[i + 1];

            Vector3 dir1 = (p2 - p1).normalized;
            Vector3 dir2 = (p3 - p2).normalized;

            float angle = Vector3.Angle(dir1, dir2);
            if (angle > threshold) // Significant turning point
            {
                corners.Add(p2);
                Debug.Log($"Corner detected at: {p2} with angle: {angle}");
            }
        }
    }

}

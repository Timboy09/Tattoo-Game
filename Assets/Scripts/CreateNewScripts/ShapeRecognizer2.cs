using System.Collections.Generic;
using UnityEngine;

public class ShapeRecognizer2 : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private List<Vector3> points = new List<Vector3>();

    void Start()
    {
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer is not assigned!");
            return;
        }

        // Fetch points from the LineRenderer
        GetPointsFromLineRenderer();
    }

    void GetPointsFromLineRenderer()
    {
        int positionCount = lineRenderer.positionCount;
        for (int i = 0; i < positionCount; i++)
        {
            points.Add(lineRenderer.GetPosition(i));
        }
    }

    public string RecognizeShape()
    {
        if (points.Count < 3)
        {
            return "Not enough points to recognize a shape.";
        }

        // Simplifying the points to reduce noise
        SimplifyPoints(0.1f);

        // Shape recognition logic
        if (IsCircle())
        {
            return "Circle detected!";
        }
        else if (IsTriangle())
        {
            return "Triangle detected!";
        }
        else if (IsSquare())
        {
            return "Square detected!";
        }

        return "Shape not recognized.";
    }

    private void SimplifyPoints(float tolerance)
    {
        // Implement a simple point simplification algorithm (e.g., Ramer-Douglas-Peucker)
        // For now, we will keep all points for simplicity.
        // You may replace this section with a proper algorithm.
    }

    private bool IsCircle()
    {
        // Basic circle check logic (distance from center should be consistent)
        Vector3 center = CalculateCentroid();
        float radius = Vector3.Distance(center, points[0]);
        foreach (var point in points)
        {
            if (Mathf.Abs(Vector3.Distance(center, point) - radius) > 0.1f) // Tolerance
            {
                return false;
            }
        }
        return true;
    }

    private bool IsTriangle()
    {
        // Check if there are 3 points close together
        return points.Count == 3;
    }

    private bool IsSquare()
    {
        if (points.Count != 4) return false;

        // Check if the points form a square (you can add more robust checks based on angles)
        float distance1 = Vector3.Distance(points[0], points[1]);
        float distance2 = Vector3.Distance(points[1], points[2]);
        float distance3 = Vector3.Distance(points[2], points[3]);
        float distance4 = Vector3.Distance(points[3], points[0]);

        return Mathf.Abs(distance1 - distance2) < 0.1f &&
               Mathf.Abs(distance2 - distance3) < 0.1f &&
               Mathf.Abs(distance3 - distance4) < 0.1f;
    }

    private Vector3 CalculateCentroid()
    {
        Vector3 centroid = Vector3.zero;
        foreach (var point in points)
        {
            centroid += point;
        }
        return centroid / points.Count;
    }
}
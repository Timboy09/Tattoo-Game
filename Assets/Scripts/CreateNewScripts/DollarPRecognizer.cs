using System.Collections.Generic;
using UnityEngine;

public class DollarPRecognizer : MonoBehaviour
{
    public class Point
    {
        public float x;
        public float y;

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    private Dictionary<string, List<Point>> gestures = new Dictionary<string, List<Point>>();

    public void AddGesture(string name, List<Point> points)
    {
        gestures[name] = points;
    }

    public string Recognize(List<Point> inputPoints)
    {
        // Implement the recognition logic
        // This is a basic example; you may need to implement your own recognition algorithm

        foreach (var gesture in gestures)
        {
            if (IsMatch(gesture.Value, inputPoints))
            {
                return gesture.Key;
            }
        }
        return null; // No match found
    }

    private bool IsMatch(List<Point> gesturePoints, List<Point> inputPoints)
    {
        // Implement your matching logic here, such as comparing point distances
        // For simplicity, this example just checks the count of points
        // In reality, you should use a proper algorithm (e.g., least squares)

        if (gesturePoints.Count != inputPoints.Count)
        {
            return false;
        }

        for (int i = 0; i < gesturePoints.Count; i++)
        {
            // Check if the points match closely enough
            if (Vector2.Distance(new Vector2(gesturePoints[i].x, gesturePoints[i].y),
                                 new Vector2(inputPoints[i].x, inputPoints[i].y)) > 0.1f) // Adjust tolerance as needed
            {
                return false;
            }
        }
        return true; // All points matched
    }
}

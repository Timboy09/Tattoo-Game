using System.Collections.Generic;
using UnityEngine;

public class ShapesDatabase : MonoBehaviour
{
    private DollarPRecognizer recognizer;

    void Start()
    {
        recognizer = GetComponent<DollarPRecognizer>();

        // Define a few basic shapes
        recognizer.AddGesture("Rectangle", GetRectangle());
        recognizer.AddGesture("Circle", GetCircle());
        recognizer.AddGesture("Triangle", GetTriangle());
        recognizer.AddGesture("Star", GetStar());
    }

    private List<DollarPRecognizer.Point> GetRectangle()
    {
        return new List<DollarPRecognizer.Point>
        {
            new DollarPRecognizer.Point(-0.5f, -0.5f),
            new DollarPRecognizer.Point(0.5f, -0.5f),
            new DollarPRecognizer.Point(0.5f, 0.5f),
            new DollarPRecognizer.Point(-0.5f, 0.5f),
            new DollarPRecognizer.Point(-0.5f, -0.5f) // Closing the rectangle
        };
    }

    private List<DollarPRecognizer.Point> GetCircle()
    {
        List<DollarPRecognizer.Point> circle = new List<DollarPRecognizer.Point>();
        int segments = 40;
        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.Deg2Rad * (i * 360.0f / segments);
            circle.Add(new DollarPRecognizer.Point(Mathf.Cos(angle), Mathf.Sin(angle)));
        }
        return circle;
    }

    private List<DollarPRecognizer.Point> GetTriangle()
    {
        return new List<DollarPRecognizer.Point>
        {
            new DollarPRecognizer.Point(0, 0),
            new DollarPRecognizer.Point(1, 1),
            new DollarPRecognizer.Point(-1, 1),
            new DollarPRecognizer.Point(0, 0)
        };
    }

    private List<DollarPRecognizer.Point> GetStar()
    {
        return new List<DollarPRecognizer.Point>
        {
            new DollarPRecognizer.Point(0, 1),
            new DollarPRecognizer.Point(0.2f, 0.4f),
            new DollarPRecognizer.Point(1, 0.4f),
            new DollarPRecognizer.Point(0.4f, -0.2f),
            new DollarPRecognizer.Point(0.6f, -1),
            new DollarPRecognizer.Point(0, -0.6f),
            new DollarPRecognizer.Point(-0.6f, -1),
            new DollarPRecognizer.Point(-0.4f, -0.2f),
            new DollarPRecognizer.Point(-1, 0.4f),
            new DollarPRecognizer.Point(-0.2f, 0.4f),
            new DollarPRecognizer.Point(0, 1)
        };
    }
}

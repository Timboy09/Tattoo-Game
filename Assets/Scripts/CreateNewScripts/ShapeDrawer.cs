using System.Collections.Generic;
using UnityEngine;

public class ShapeDrawer : MonoBehaviour
{
    public LineRenderer lineee;
    private DollarPRecognizer recognizer;
    private List<DollarPRecognizer.Point> drawnPoints = new List<DollarPRecognizer.Point>();

    void Start()
    {
        recognizer = GetComponent<DollarPRecognizer>();
    }

    void Update()
    {
        // Capture input points and call the recognition method
        if (Input.GetMouseButtonDown(0)) // Start drawing
        {
            drawnPoints.Clear();
        }

        if (Input.GetMouseButton(0)) // While drawing
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            drawnPoints.Add(new DollarPRecognizer.Point(mousePos.x, mousePos.y));
        }

        if (Input.GetMouseButtonUp(0)) // Finished drawing
        {
            string recognizedShape = recognizer.Recognize(drawnPoints);
            Debug.Log($"Recognized Shape: {recognizedShape}");
        }
    }
}

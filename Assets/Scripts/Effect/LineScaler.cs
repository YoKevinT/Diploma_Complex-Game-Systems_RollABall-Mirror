using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bypasser script for allowing us to animate the scale of a LineRenderer

[RequireComponent(typeof(LineRenderer))]
public class LineScaler : MonoBehaviour
{
    public float scale = 1.0f;
    private LineRenderer lineRenderer;

    private void OnDrawGizmos()
    {
        // Scale the Line
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = scale;
    }

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        lineRenderer.widthMultiplier = scale;        
    }
}

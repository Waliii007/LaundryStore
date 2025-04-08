using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LaundaryMan
{
    [ExecuteAlways]
    public class DrawPath : MonoBehaviour
    {
        public Color gizmoColor = Color.red; // Set the color of the path
        public float sphereSize = 0.2f; // Size of sphere Gizmos

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;

            // Get all child transforms
            Transform[] pathPoints = GetComponentsInChildren<Transform>();

            for (int i = 1; i < pathPoints.Length; i++)
            {
                Transform currentPoint = pathPoints[i];
                Transform previousPoint = pathPoints[i - 1];

                // Draw a sphere at each point
                Gizmos.DrawSphere(currentPoint.position, sphereSize);

                // Draw a line connecting points
                if (i > 1)
                {
                    Gizmos.DrawLine(previousPoint.position, currentPoint.position);
                }
            }
        }
    }
}
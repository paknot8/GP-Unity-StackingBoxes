using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHeightLine : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Origin point of the raycast (current object's position)
        Vector3 origin = transform.position;

        // Direction of the raycast (downward direction)
        Vector3 direction = Vector3.down;

        // Length of the raycast (5 units)
        float distance = 5f;

        // Perform the raycast
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, distance))
        {
            // Draw a line from the object's position to the hit point
            Debug.DrawLine(origin, hit.point, Color.red);
        }
        else
        {
            // If no hit, draw a line indicating the full distance
            Vector3 endPosition = origin + direction * distance;
            Debug.DrawLine(origin, endPosition, Color.red);
        }
    }
}

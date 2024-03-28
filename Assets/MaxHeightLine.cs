using UnityEngine;

public class MaxHeightLine : MonoBehaviour
{
    public float distance = 2f;
    public LayerMask layerMask; // Layer mask to filter out collisions with the object's own layer
    RaycastHit2D hit;

    // Update is called once per frame
    void Update()
    {
        // Cast a ray downwards in local space
        Vector2 direction = -transform.up; // Using transform.up to cast downwards in local space
        hit = Physics2D.Raycast(transform.position, direction, distance, ~layerMask); // Use ~ to invert the layer mask

        if (hit.collider != null)
        {
            Debug.Log("You Hit SOMETHING!");
            Debug.DrawLine(transform.position, hit.point, Color.blue);
        }
        else
        {
            // If no hit, draw the line to the maximum distance
            Vector3 endPosition = transform.position + (Vector3)direction * distance;
            Debug.DrawLine(transform.position, endPosition, Color.red);
            Debug.Log("No Hit");
        }
    }
}

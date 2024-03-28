using UnityEngine;
using System.Collections;

public class MaxHeightLine : MonoBehaviour
{
    public float distance = 1f;
    public LayerMask layerMask; // Layer mask to filter out collisions with the object's own layer
    public int rayCount = 10; // Number of rays to cast
    RaycastHit2D[] hits;

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine to toggle the script state
        StartCoroutine(ToggleScript());
    }

    // Coroutine to toggle the script state
    IEnumerator ToggleScript()
    {
        while (true)
        {
            // Enable the script
            enabled = true;

            // Wait for 2 seconds
            yield return new WaitForSeconds(2f);

            // Disable the script
            enabled = false;

            // Wait for 2 seconds
            yield return new WaitForSeconds(2f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            // Calculate the width of the object using its BoxCollider2D
            float objectWidth = GetComponent<SpriteRenderer>().bounds.size.x;

            // Calculate the spacing between each ray
            float raySpacing = objectWidth / (rayCount - 1);

            // Cast rays downwards
            hits = new RaycastHit2D[rayCount];
            for (int i = 0; i < rayCount; i++)
            {
                // Calculate the origin of each ray
                Vector2 rayOrigin = new Vector2(transform.position.x - objectWidth / 2 + i * raySpacing, transform.position.y);

                // Cast a ray downwards at the calculated origin
                hits[i] = Physics2D.Raycast(rayOrigin, Vector2.down, distance, ~layerMask);

                // Draw debug lines
                if (hits[i].collider != null)
                {
                    Debug.Log("Ray " + i + " hit: " + hits[i].collider.gameObject.name);
                    Debug.DrawLine(rayOrigin, hits[i].point, Color.blue);
                }
                else
                {
                    Vector3 endPosition = rayOrigin + Vector2.down * distance;
                    Debug.DrawLine(rayOrigin, endPosition, Color.red);
                    Debug.Log("Ray " + i + " didn't hit anything");
                }
            }
        }
    }
}

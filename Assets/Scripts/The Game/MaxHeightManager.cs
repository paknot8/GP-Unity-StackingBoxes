using UnityEngine;

public class MaxHeightManager : MonoBehaviour
{
    [SerializeField] private GameObject maxHeightLineObject;
    [SerializeField] private LayerMask layerMask; // Layer mask to filter out collisions with the object's own layer
    [SerializeField] private float rayDistance = 1f;
    [SerializeField] private int rayCount = 25;
    [SerializeField] private RaycastHit2D[] hits;
    [HideInInspector] private MainCameraManager mainCameraManager;

    // Start is called before the first frame update
    void Start()
    {
        hits = new RaycastHit2D[rayCount]; // Initialize hits array here
        mainCameraManager = Camera.main.GetComponent<MainCameraManager>();
    }

    void Update() => DrawRayDownwards();

    public void EnableObject() => maxHeightLineObject.SetActive(true);
    public void DisableObject()
    {
        maxHeightLineObject.SetActive(false);
        DisableRays();
    }

    private void DisableRays()
    {
        for (int i = 0; i < rayCount; i++)
        {
            Debug.DrawLine(Vector3.zero, Vector3.zero, Color.clear);
        }
    }

    private void DrawRayDownwards()
    {
        if (!maxHeightLineObject.activeSelf) // Don't draw rays if the object is disabled
            return;

        if (maxHeightLineObject != null && maxHeightLineObject)
        {
            float objectWidth = maxHeightLineObject.GetComponent<SpriteRenderer>().bounds.size.x; // Calculate the width of the object using its BoxCollider2D
            float raySpacing = objectWidth / (rayCount - 1); // Calculate the spacing between each ray

            // Cast rays downwards
            for (int i = 0; i < rayCount; i++)
            {
                // Calculate the origin of each ray
                Vector2 rayOrigin = new(maxHeightLineObject.transform.position.x - objectWidth / 2 + i * raySpacing, maxHeightLineObject.transform.position.y);
                hits[i] = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance, ~layerMask); // Cast a ray downwards at the calculated origin

                // Draw lines
                if (hits[i].collider != null)
                {
                    Debug.DrawLine(rayOrigin, hits[i].point, Color.blue);
                    DisableObject();
                    if (mainCameraManager != null)
                    {
                        mainCameraManager.GoUp(); // Call GoUp method in MainCamera_Manager.cs
                    }
                    EnableObject();
                }
                else
                {
                    Vector3 endPosition = rayOrigin + Vector2.down * rayDistance;
                    Debug.DrawLine(rayOrigin, endPosition, Color.red);
                }
            }
        }
    }
}

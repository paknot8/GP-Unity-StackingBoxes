using UnityEngine;

public class MaxHeightManager : MonoBehaviour
{
    #region Variables & References
        // References
        [SerializeField] private GameObject maxHeightLineObject;
        [SerializeField] private LayerMask layerMask; // Layer mask to filter out collisions with the object's own layer

        // Variables
        public float distance = 1f;
    
        // Raycast
        public int rayCount = 10; // Number of rays to cast
        RaycastHit2D[] hits;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Enable()
    {
        maxHeightLineObject.SetActive(true);
    }

    public void Disable()
    {
        maxHeightLineObject.SetActive(false);
        DisableRays(); // Disable the rays when disabling the object
    }

    private void DisableRays()
    {
        // Draw debug lines to disable all rays
        for (int i = 0; i < rayCount; i++)
        {
            Debug.DrawLine(Vector3.zero, Vector3.zero, Color.clear);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleToggleMaxHeightLine();
        DrawRayDownwards();
    }

    private void HandleToggleMaxHeightLine()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!maxHeightLineObject.activeSelf)
            {
                Enable();
            }
            else
            {
                Disable();
            }
        }
    }

    private void DrawRayDownwards()
    {
        if (!maxHeightLineObject.activeSelf) // Don't draw rays if the object is disabled
            return;

        if(maxHeightLineObject != null && maxHeightLineObject){
            // Calculate the width of the object using its BoxCollider2D
            float objectWidth = maxHeightLineObject.GetComponent<SpriteRenderer>().bounds.size.x;
            

            // Calculate the spacing between each ray
            float raySpacing = objectWidth / (rayCount - 1);

            // Cast rays downwards
            hits = new RaycastHit2D[rayCount];
            for (int i = 0; i < rayCount; i++)
            {
                // Calculate the origin of each ray
                Vector2 rayOrigin = new(maxHeightLineObject.transform.position.x - objectWidth / 2 + i * raySpacing, maxHeightLineObject.transform.position.y);

                // Cast a ray downwards at the calculated origin
                hits[i] = Physics2D.Raycast(rayOrigin, Vector2.down, distance, ~layerMask);

                // Draw debug lines
                if (hits[i].collider != null)
                {
                    //Debug.Log("Ray " + i + " hit: " + hits[i].collider.gameObject.name);
                    Debug.DrawLine(rayOrigin, hits[i].point, Color.blue);
                }
                else
                {
                    Vector3 endPosition = rayOrigin + Vector2.down * distance;
                    Debug.DrawLine(rayOrigin, endPosition, Color.red);
                    //Debug.Log("Ray " + i + " didn't hit anything");
                }
            }
        } 
    }
}

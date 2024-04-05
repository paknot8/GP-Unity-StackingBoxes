using UnityEngine;

public class GuideLineScript : MonoBehaviour
{
    // Reference to the Renderer component
    [HideInInspector] private Renderer guideLines;
    [SerializeField] private float transparancy = 0.3f;

    void Awake(){
        guideLines = GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Check if a Renderer component exists
        if (guideLines != null)
        {
            // Get the material of the object
            Material material = guideLines.material;

            // Get the current color of the material
            Color color = material.color;

            // Set the alpha value to desired transparency (0 for fully transparent, 1 for fully opaque)
            color.a = transparancy;

            // Set the modified color back to the material
            material.color = color;
        }
        else
        {
            Debug.LogWarning("Renderer component not found on this object.");
        }
    }
}

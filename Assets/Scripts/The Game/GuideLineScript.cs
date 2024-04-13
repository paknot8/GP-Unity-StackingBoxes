using UnityEngine;

public class GuideLineScript : MonoBehaviour
{
    [Header("Variables & References")]
    [HideInInspector] private Renderer guideLines;
    private float transparancy;

    void Awake(){
        guideLines = GetComponent<Renderer>();
        transparancy = 1f;
    }

    void Start() => ObjectTransparancy();

    private void ObjectTransparancy()
    {
        if (guideLines != null)
        {
            // Get the material and color of the object
            Material material = guideLines.material;
            Color color = material.color;

            // Set the alpha value to desired transparency (0 for fully transparent, 1 for fully opaque)
            color.a = transparancy;
            material.color = color; // Set the modified color back to the material
        }
    }
}

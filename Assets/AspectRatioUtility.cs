using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioUtility : MonoBehaviour
{
    // Set the desired aspect ratio (width / height)
    public float targetAspectRatio = 9f / 16f;

    private void Start()
    {
        // Calculate the desired aspect ratio
        float currentAspectRatio = (float)Screen.width / Screen.height;

        // Check if the current aspect ratio is already approximately equal to the target aspect ratio
        if (Mathf.Approximately(currentAspectRatio, targetAspectRatio))
        {
            return;
        }

        // Calculate the desired width
        float scaleHeight = currentAspectRatio / targetAspectRatio;

        // If scaled width is less than current width, add pillarbox
        if (scaleHeight < 1.0f)
        {
            Rect rect = Camera.main.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            Camera.main.rect = rect;
        }
        else // Add letterbox
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = Camera.main.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            Camera.main.rect = rect;
        }
    }
}

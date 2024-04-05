using System.Collections;
using UnityEngine;

public class MainCameraManager : MonoBehaviour
{
    // Speed of camera movement
    public float moveSpeed = 2f;

    // Flag to track if camera is already moving
    private bool isMoving = false;

    // Moves the camera upwards if it's not already moving
    public void GoUp()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCameraUpSmoothly());
        }
    }

    // Smoothly moves the camera upwards using coroutine
    IEnumerator MoveCameraUpSmoothly()
    {
        isMoving = true;

        // Calculate target position
        Vector3 targetPosition = transform.position + Vector3.up * 2f;

        // Move the camera to the target position
        while (transform.position.y < targetPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Reset the flag since camera has reached the target position
        isMoving = false;
    }
}

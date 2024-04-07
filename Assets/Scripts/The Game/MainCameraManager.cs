using System.Collections;
using UnityEngine;

public class MainCameraManager : MonoBehaviour
{
    public float cameraMoveSpeed = 2f;
    private bool cameraIsMoving = false;

    // Moves the camera upwards if it's not already moving
    public void GoUp()
    {
        if (!cameraIsMoving) { StartCoroutine(MoveCameraUpSmoothly()); }
    }

    // Smoothly moves the camera upwards using coroutine
    IEnumerator MoveCameraUpSmoothly()
    {
        cameraIsMoving = true;
        Vector3 targetPosition = transform.position + Vector3.up * 2f;
        // Move the camera to the target position
        while (transform.position.y < targetPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, cameraMoveSpeed * Time.deltaTime);
            yield return null;
        }
        cameraIsMoving = false; // Reset since camera has reached the target position
    }
}

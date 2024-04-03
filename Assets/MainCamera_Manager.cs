using System.Collections;
using UnityEngine;

public class MainCamera_Manager : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of movement

    void Update()
    {

    }

    public void GoUp()
    {
        // Check if the camera is already moving
        if (!isMoving)
        {
            StartCoroutine(MoveObjectUpSmoothly());
        }
    }

    private bool isMoving = false; // Flag to track if camera is already moving

    IEnumerator MoveObjectUpSmoothly()
    {
        isMoving = true; // Set the flag to indicate the camera is moving
        
        Vector3 targetPosition = transform.position + Vector3.up * 1f;
        // Move the camera to the target position
        while (transform.position.y < targetPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false; // Reset the flag since camera has reached the target position
    }


    // public void GoUp()
    // {
    //     StartCoroutine(MoveObjectUpSmoothly());
    // }

    // IEnumerator MoveObjectUpSmoothly()
    // {
    //     Vector3 targetPosition = transform.position + Vector3.up * 3f;
    //     while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
    //     {
    //         transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    //         yield return null;
    //     }
    // }
}

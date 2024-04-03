using System.Collections;
using UnityEngine;

public class MainCamera_Manager : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of movement

    void Update()
    {
        // GoUp();
    }

    public void GoUp()
    {
        // if (Input.GetKeyDown(KeyCode.O)) // Check for 'O' key press
        // {
        //     StartCoroutine(MoveObjectUpSmoothly());
        // }

        StartCoroutine(MoveObjectUpSmoothly());
    }

    IEnumerator MoveObjectUpSmoothly()
    {
        Vector3 targetPosition = transform.position + Vector3.up * 2f;
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}

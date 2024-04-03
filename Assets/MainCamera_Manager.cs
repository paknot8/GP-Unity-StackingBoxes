using System.Collections;
using UnityEngine;

public class MoveUpOnKeyPress : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of movement

    void Update()
    {
        UpButton();
    }

    private void UpButton()
    {
        if (Input.GetKeyDown(KeyCode.O)) // Check for 'O' key press
        {
            StartCoroutine(MoveObjectUpSmoothly());
        }
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

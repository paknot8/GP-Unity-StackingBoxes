using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Triangle : MonoBehaviour
{
    public float moveSpeed = 5f;
    [NonSerialized] private Vector2 movementInput;
    private Rigidbody rb; // Reference to Rigidbody component

    void Start()
    {
        // Get the reference to the Rigidbody component
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity by default
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        // Calculate movement direction
        Vector2 moveDirection = new(movementInput.x, 0f);

        // Move the object
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered Collision");
        if (other.CompareTag("GameOverLine"))
        {
            // Destroy the object
            Destroy(gameObject);
        }
    }

    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void OnDrop(InputValue value)
    {
        if (value.isPressed)
        {
            // Enable gravity when the drop button is pressed
            rb.useGravity = true;
        }
    }
}

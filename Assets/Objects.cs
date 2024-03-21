using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Objects : MonoBehaviour
{
    public float moveSpeed = 5f;
    [NonSerialized] private Vector2 movementInput;
    private Rigidbody rb;
    private Vector3 initialPosition; // Store the initial position of the square
    private bool isDropped = false; // Flag to indicate whether the square has been dropped
    private bool isStacked = false; // Flag to indicate whether the square has stacked on another square

    public GameObject squarePrefab; // Reference to the Square prefab

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        initialPosition = transform.position; // Store the initial position when the object is spawned
    }

    void Update()
    {
        if (!isDropped && !isStacked) // Only move if the square has not been dropped and has not stacked
        {
            Move();
        }
    }

    void Move()
    {
        Vector2 moveDirection = new Vector2(movementInput.x, 0f);
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered Collision");
        if (other.CompareTag("GameOverLine"))
        {
            Destroy(gameObject);
        }
        else if (!isDropped && !isStacked) // Check if the square has not been dropped and has not stacked
        {
            // Stack the new square on top of the previously dropped square
            StackSquare();
        }
    }

    void StackSquare()
    {
        // Calculate the position to stack the new square
        Vector3 stackPosition = transform.position + Vector3.up;

        // Align the new square with the stacked position
        transform.position = new Vector3(stackPosition.x, transform.position.y, transform.position.z);

        // Set the dropped flag to true and the stacked flag to true
        isDropped = true;
        isStacked = true;

        // Spawn the new square
        SpawnSquare(initialPosition);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Stackable")) // Check if the collision is with a stackable object
        {
            isStacked = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Stackable")) // Check if the collision with a stackable object ends
        {
            isStacked = false;
        }
    }

    void OnMove(InputValue value)
    {
        if (!isDropped && !isStacked) // Only respond to movement controls if the square has not been dropped and has not stacked
        {
            movementInput = value.Get<Vector2>();
        }
    }

    void OnDrop(InputValue value)
    {
        if (value.isPressed && !isDropped) // Only drop if the square has not been dropped
        {
            rb.useGravity = true;
            isDropped = true; // Set the flag to true indicating that the square has been dropped
        }
    }

    void SpawnSquare(Vector3 position)
    {
        // Spawn the Square prefab at the given position
        GameObject newSquare = Instantiate(squarePrefab, position, Quaternion.identity);
        Rigidbody newSquareRb = newSquare.GetComponent<Rigidbody>();
        if (newSquareRb != null)
        {
            newSquareRb.velocity = Vector3.zero;
        }
    }
}

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

    public GameObject squarePrefab; // Reference to the Square prefab

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        initialPosition = transform.position; // Store the initial position when the object is spawned
    }

    void Update()
    {
        if (!isDropped) // Only move if the square has not been dropped
        {
            Move();
        }
    }

    void Move()
    {
        Vector2 moveDirection = new Vector2(movementInput.x, 0f);
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
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

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered Collision");
        if (other.CompareTag("GameOverLine"))
        {
            Destroy(gameObject);
        }
        else
        {
            // Spawn the new square at the initial position
            SpawnSquare(initialPosition);
        }
    }

    void OnMove(InputValue value)
    {
        if (!isDropped) // Only respond to movement controls if the square has not been dropped
        {
            movementInput = value.Get<Vector2>();
        }
    }

    void OnDrop(InputValue value)
    {
        if (value.isPressed)
        {
            rb.useGravity = true;
            isDropped = true; // Set the flag to true indicating that the square has been dropped
        }
    }
}

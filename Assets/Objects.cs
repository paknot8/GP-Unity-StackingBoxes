using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Objects : MonoBehaviour
{
    public float moveSpeed = 5f;
    [NonSerialized] private Vector2 movementInput;
    private Rigidbody rb;
   
    public GameObject squarePrefab; // Reference to the Square prefab
    public GameObject trianglePrefab; // Reference to the Triangle prefab

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector2 moveDirection = new(movementInput.x, 0f);
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection);
    }

    void SpawnSquare()
    {
        // Spawn the Square prefab at position (2, 0.4, 0)
        Instantiate(squarePrefab, new Vector3(2f, 0.4f, 0f), Quaternion.identity);
    }

    void SpawnTriangle()
    {
        // Spawn the Triangle prefab at position (2, 0.4, 0) with an offset of 2 units along the x-axis
        Instantiate(trianglePrefab, new Vector3(4f, 0.4f, 0f), Quaternion.identity);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered Collision");
        if (other.CompareTag("GameOverLine"))
        {
            Destroy(gameObject);
        }
        SpawnSquare();
        SpawnTriangle();
    }

    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    void OnDrop(InputValue value)
    {
        if (value.isPressed)
        {
            rb.useGravity = true;
        }
    }
}

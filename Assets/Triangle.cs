using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Triangle : MonoBehaviour
{
    public float moveSpeed = 5f;
    [NonSerialized] private Vector2 movementInput;

    void Start(){
        
    }

    void Update()
    {
        // Move the object based on the input
        Move();
    }

    void Move()
    {
        // Calculate movement direction
        Vector2 moveDirection = new Vector2(movementInput.x, 0f);

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
}

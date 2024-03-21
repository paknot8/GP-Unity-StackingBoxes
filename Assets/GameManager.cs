using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform blockPrefab;
    [SerializeField] private Transform blockHolder;

    private Transform currentBlock = null;
    private Rigidbody2D currentRigidbody;

    private Vector2 BlockStartPosition = new Vector2(0f,2f);

    private float blockSpeed = 5f;
    private float blockSpeedIncrement = 0.5f;
    private float blockDirection = 1f;
    private float xLimit = 2f;

    private float timeBetweenRounds = 1f;

    // Start is called before the first frame update
    void Start()
    {
        SpawnNewBlock();
    }

    private void SpawnNewBlock(){
        // Create a block with te desired properties.
        currentBlock = Instantiate(blockPrefab, blockHolder);
        currentBlock.position = BlockStartPosition;
        currentBlock.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        currentRigidbody = currentBlock.GetComponent<Rigidbody2D>();
        // Increase the block speed each time to make it harder
        //blockSpeed += blockSpeedIncrement;
    }

    private IEnumerator DelayedSpawn(){
        yield return new WaitForSeconds(timeBetweenRounds);
        SpawnNewBlock();
    }

    // Update is called once per frame
    void Update()
    {
        // If we have a waiting block, move it about.
        if (currentBlock){
            float moveAmount = Time.deltaTime * blockSpeed * blockDirection;
            currentBlock.position += new Vector3(moveAmount, 0, 0);
            // if we've gone as far as we want, reverse direction.
            if(Mathf.Abs(currentBlock.position.x) > xLimit){
                // Set to the limit so it doesn't go further.
                currentBlock.position = new Vector3(blockDirection * xLimit, currentBlock.position.y, 0);
                blockDirection = -blockDirection;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Stop it moving.
                currentBlock = null;
                // Activate the RigidBody to enable gravity to drop it.
                currentRigidbody.simulated = true;
                // Spawn the next block.
                StartCoroutine(DelayedSpawn());
            }
        }
    }

    // void OnMove(InputValue value){
    //     Vector2 movement = value.Get<Vector2>();
    // }

    // void OnDrop(InputValue value)
    // {
    //     if (value.isPressed)
    //     {
    //         // Stop it moving.
    //         currentBlock = null;
    //         // Activate the RigidBody to enable gravity to drop it.
    //         currentRigidbody.simulated = true;
    //         // Spawn the next block.
    //         SpawnNewBlock();
    //     }
    // }
}

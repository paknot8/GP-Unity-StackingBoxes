using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    // For the placeholders in the inspector
    [SerializeField] private Transform blockPrefab;
    [SerializeField] private Transform blockHolder;

    [SerializeField] private TMPro.TextMeshProUGUI LivesText; // reference to TextMesh Pro

    private Transform currentBlock = null;
    private Rigidbody2D currentRigidbody;

    private Vector2 BlockStartPosition = new(0f,2f);

    public Vector2 vector;
    public Vector3 movement;

    [SerializeField] private float blockSpeed = 1f;
    [SerializeField] private float blockSpeedIncrement = 0.5f;
    [SerializeField] private float blockDirection = 1f;
    [SerializeField] private float xLimit = 2f;

    [SerializeField] private float timeBetweenRounds = 1f;

    // Varaibles to handle the game state.
    private int startingLives = 3;
    private int livesRemaining;
    private bool playing = true;

    // Start is called before the first frame update
    void Start()
    {
        livesRemaining = startingLives;
        LivesText.text = $"{livesRemaining}"; // to update the Textmesh pro text
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
        
        Movement();

        // for restart game
        RestartGame();
    }

    private void Movement()
    {
        // Calculate movement direction
        //Vector2 moveDirection = new Vector2(movementInput.x, 0f);
        // Calculate movement direction
        movement = new(vector.x,0);

        // Move the object
        transform.Translate(blockSpeed * Time.deltaTime * movement);
        

        //BlockMovement();
    }

    private void BlockMovement()
    {
        if (currentBlock)
        {
            float moveAmount = Time.deltaTime * blockSpeed * blockDirection;
            //currentBlock.position += new Vector3(moveAmount, 0, 0);

            // if we've gone as far as we want, reverse direction.
            if (Mathf.Abs(currentBlock.position.x) > xLimit)
            {
                // Set to the limit so it doesn't go further.
                currentBlock.position = new Vector3(blockDirection * xLimit, currentBlock.position.y, 0);
                blockDirection = -blockDirection;
            }
        }


        // if (currentBlock)
        // {
        //     float moveAmount = Time.deltaTime * blockSpeed * blockDirection;
        //     //currentBlock.position += new Vector3(moveAmount, 0, 0);

        //     // if we've gone as far as we want, reverse direction.
        //     if (Mathf.Abs(currentBlock.position.x) > xLimit)
        //     {
        //         // Set to the limit so it doesn't go further.
        //         currentBlock.position = new Vector3(blockDirection * xLimit, currentBlock.position.y, 0);
        //         blockDirection = -blockDirection;
        //     }
        // }
    }



    private void RestartGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    // Called from LoseLife whenever it detects a block has fallen off.
    public void RemoveLife()
    {
        // Update the lives remainnig UI element
        livesRemaining = Mathf.Max(livesRemaining -1, 0);
        LivesText.text = $"{livesRemaining}";
        // Check for end of game
        if (livesRemaining == 0){
            playing = false;
        }
    }

    void OnMove(InputValue value){
        vector = value.Get<Vector2>();
    }

    void OnDrop(InputValue value)
    {
        if (value.isPressed)
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

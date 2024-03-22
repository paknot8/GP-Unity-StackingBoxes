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
    public Vector2 movement;

    [SerializeField] private float blockSpeed = 1f;
    [SerializeField] private float blockSpeedIncrement = 0.5f;
    [SerializeField] private float blockDirection = 1f;
    [SerializeField] private float xLimit = 2f;

    [SerializeField] private float timeBetweenRounds = 1f;

    // Varaibles to handle the game state.
    private readonly int startingLives = 3;
    private int livesRemaining;
    private bool isPlaying = true;

    // Start is called before the first frame update
    void Start()
    {
        livesRemaining = startingLives;
        LivesText.text = $"{livesRemaining}"; // to update the Textmesh pro text
        SpawnNewBlock();

        
    }

    // Update is called once per frame
    void Update()
    {
        BlockMovement();
    }

    private void SpawnNewBlock(){
        // Create a block with te desired properties.
        currentBlock = Instantiate(blockPrefab, blockHolder);
        currentBlock.position = BlockStartPosition;

        currentBlock.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        currentRigidbody = currentBlock.GetComponent<Rigidbody2D>(); // pass currentBlock to the rigidbody

        // Increase the block speed each time to make it harder
        //blockSpeed += blockSpeedIncrement;
    }

    private IEnumerator DelayedSpawn(){
        yield return new WaitForSeconds(timeBetweenRounds);
        SpawnNewBlock();
    }

    private void BlockMovement()
    {
        // Fixed the movement, add the currentBlock so the blocked spawned will be there
        movement = new(vector.x,vector.y);
        currentBlock.transform.Translate(blockSpeed * Time.deltaTime * movement); 
    }

    // Called from LoseLife whenever it detects a block has fallen off.
    public void RemoveLife()
    {
        // Update the lives remainnig UI element
        livesRemaining = Mathf.Max(livesRemaining -1, 0);
        LivesText.text = $"{livesRemaining}";
        // Check for end of game
        if (livesRemaining == 0){
            isPlaying = false;
        }
    }

    void OnMove(InputValue value) {
        Debug.Log(vector = value.Get<Vector2>());
        //BlockMovement(movement);
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

    void OnQuit(InputValue value){
        if(value.isPressed) UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

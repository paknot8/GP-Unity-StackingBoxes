using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/* --- Requirements ---
- The game can be in 3D or 2D, you get to pick.
- It consists of 1 scene. This scene has a 'floor' and a higher platform, like a 'table'.
- Like in Tetris, the player gets a random box that they can drop unto the table from a certain height.
- Important: dropping the boxes and their collision is Physics-based.
- The goal of the game is to stack the highest tower of boxes. 
If a box is dropped on the floor and not on the table or another box, it is game over.

- When the box lands, the player gets a new box (Again, like in Tetris)

- The boxes are of different shapes and colours (required to use PreFabs). 
Shapes can be inspired by Tetris or you can mix it up by using triangles, 
circles, capsules, etc. Only using square cubes isn't sufficient.

- Before the player drops the box they can move it around left-right horizontally using either arrow keys or mouse position 
(one of the options, both isn't required).

- If the tower of boxes gets higher and higher, make sure your camera responds by moving up or zooming out.

Tips:

For inspiration and clarification, play this browser version of the game (head's up: this is more than what is required for this achievement)
https://couchpotatoes.itch.io/afwas


*/

public class GameManager : MonoBehaviour
{
    // For the placeholders in the inspector
    [SerializeField] private Transform blockPrefab;
    [SerializeField] private Transform blockHolder;
    private Transform currentBlock = null;
    private Rigidbody2D currentRigidbody;

    [SerializeField] private TMPro.TextMeshProUGUI LivesText; // reference to TextMesh Pro

    private Vector2 BlockStartPosition = new(0f,2f);
    private Vector2 vector;
    private Vector2 movement;

    [SerializeField] private float blockSpeed = 1f;
    [SerializeField] private float timeBetweenRounds = 2f;

    // Varaibles to handle the game state.
    private readonly int startingLives = 3;
    private int livesRemaining;
    // private bool isPlaying = true;

    #region Start & Update
        void Start()
        {
            livesRemaining = startingLives;
            LivesText.text = $"{livesRemaining}"; // to update the Textmesh pro text
            SpawnNewBlock();
        }

        void Update()
        {
            if(currentBlock != null){
                BlockMovement();
            }
        }
    #endregion

    private void SpawnNewBlock(){
        // Create a block with te desired properties.
        currentBlock = Instantiate(blockPrefab, blockHolder);
        currentBlock.position = BlockStartPosition;
        currentBlock.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        currentRigidbody = currentBlock.GetComponent<Rigidbody2D>(); // pass currentBlock to the rigidbody
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
            //isPlaying = false;
        }
    }

    void OnMove(InputValue value) => Debug.Log(vector = value.Get<Vector2>());

    void OnDrop(InputValue value)
    {
        if (value.isPressed && currentBlock != null)
        {
            // Stop it from moving
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

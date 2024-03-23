using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/* --- Requirements ---
- The game can be in 3D or 2D, you get to pick. (DONE)
- It consists of 1 scene. This scene has a 'floor' and a higher platform, like a 'table'. (DONE)
- Like in Tetris, the player gets a random box that they can drop unto the table from a certain height. (DONE)
- Important: dropping the boxes and their collision is Physics-based. (DONE)

- The goal of the game is to stack the highest tower of boxes. 
If a box is dropped on the floor and not on the table or another box, it is game over. (DONE)

- When the box lands, the player gets a new box (Again, like in Tetris) (DONE)

- The boxes are of different shapes and colours (required to use PreFabs). 
Shapes can be inspired by Tetris or you can mix it up by using triangles, 
circles, capsules, etc. Only using square cubes isn't sufficient. (DONE)

- Before the player drops the box they can move it around left-right horizontally using either arrow keys or mouse position 
(one of the options, both isn't required). (DONE)

- If the tower of boxes gets higher and higher, make sure your camera responds by moving up or zooming out.

Tips:

For inspiration and clarification, play this browser version of the game (head's up: this is more than what is required for this achievement)
https://couchpotatoes.itch.io/afwas
*/

// TO WATCH Video Tutorial camera: https://www.youtube.com/watch?v=4QP0ZTPUaI4

public class GameManager : MonoBehaviour
{
    #region Variables and References
        // For the placeholders in the inspector
        [SerializeField] private Transform[] objectPrefabs; // Array of prefabs to spawn
        [SerializeField] private Transform objectPrefab;
        [SerializeField] private Transform objectHolder;
        [HideInInspector] private Transform currentObject = null;
        [HideInInspector] private Rigidbody2D currentRigidbody;

        [SerializeField] private TMPro.TextMeshProUGUI LivesText; // reference to TextMesh Pro

        // Movement & Position Variables
        private Vector2 objectStartPosition = new(0f,2f);
        private Vector2 vector;
        private Vector2 movement;
        // Speed and Time
        [SerializeField] private float objectMoveSpeed = 3f;
        [SerializeField] private float timeBetweenRounds = 2f;

        // Variables to handle the game state.
        private readonly int startingLives = 3;
        private int livesRemaining;
        private bool isPlaying = true;
    #endregion

    #region Start & Update
        void Start()
        {
            ResetLives();
            UpdateLivesText();
            SpawnNewObject();
        }

        void Update()
        {
            CheckPlaceHolderIsEmpty();
        }
    #endregion

    #region Spawning
        private void SpawnNewObject()
        {
            // Randomly select a prefab from the ObjectPrefabs array
            int randomIndex = Random.Range(0, objectPrefabs.Length);
            Transform selectedPrefab = objectPrefabs[randomIndex];

            // Create a Object with the selected prefab
            currentObject = Instantiate(selectedPrefab, objectHolder);
            currentObject.position = objectStartPosition;
            currentObject.GetComponent<SpriteRenderer>().color = Random.ColorHSV();

            // Set currentRigidbody for the new Object
            currentRigidbody = currentObject.GetComponent<Rigidbody2D>();
        }

        private IEnumerator DelaySpawnNewObject()
        {
            yield return new WaitForSeconds(timeBetweenRounds);
            SpawnNewObject();
        }

        private void StopAndSpawnNext()
        {   
            currentObject = null; // Stop it from moving
            currentRigidbody.simulated = true; // Activate the RigidBody to enable gravity to drop it.
            StartCoroutine(DelaySpawnNewObject()); // Spawn the next Object.
        }
    #endregion

    #region Movement
        private void ObjectMovement()
        {
            // Fixed the movement, add the currentObject so the objected spawned will be there and control that specific object
            movement = new(vector.x,vector.y);
            currentObject.transform.Translate(objectMoveSpeed * Time.deltaTime * movement); 
        }

        // Check if the currentObject is empty in the Placeholder
        private void CheckPlaceHolderIsEmpty(){
            if (currentObject != null) ObjectMovement();
        }
    #endregion

    #region Stats & Score
        public void SubstractLifePoint() // Called from LoseLife whenever it detects a Object has fallen off.
        {
            // Update the lives remainnig UI element
            livesRemaining = Mathf.Max(livesRemaining -1, 0);
            LivesText.text = $"{livesRemaining}";

            // Check for end of game
            if (livesRemaining == 0){
                //isPlaying = false;
            }
        }

        void ResetLives() => livesRemaining = startingLives;
        void UpdateLivesText() => LivesText.text = $"{livesRemaining}"; // to update the Textmesh pro text
    #endregion

    #region Menu
        void PauseGame(){
            if(isPlaying) {
                Time.timeScale = 0f; // Pause the game
                isPlaying = false;
            } else {
                Time.timeScale = 1f; // UnPause the game
                isPlaying = true;
            }
        }
    #endregion

    #region New Input System Input/Controls
        void OnMove(InputValue value){
            Debug.Log(vector = value.Get<Vector2>());
        }

        void OnDrop(InputValue value){
            if (value.isPressed && currentObject != null) StopAndSpawnNext();
        }

        void OnQuit(InputValue value){
            if(value.isPressed) SceneManager.LoadScene(0);
        }

        void OnPause(InputValue value){
            if (value.isPressed) PauseGame();
        }
    #endregion
}
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// GameManager class responsible for managing the game flow and objects.
public class GameManager : MonoBehaviour
{
    #region Variables & References
        // Serialized fields for easy tweaking in the Unity Editor
        [Header("Object References")]
        [SerializeField] private Transform[] objectPrefabs;
        [SerializeField] private Transform objectHolder;
        [SerializeField] private TMPro.TextMeshProUGUI livesText;
        [SerializeField] private TMPro.TextMeshProUGUI scoreText;
        [SerializeField] private AudioSource scoreSound;
        [SerializeField] private AudioSource substractLifeSound;
        [SerializeField] private AudioSource gameOverSound;
        [SerializeField] private AudioSource mainThemeMusic;
        [SerializeField] private AudioSource gameOverMusic;

        // Script References
        [SerializeField] private MaxHeightManager maxHeightLine;
        [SerializeField] private GameUI gameUI;
        [SerializeField] private ScoreManager scoreManager;

        [Header("Public Variables")]
        public float currentObjectMoveSpeed;
        public int startingLives;

        // Private variables
        private Transform currentObject; // Current spawned object
        private Rigidbody2D currentRigidbody; // Rigidbody of the object
        private int livesRemaining;
        public bool isPlaying;
        private Vector2 vector;
        public int score;

        // restricted colors
        [HideInInspector] private readonly Color[] restrictedColors =
        {
            new(0.1f, 0.5f, 0.1f),  // Dark green
            new(0.4f, 0.1f, 0.4f),  // Dark purple
            new(0.1f, 0.1f, 0.5f),  // Dark blue
            new(0f, 0f, 0f),        // Black
            new(0.5f, 0.1f, 0.1f),  // Dark red
            new(0.3f, 0.3f, 0f),    // Dark yellow
            new(0.1f, 0.4f, 0.4f),  // Dark cyan
        };
    #endregion

    #region Default Unity Methods
        // Awake is called when the script instance is being loaded
        void Awake(){
            currentObjectMoveSpeed = 4f;
            startingLives = 3;
            score = 0;
            isPlaying = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            SpawnNewObject();
            ResetLives();
            UpdateLivesText();
            scoreManager.LoadScore();
        }

        // Update is called once per frame
        void Update() => CheckPlaceholderIsEmpty();
    #endregion

    #region Player Object & Spawning
        // Spawns a new object with random properties
        private void SpawnNewObject()
        {
            int randomIndex = Random.Range(0, objectPrefabs.Length);
            Transform selectedPrefab = objectPrefabs[randomIndex];

            // Select a random color from the restricted colors array
            Color selectedColor = restrictedColors[Random.Range(0, restrictedColors.Length)];

            // Spawn position based on camera position and height
            Vector3 cameraPosition = Camera.main.transform.position;
            float cameraHeight = Camera.main.orthographicSize;
            Vector2 spawnPosition = new(cameraPosition.x, cameraPosition.y + cameraHeight - 2f);

            // Instantiate the object and set its color
            currentObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
            currentObject.GetComponent<SpriteRenderer>().color = selectedColor;
            currentRigidbody = currentObject.GetComponent<Rigidbody2D>();
        }

        // Coroutine for delayed object spawning and object enabler
        private IEnumerator DelaySpawn()
        {
            maxHeightLine.DisableObject();
            yield return new WaitForSeconds(1f);
            maxHeightLine.EnableObject();
            yield return new WaitForSeconds(2f);
            SpawnNewObject();
        }

        // Stops the current object and spawns the next one after a delay
        private void StopAndSpawnNext()
        {
            if (currentObject != null)
            {
                // Increment the score when releasing the object
                score++;
                scoreSound.Play();
                UpdateScoreText();

                currentObject = null;
                currentRigidbody.simulated = true;
                StartCoroutine(DelaySpawn());
            }
        }

        // Moves the current object according to input
        private void ObjectMovement()
        {
            Vector2 moveInput = new Vector2(vector.x, vector.y);
            moveInput.x *= currentObjectMoveSpeed * Time.deltaTime; // Multiply only by speed for horizontal movement
            currentObject.transform.Translate(moveInput);
        }

        // Checks if the placeholder for the object is empty and moves it if necessary
        private void CheckPlaceholderIsEmpty() { if (currentObject != null) ObjectMovement(); }
    #endregion

    #region Game UI
        // Subtracts one life point and updates UI
        public void SubtractLifePoint()
        {
            livesRemaining = Mathf.Max(0, livesRemaining - 1); // ensure that the number never goes to 0 when substracting
            livesText.text = $"{livesRemaining}";
            if (livesRemaining > 0) substractLifeSound.Play();
            UpdateScoreText();

            // Load main menu scene if lives run out
            if (livesRemaining == 0)
            {
                gameOverSound.Play();
                GameOver();
            }
        }

        // UI Text updates
        private void ResetLives() => livesRemaining = startingLives;
        private void UpdateLivesText() => livesText.text = $"{livesRemaining}";
        private void UpdateScoreText() => scoreText.text = $"Score: {score}";

        private void PauseGame()
        {
            // Toggle the game's time scale between paused and normal
            if (isPlaying)
            {
                Time.timeScale = 0f; // Pause the game
                isPlaying = false;
            }
            else
            {
                Time.timeScale = 1f; // Resume the game
                isPlaying = true;
            }
            gameUI.OnGamePaused();
        }

        // Method to handle game over and save the score
        private void GameOver()
        {
            if (isPlaying)
            {
                Time.timeScale = 0f; // Pause the game
                isPlaying = false;
                mainThemeMusic.Stop();
                gameOverMusic.Play();
                
                // scoreManager.SaveScore(score); // Save the score when the game is over

                gameUI.OnGameOver();
            }
        }
    #endregion

    #region New System Input
        void OnMoveLeft(InputValue value)
        {
            vector = value.Get<Vector2>();
            vector.y = 0f; // Make sure vertical input is zero
        }

        void OnMoveRight(InputValue value)
        {
            vector = value.Get<Vector2>();
            vector.y = 0f; // Make sure vertical input is zero
        }

        void OnDrop(InputValue value)
        {
            if (value.isPressed && currentObject != null) StopAndSpawnNext();
        }

        void OnPause(InputValue value)
        {
            if (value.isPressed)
            {
                PauseGame();
            }
        }
    #endregion
}

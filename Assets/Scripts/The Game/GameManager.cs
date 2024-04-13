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

        [Header("Sound Effects")]
        [SerializeField] private AudioSource scoreSound;
        [SerializeField] private AudioSource substractLifeSound;
        [SerializeField] private AudioSource mainThemeMusic;
        [SerializeField] private AudioSource gameOverMusic;
        [SerializeField] private AudioSource onPauseSound;

        [Header("Script References")]
        [SerializeField] private MaxHeightManager maxHeightLine;
        [SerializeField] private GameUI gameUI;
        [SerializeField] private ScoreManager scoreManager;

        [Header("Public Variables")]
        public float currentObjectMoveSpeed;
        public int startingLives;

        // Other Variables
        private Transform currentObject; // Current spawned object
        private Rigidbody2D currentRigidbody; // Rigidbody of the object
        private int livesRemaining;
        public bool isPlaying;
        private Vector2 vector;
        public int score;

        // --- Used in Gold 2 as Color change now has texture and prefabs ---
        // // restricted colors
        // [HideInInspector] private readonly Color[] restrictedColors =
        // {
        //     new Color(0.3f, 0.7f, 0.3f),  // Light green
        //     new Color(0.7f, 0.3f, 0.7f),  // Light purple
        //     new Color(0.3f, 0.3f, 0.7f),  // Light blue
        //     new Color(0.5f, 0.5f, 0.5f),  // Gray
        //     new Color(0.7f, 0.3f, 0.3f),  // Light red
        //     new Color(0.5f, 0.5f, 0.1f),  // Light yellow
        //     new Color(0.3f, 0.7f, 0.7f)   // Light cyan
        // };
    #endregion

    #region Default Unity Methods
        void Awake()
        {
            currentObjectMoveSpeed = 4f;
            startingLives = 3;
            score = 0;
            isPlaying = true;

            // Get a reference to ScoreManager
            scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
            {
                // Load the score when the game starts
                scoreManager.LoadTopScore();
            }
        }

        void Start()
        {
            SpawnNewObject();
            ResetLives();
            UpdateLivesText();
        }

        void Update() => CheckPlaceholderIsEmpty();
    #endregion

    #region Player Object & Spawning
        // Spawns a new object with random properties
        private void SpawnNewObject()
        {
            int randomIndex = Random.Range(0, objectPrefabs.Length);
            Transform selectedPrefab = objectPrefabs[randomIndex];

            // Select a random color from the restricted colors array
            // Color selectedColor = restrictedColors[Random.Range(0, restrictedColors.Length)];

            // Spawn position based on camera position and height
            Vector3 cameraPosition = Camera.main.transform.position;
            float cameraHeight = Camera.main.orthographicSize;
            Vector2 spawnPosition = new(cameraPosition.x, cameraPosition.y + cameraHeight - 2f);

            // Instantiate the object and set its color
            currentObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
            // currentObject.GetComponent<SpriteRenderer>().color = selectedColor;
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
            if(currentObject != null){
                Vector2 moveInput = new(vector.x, vector.y);
                moveInput.x *= currentObjectMoveSpeed * Time.deltaTime;
                currentObject.transform.Translate(moveInput);
            }
        }

        // Checks if the placeholder for the object is empty and moves it if necessary
        private void CheckPlaceholderIsEmpty() { if (currentObject != null) ObjectMovement(); }
    #endregion

    #region Game UI
        // Subtracts one life point and updates UI
        public void SubtractLifePoint()
        {
            livesRemaining = Mathf.Max(0, livesRemaining - 1); // ensure that the number never goes to 0 when substracting
            UpdateLivesText();
            if (livesRemaining > 0) substractLifeSound.Play();
            UpdateScoreText();

            // Load main menu scene if lives run out
            if (livesRemaining == 0)
            {
                GameOver();
            }
        }

        // UI Text updates
        private void ResetLives() => livesRemaining = startingLives;
        private void UpdateLivesText() => livesText.text = $"{livesRemaining}";
        private void UpdateScoreText() => scoreText.text = $"Score: {score}";

        private void PauseGame()
        {
            if (isPlaying)
            {
                Time.timeScale = 0f;
                isPlaying = false;
            }
            else
            {
                Time.timeScale = 1f;
                isPlaying = true;
            }
            gameUI.OnGamePaused();
        }

        // Method to handle game over and save the score
        private void GameOver()
        {
            // Save the score when the game is over
            if (scoreManager != null)
            {
                scoreManager.SaveTopScore(score);
            }
            Time.timeScale = 0f; // Pause the game
            isPlaying = false;
            mainThemeMusic.Stop();
            gameOverMusic.Play();
            gameUI.OnGameOver();
        }
    #endregion

    #region New System Input
        void OnMove(InputValue value) => vector = value.Get<Vector2>();

        void OnDrop(InputValue value)
        {
            if (value.isPressed && currentObject != null) StopAndSpawnNext();
        }

        void OnPause(InputValue value)
        {
            if (value.isPressed)
            {
                onPauseSound.Play();
                PauseGame();
            }
        }
    #endregion
}

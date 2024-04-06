using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// GameManager class responsible for managing the game flow and objects.
public class GameManager : MonoBehaviour
{
    // Serialized fields for easy tweaking in the Unity Editor
    [Header("Object References")]
    [SerializeField] private Transform[] objectPrefabs;
    [SerializeField] private Transform objectHolder;
    [SerializeField] private MaxHeightManager maxHeightLine;
    [SerializeField] private TMPro.TextMeshProUGUI livesText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private GameUI gameUI;

    [Header("Public Variables")]
    public float currentObjectMoveSpeed;
    public int startingLives;

    // Private variables
    private Transform currentObject; // Current spawned object
    private Rigidbody2D currentRigidbody; // Rigidbody of the object
    private int livesRemaining;
    public bool isPlaying = true;
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

    // Awake is called when the script instance is being loaded
    void Awake(){
        currentObjectMoveSpeed = 4f;
        startingLives = 3;
        score = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnNewObject();
        ResetLives();
        UpdateLivesText();
    }

    // Update is called once per frame
    void Update() => CheckPlaceholderIsEmpty();

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
        Vector2 spawnPosition = new Vector2(cameraPosition.x, cameraPosition.y + cameraHeight - 2f);

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
            UpdateScoreText();

            currentObject = null;
            currentRigidbody.simulated = true;
            StartCoroutine(DelaySpawn());
        }
    }

    // Moves the current object according to input
    private void ObjectMovement() => currentObject.transform.Translate(currentObjectMoveSpeed * Time.deltaTime * new Vector2(vector.x, vector.y));

    // Checks if the placeholder for the object is empty and moves it if necessary
    private void CheckPlaceholderIsEmpty() { if (currentObject != null) ObjectMovement(); }

    // Subtracts one life point and updates UI
    public void SubtractLifePoint()
    {
        livesRemaining = Mathf.Max(livesRemaining - 1, 0);
        livesText.text = $"{livesRemaining}";

        // Load main menu scene if lives run out
        if (livesRemaining == 0)
        {
            Debug.Log("You Lost!");
            SceneManager.LoadScene(0);
        }
    }

    private void ResetLives() => livesRemaining = startingLives; // Resets life count to starting value
    private void UpdateLivesText() => livesText.text = $"{livesRemaining}"; // Updates lives text in UI
    private void UpdateScoreText() => scoreText.text = $"Score: {score}"; // Updates score text in UI

    private void PauseGame()
    {
        Time.timeScale = isPlaying ? 0f : 1f;
        isPlaying = !isPlaying;
        gameUI.OnGamePaused();
    }

    #region New System Input
        void OnMove(InputValue value) => vector = value.Get<Vector2>();
        void OnDrop(InputValue value) { if (value.isPressed && currentObject != null) StopAndSpawnNext(); }
        void OnQuit(InputValue value) { if (value.isPressed) SceneManager.LoadScene(0); }
        void OnPause(InputValue value){  
            if (value.isPressed) {
                PauseGame();
            }
        }
    #endregion
}

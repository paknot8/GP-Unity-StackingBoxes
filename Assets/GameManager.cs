using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Serialized fields for easy tweaking in the Unity Editor
    [Header("Object References")]
    [SerializeField] private Transform[] objectPrefabs;
    [SerializeField] private Transform objectHolder;
    [SerializeField] private TMPro.TextMeshProUGUI livesText;
    [SerializeField] private MaxHeightManager maxHeightLine;

    [Header("Public Variables")]
    public float objectMoveSpeed;
    public int startingLives;

    // Private variables
    private Transform currentObject; // Current spawned object
    private Rigidbody2D currentRigidbody; // Rigidbody of the object
    private int livesRemaining;
    private bool isPlaying = true;
    private Vector2 vector;

    void Awake(){
        objectMoveSpeed = 4f;
        startingLives = 3;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetLives();
        UpdateLivesText();
        SpawnNewObject();
    }

    void Update()
    {
        CheckPlaceholderIsEmpty();
    }

    // Spawns a new object with random properties
    private void SpawnNewObject()
    {
        int randomIndex = Random.Range(0, objectPrefabs.Length);
        Transform selectedPrefab = objectPrefabs[randomIndex];
        
        // Calculate spawn position based on camera position and height
        Vector3 cameraPosition = Camera.main.transform.position;
        float cameraHeight = Camera.main.orthographicSize;
        Vector2 spawnPosition = new(cameraPosition.x, cameraPosition.y + cameraHeight - 2f);

        currentObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        currentObject.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        currentRigidbody = currentObject.GetComponent<Rigidbody2D>();
    }

    // Coroutine for delayed object spawning
    private IEnumerator DelaySpawnNewObject()
    {
        maxHeightLine.Disable();
        yield return new WaitForSeconds(1f);
        maxHeightLine.Enable();
        yield return new WaitForSeconds(2f);
        SpawnNewObject();
    }

    // Stops the current object and spawns the next one after a delay
    private void StopAndSpawnNext()
    {
        currentObject = null;
        currentRigidbody.simulated = true;
        StartCoroutine(DelaySpawnNewObject());
    }

    // Moves the current object according to input
    private void ObjectMovement() => currentObject.transform.Translate(objectMoveSpeed * Time.deltaTime * new Vector2(vector.x, vector.y));

    // Checks if the placeholder for the object is empty and moves it if necessary
    private void CheckPlaceholderIsEmpty()
    {
        if (currentObject != null) ObjectMovement();
    }

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

    // Resets life count to starting value
    private void ResetLives() => livesRemaining = startingLives;

    // Updates lives text in UI
    private void UpdateLivesText() => livesText.text = $"{livesRemaining}";

    // Pauses the game
    private void PauseGame()
    {
        Time.timeScale = isPlaying ? 0f : 1f;
        isPlaying = !isPlaying;
    }

    #region New System Input
        void OnMove(InputValue value) => vector = value.Get<Vector2>();

        void OnDrop(InputValue value)
        {
            if (value.isPressed && currentObject != null) StopAndSpawnNext();
        }

        void OnQuit(InputValue value)
        {
            if (value.isPressed) SceneManager.LoadScene(0);
        }

        void OnPause(InputValue value)
        {
            if (value.isPressed) PauseGame();
        }
    #endregion
}

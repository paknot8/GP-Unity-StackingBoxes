using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    #region Variables & References
        [SerializeField] private Transform[] objectPrefabs;
        [SerializeField] private Transform objectHolder;
        [SerializeField] private TMPro.TextMeshProUGUI LivesText;
        [SerializeField] private float objectMoveSpeed = 4f;
        [SerializeField] private int startingLives = 3;
        [SerializeField] private MaxHeight_Manager maxHeightLineReferenceToObject;

        private Transform currentObject; // Current Spawned object
        private Rigidbody2D currentRigidbody; // Gravity of the object

        // Variables
        public Vector2 objectStartPosition = new(0f, 2f);
        private Vector2 vector;
        private Vector2 movement;
        private int livesRemaining;
        private bool isPlaying = true;

        private MainCameraManager mainCameraManager;
        
    #endregion

    void Start()
    {
        Debug.Log("Start Game -> GameManager.cs");
        ResetLives();
        UpdateLivesText();
        SpawnNewObject();
    }

    void Update()
    {
        CheckPlaceHolderIsEmpty();
    }

    // private void SpawnNewObject()
    // {
    //     int randomIndex = Random.Range(0, objectPrefabs.Length);
    //     Transform selectedPrefab = objectPrefabs[randomIndex];
    //     currentObject = Instantiate(selectedPrefab);
    //     currentObject.position = objectStartPosition;
    //     currentObject.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
    //     currentRigidbody = currentObject.GetComponent<Rigidbody2D>();
    // }

    // private void SpawnNewObject()
    // {
    //     int randomIndex = Random.Range(0, objectPrefabs.Length);
    //     Transform selectedPrefab = objectPrefabs[randomIndex];
    //     currentObject = Instantiate(selectedPrefab);
        
    //     // Get the position of the main camera
    //     Vector3 cameraPosition = Camera.main.transform.position;
        
    //     // Calculate the spawn position relative to the camera view
    //     Vector2 spawnPosition = cameraPosition + Vector3.up * 2f;
        
    //     currentObject.position = spawnPosition;
    //     currentObject.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
    //     currentRigidbody = currentObject.GetComponent<Rigidbody2D>();
    // }

    private void SpawnNewObject()
    {
        int randomIndex = Random.Range(0, objectPrefabs.Length);
        Transform selectedPrefab = objectPrefabs[randomIndex];
        currentObject = Instantiate(selectedPrefab);

        // Calculate the position of the objectHolder
        Vector3 cameraPosition = Camera.main.transform.position;
        float cameraHeight = Camera.main.orthographicSize;
        Vector2 spawnPosition = new(cameraPosition.x, cameraPosition.y + cameraHeight - 2f);

        currentObject.position = spawnPosition;
        currentObject.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        currentRigidbody = currentObject.GetComponent<Rigidbody2D>();
    }


    private IEnumerator DelaySpawnNewObject()
    {
        maxHeightLineReferenceToObject.Disable();
        yield return new WaitForSeconds(3f);
        StartCoroutine(DelayHeightLimiter());
        SpawnNewObject();
    }

    private IEnumerator DelayHeightLimiter()
    {
        yield return new WaitForSeconds(2f);
        maxHeightLineReferenceToObject.Enable();
    }

    private void StopAndSpawnNext()
    {
        currentObject = null;
        currentRigidbody.simulated = true;

        StartCoroutine(DelaySpawnNewObject());
    }

    private void ObjectMovement()
    {
        movement = new Vector2(vector.x, vector.y);
        currentObject.transform.Translate(objectMoveSpeed * Time.deltaTime * movement);
    }

    private void CheckPlaceHolderIsEmpty()
    {
        if (currentObject != null) ObjectMovement();
    }

    public void SubstractLifePoint()
    {
        livesRemaining = Mathf.Max(livesRemaining - 1, 0);
        LivesText.text = $"{livesRemaining}";
        if (livesRemaining == 0)
        {
            Debug.Log("You Lost!");
            SceneManager.LoadScene(0);
        }
    }

    private void ResetLives() => livesRemaining = startingLives;
    private void UpdateLivesText() => LivesText.text = $"{livesRemaining}";

    void PauseGame()
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

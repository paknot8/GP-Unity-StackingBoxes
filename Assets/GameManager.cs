using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables & References
        [SerializeField] private Transform[] objectPrefabs;
        [SerializeField] private Transform objectHolder;
        [SerializeField] private TMPro.TextMeshProUGUI LivesText;
        [SerializeField] private float objectMoveSpeed = 3f;
        [SerializeField] private float timeBetweenRounds = 1f;
        [SerializeField] private int startingLives = 3;
        [SerializeField] private MaxHeightManager maxHeightLineReferenceToObject;

        private Vector2 objectStartPosition = new(0f, 2f);
        private Vector2 vector;
        private Vector2 movement;
        private int livesRemaining;
        private bool isPlaying = true;
        private Transform currentObject;
        private Rigidbody2D currentRigidbody;
    #endregion 

    void Awake(){
        
    }

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

    private void SpawnNewObject()
    {
        int randomIndex = Random.Range(0, objectPrefabs.Length);
        Transform selectedPrefab = objectPrefabs[randomIndex];
        currentObject = Instantiate(selectedPrefab, objectHolder);
        currentObject.position = objectStartPosition;
        currentObject.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        currentRigidbody = currentObject.GetComponent<Rigidbody2D>();
    }

    private IEnumerator DelaySpawnNewObject()
    {
        maxHeightLineReferenceToObject.Disable();
        yield return new WaitForSeconds(timeBetweenRounds);
        SpawnNewObject();
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
            // Game over logic
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

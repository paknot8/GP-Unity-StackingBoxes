using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    #region Variables & References
        [Header("UI Objects")]
        [SerializeField] private TMPro.TextMeshProUGUI scoreText;
        [SerializeField] private TMPro.TextMeshProUGUI scoreTextGameOverText;
        [SerializeField] private TMPro.TextMeshProUGUI topScoreText;
        [SerializeField] private TMPro.TextMeshProUGUI topScoreTextPaused;
        [SerializeField] private Image panelImage;
        
        [Header("Script Object References")]
        [SerializeField] private GameManager gameManager;

        [Header("Object References")]
        [SerializeField] private GameObject gameScreenCanvas;
        [SerializeField] private GameObject gamePauseCanvas;
        [SerializeField] private GameObject gameOverCanvas;

        [Header("Sound Effects")]
        [SerializeField] private float transparacy;
        private ScoreManager scoreManager;

        [Header("Sound Effects")]
        [SerializeField] private AudioSource buttonClickSound;
    #endregion

    #region Default Unity Functions
        void Awake()
        {
            transparacy = 0.8f;

            // Get a reference to ScoreManager
            scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
            {
                // Update the score text when the game UI starts
                UpdateScoreText();
            }
        }

        void Start()
        {
            SetPanelTransparency(transparacy);
        }
    #endregion

    private void UpdateScoreText()
    {
        // Load the top score from ScoreManager and update the UI
        int topScore = scoreManager.LoadTopScore();
        topScoreText.text = $"Top Score: {topScore}";
        topScoreTextPaused.text = $"Top Score: {topScore}";
        scoreTextGameOverText.text = $"Score: {gameManager.score}";
    }

    #region UI Buttons
        public void RestartGame()
        {
            ResetValues();
            SceneManager.LoadScene(1); 
        }

        public void ToMainMenu()
        {
            ResetValues();
            SceneManager.LoadScene(0);
        }

        public void ExitGame()
        {
            ResetValues();
            #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
            #else
                    Application.Quit();
            #endif
        }

        private void ResetValues()
        {
            buttonClickSound.Play();
            gameManager.isPlaying = true;
            Time.timeScale = 1;
            OnGamePaused();
        }

        public void OnGamePaused()
        {
            buttonClickSound.Play();
            UpdateScoreText();
            if (!gameManager.isPlaying)
            {
                gameScreenCanvas.SetActive(false);
                gamePauseCanvas.SetActive(true);
            }
            else
            {
                gameScreenCanvas.SetActive(true);
                gamePauseCanvas.SetActive(false);
            }
        }

        public void OnGameOver()
        {
            UpdateScoreText();
            if (!gameManager.isPlaying)
            {
                gameScreenCanvas.SetActive(false);
                gamePauseCanvas.SetActive(false);
                gameOverCanvas.SetActive(true);
            }
        }
    #endregion

    #region Extra Functions
        private void SetPanelTransparency(float alpha)
        {
            Color color = panelImage.color; // Get the color of the panel's image
            color.a = alpha;                // Set the alpha value of the color
            panelImage.color = color;       // Assign the modified color back to the panel's image
        }
    #endregion
}

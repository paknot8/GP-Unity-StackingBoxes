using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Import Unity UI namespace

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private Image panelImage; // Use Image component for UI elements

    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject gameScreenCanvas;
    [SerializeField] private GameObject gamePauseCanvas;
    [SerializeField] private GameObject gameOverCanvas;

    [SerializeField] private AudioSource buttonClickSound;

    [SerializeField] private float transparacy;

    void Awake(){
        transparacy = 0.8f;
    }

    void Start()
    {
        SetPanelTransparency(transparacy);
    }

    private void UpdateScoreText() => scoreText.text = $"Your Score : {gameManager.score}";

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

    private void SetPanelTransparency(float alpha)
    {
        // Get the color of the panel's image
        Color color = panelImage.color;

        // Set the alpha value of the color
        color.a = alpha;

        // Assign the modified color back to the panel's image
        panelImage.color = color;
    }
}

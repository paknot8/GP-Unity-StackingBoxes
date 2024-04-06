using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Import Unity UI namespace

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;
    [SerializeField] private Image panelImage; // Use Image component for UI elements

    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject GameScreenCanvas;
    [SerializeField] private GameObject GamePauseCanvas;

    [SerializeField] private float transparacy;

    void Awake(){
        transparacy = 0.8f;
    }

    void Start()
    {
        SetPanelTransparency(transparacy);
    }

    void Update()
    {
        
    }

    private void UpdateScoreText() => scoreText.text = $"My Score: {gameManager.score}";

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

    private void ResetValues()
    {
        gameManager.isPlaying = true;
        Time.timeScale = 1;
        OnGamePaused();
    }

    public void OnGamePaused()
    {
        UpdateScoreText();
        if (!gameManager.isPlaying)
        {
            GameScreenCanvas.SetActive(false);
            GamePauseCanvas.SetActive(true);
        }
        else
        {
            GameScreenCanvas.SetActive(true);
            GamePauseCanvas.SetActive(false);
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

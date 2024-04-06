using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject GameScreenCanvas;
    [SerializeField] private GameObject GamePauseCanvas;
    // [SerializeField] private GameObject GameOverCanvas;

    void Update(){
        UpdateScoreText();
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

    private void ResetValues(){
        gameManager.isPlaying = true;
        Time.timeScale = 1;
        OnGamePaused();
    }

    public void OnGamePaused(){
        if(!gameManager.isPlaying){
            GameScreenCanvas.SetActive(false);
            GamePauseCanvas.SetActive(true);
        } else {
            GameScreenCanvas.SetActive(true);
            GamePauseCanvas.SetActive(false);
        }
    }
}

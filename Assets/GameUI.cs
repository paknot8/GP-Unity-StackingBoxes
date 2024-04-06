using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject GameScreenCanvas;
    [SerializeField] private GameObject GamePauseCanvas;
    // [SerializeField] private GameObject GameOverCanvas;

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
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

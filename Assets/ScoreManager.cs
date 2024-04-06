using UnityEngine;

public class ScoreManager : MonoBehaviour
{   
    [SerializeField] private GameManager gameManager;
    private const string ScoreKey = "Score"; // Key for saving and loading the score

    // Method to save the score
    public void SaveScore()
    {
        PlayerPrefs.SetInt(ScoreKey, gameManager.score);
        PlayerPrefs.Save();
    }

    // Method to load the score
    public void LoadScore()
    {
        if (PlayerPrefs.HasKey(ScoreKey))
        {
            gameManager.score = PlayerPrefs.GetInt(ScoreKey);
            // gameManager.UpdateTopScoreText(); // Update the score text after loading
        }
    }

    // Method to reset the score
    public void ResetScore()
    {
        gameManager.score = 0;
        //gameManager.UpdateScoreText();
    }
}

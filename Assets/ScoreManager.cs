using UnityEngine;

public class ScoreManager : MonoBehaviour
{   
    public int passedScore = 0;
    private const string ScoreKey = "Score"; // Key for saving and loading the score

    // Method to save the score
    public void SaveScore(int score)
    {
        passedScore = score;
        PlayerPrefs.SetInt(ScoreKey, score);
        PlayerPrefs.Save();
    }

    // Method to load the score
    public void LoadScore()
    {
        if (PlayerPrefs.HasKey(ScoreKey))
        {
            passedScore = PlayerPrefs.GetInt(ScoreKey);
            // gameManager.UpdateTopScoreText(); // Update the score text after loading
        }
    }

    // Method to reset the score
    public void ResetScore()
    {
        passedScore = 0;
        //gameManager.UpdateScoreText();
    }
}

using UnityEngine;

public class ScoreManager : MonoBehaviour
{   
    [HideInInspector] public int passedScore;
    private const string TopScoreKey = "TopScore"; // Key for saving and loading the top score

    // Method to save the top score
    public void SaveTopScore(int topScore)
    {
        // Load the current top score
        int currentTopScore = LoadTopScore();

        // Compare the new top score with the current top score
        if (topScore > currentTopScore)
        {
            // If the new score is higher, update the top score
            PlayerPrefs.SetInt(TopScoreKey, topScore);
            PlayerPrefs.Save();
        }
    }

    // Method to load the top score
    public int LoadTopScore()
    {
        if (PlayerPrefs.HasKey(TopScoreKey))
        {
            return PlayerPrefs.GetInt(TopScoreKey);
        }
        return 0; // If no top score saved yet, return 0
    }

    // Method to reset the score
    public void ResetScore()
    {
        passedScore = 0;
    }
}

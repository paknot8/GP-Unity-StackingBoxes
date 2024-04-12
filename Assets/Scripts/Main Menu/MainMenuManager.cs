using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // References
    public SettingsManager settingsManager;
    public ScoreManager scoreManager; // Reference to ScoreManager

    [SerializeField] private GameObject mainMenuToggle; // GameObject to enable/disable

    [SerializeField] private AudioSource buttonClickSound;

    [SerializeField] private TMPro.TextMeshProUGUI topScoreText;

    // Called when the main menu is loaded
    void Start()
    {
        // Load and display the top score
        if (scoreManager != null && topScoreText != null)
        {
            int topScore = scoreManager.LoadTopScore();
            topScoreText.text = $"Top Score: {topScore}";
        }
    }

    public void PlayGame()
    {
        buttonClickSound.Play();
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        buttonClickSound.Play();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void GoToSettings()
    {
        buttonClickSound.Play();
        if (settingsManager != null)
        {
            mainMenuToggle.SetActive(false);
            settingsManager.EnableObject();
        }
        else
        {
            Debug.LogWarning("Settings Manager not assigned.");
        }
    }

    public void BackToMainMenu()
    {
        buttonClickSound.Play();
        if (mainMenuToggle != null)
        {
            mainMenuToggle.SetActive(true);
            settingsManager.DisableObject();
        }
        else
        {
            Debug.LogWarning("No object assigned to toggle.");
        }
    }
}

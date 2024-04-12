using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("References to Objects Scripts")]
    public SettingsManager settingsManager;
    public ScoreManager scoreManager;

    [Header("References to Objects")]
    [SerializeField] private GameObject mainMenuToggle; // GameObject to enable/disable
    [SerializeField] private AudioSource buttonClickSound;
    [SerializeField] private TMPro.TextMeshProUGUI topScoreText;

    #region Default Unity Functions
    void Start()
        {
            // Load and display the top score
            if (scoreManager != null && topScoreText != null)
            {
                int topScore = scoreManager.LoadTopScore();
                topScoreText.text = $"Top Score: {topScore}";
            }
        }
    #endregion

    #region Buttons
        public void PlayGame()
        {
            buttonClickSound.Play();
            SceneManager.LoadScene(1);
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

        public void QuitGame()
        {
            buttonClickSound.Play();
            #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
            #else
                    Application.Quit();
            #endif
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
    #endregion
}

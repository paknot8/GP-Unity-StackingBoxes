using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // References
    public SettingsManager settingsManager; 
    public ScoreManager scoreManager;

    [SerializeField] private GameObject mainMenuToggle; // GameObject to enable/disable
    [SerializeField] private TMPro.TextMeshProUGUI topScoreText;

    void Start(){

    }
    void Update(){
        scoreManager.LoadScore();
    }

    private void UpdateScoreText() => topScoreText.text = $"Your Top Score : {scoreManager.passedScore}";

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void GoToSettings()
    {
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

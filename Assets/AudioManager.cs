using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Slider musicSlider; // Slider for controlling music volume
    [SerializeField] Slider soundSlider; // Slider for controlling sound effects volume

    void Start()
    {
        LoadVolumeLevels();
    }

    private void LoadVolumeLevels()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
        }
        if (!PlayerPrefs.HasKey("soundVolume"))
        {
            PlayerPrefs.SetFloat("soundVolume", 1);
        }
        Load();
    }

    // Method to change the volume of all AudioSources tagged as "Music" based on the music slider value
    public void ChangeMusicVolume()
    {
        foreach (var audioSource in FindObjectsOfType<AudioSource>())
        {
            if (audioSource.CompareTag("Music"))
            {
                audioSource.volume = musicSlider.value;
            }
        }
        Save();
    }

    // Method to change the volume of all AudioSources tagged as "Sound" based on the sound slider value
    public void ChangeSoundVolume()
    {
        foreach (var audioSource in FindObjectsOfType<AudioSource>())
        {
            if (audioSource.CompareTag("Sound"))
            {
                audioSource.volume = soundSlider.value;
            }
        }
        Save(); // Save the changes made to the sound volume
    }

    // Load volume settings from PlayerPrefs
    private void Load()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        soundSlider.value = PlayerPrefs.GetFloat("soundVolume");
    }

    // Save volume settings to PlayerPrefs
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("soundVolume", soundSlider.value);
    }
}

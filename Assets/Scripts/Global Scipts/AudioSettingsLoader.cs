using UnityEngine;

public class AudioSettingsLoader : MonoBehaviour
{
    void Start()
    {
        LoadVolumeLevels();
    }

    void LoadVolumeLevels()
    {
        // Load volume settings from PlayerPrefs
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1);
        float soundVolume = PlayerPrefs.GetFloat("soundVolume", 1);

        // Apply loaded music volume to objects tagged as "Music" and "Sound" in the scene
        ApplyVolumeToTaggedObjects("Music", musicVolume);
        ApplyVolumeToTaggedObjects("Sound", soundVolume);
    }

    void ApplyVolumeToTaggedObjects(string tag, float volume)
    {
        // Find all objects with the specified tag
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);

        // Apply volume settings to all AudioSources in the tagged objects
        foreach (GameObject obj in taggedObjects)
        {
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = volume;
            }
        }
    }
}

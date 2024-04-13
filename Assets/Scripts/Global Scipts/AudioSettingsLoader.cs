using UnityEngine;

public class AudioSettingsLoader : MonoBehaviour
{
    void Start()
    {
        LoadMusicLevels();
        LoadSoundLevels();
    }

    void LoadMusicLevels()
    {
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1);
        ApplyVolumeToTaggedObjects("Music", musicVolume);
    }

    void LoadSoundLevels()
    {
        float soundVolume = PlayerPrefs.GetFloat("soundVolume", 1);
        ApplyVolumeToTaggedObjects("Sound", soundVolume);
    }

    void ApplyVolumeToTaggedObjects(string tag, float volume)
    {
        // Find all objects with the specified tag
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);

        // Apply volume settings to all AudioSources in the tagged objects
        foreach (GameObject obj in taggedObjects)
        {
            if (obj.TryGetComponent<AudioSource>(out var audioSource))
            {
                audioSource.volume = volume;
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenResolution : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown; // Dropdown UI element for selecting resolution

    private Resolution[] resolutions; // Array to store available screen resolutions
    private List<Resolution> filteredResolutions; // List to store filtered screen resolutions based on refresh rate

    private int currentRefreshRate; // Current refresh rate of the screen
    private int currentResolutionIndex = 0; // Index of the current resolution selected

    [System.Obsolete]
    void Start()
    {
        // Call method to load and set resolution
        LoadResolution();

        // Add listener to resolution dropdown
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    [System.Obsolete]
    private void LoadResolution()
    {
        // Get all available screen resolutions
        resolutions = Screen.resolutions;

        // Initialize list to store filtered resolutions
        filteredResolutions = new List<Resolution>();

        // Clear existing options in the resolution dropdown
        resolutionDropdown.ClearOptions();

        // Get current refresh rate of the screen
        currentRefreshRate = Screen.currentResolution.refreshRate;

        // Check if resolution is set in PlayerPrefs, otherwise use current screen resolution
        int screenWidth = PlayerPrefs.GetInt("ScreenWidth", Screen.currentResolution.width);
        int screenHeight = PlayerPrefs.GetInt("ScreenHeight", Screen.currentResolution.height);

        // Filter resolutions based on current refresh rate
        foreach (Resolution res in resolutions)
        {
            if (res.refreshRate == currentRefreshRate)
            {
                filteredResolutions.Add(res); // Add resolutions with matching refresh rate to filtered list
            }
        }

        List<string> options = new List<string>(); // List to store dropdown options

        // Populate dropdown options
        foreach (Resolution res in filteredResolutions)
        {
            string aspectRatio = GetAspectRatio(res.width, res.height); // Get aspect ratio of the resolution
            string resolutionOption = res.width + "x" + res.height + " (" + aspectRatio + ") " + res.refreshRate + " Hz"; // Format resolution option
            options.Add(resolutionOption); // Add formatted option to list

            // Check if resolution matches PlayerPrefs or current screen resolution
            if (res.width == screenWidth && res.height == screenHeight)
            {
                currentResolutionIndex = filteredResolutions.IndexOf(res); // Set index of current resolution
            }
        }

        // Add options to the resolution dropdown
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex; // Set dropdown value to current resolution index
        resolutionDropdown.RefreshShownValue(); // Refresh dropdown display

        // Set the resolution based on PlayerPrefs or current screen resolution
        Screen.SetResolution(screenWidth, screenHeight, true);
    }

    // Method to calculate aspect ratio of a resolution
    private string GetAspectRatio(int width, int height)
    {
        float aspectRatio = (float)width / height; // Calculate aspect ratio
        string aspectRatioString = aspectRatio.ToString("0.##"); // Format aspect ratio as string
        return aspectRatioString; // Return formatted aspect ratio
    }

    // Method called when a resolution is selected from the dropdown
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex]; // Get selected resolution
        Screen.SetResolution(resolution.width, resolution.height, true); // Set screen resolution

        // Save selected resolution to PlayerPrefs
        PlayerPrefs.SetInt("ScreenWidth", resolution.width);
        PlayerPrefs.SetInt("ScreenHeight", resolution.height);
        PlayerPrefs.Save(); // Save PlayerPrefs to disk
    }

    // Method called when the application is quitting
    private void OnApplicationQuit()
    {
        SaveResolution();
    }

    // Method called when the application is paused (e.g., switching to another app)
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveResolution();
        }
    }

    // Method to save resolution settings to PlayerPrefs
    private void SaveResolution()
    {
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;
        PlayerPrefs.SetInt("ScreenWidth", screenWidth);
        PlayerPrefs.SetInt("ScreenHeight", screenHeight);
        PlayerPrefs.Save(); // Save PlayerPrefs to disk
    }

    // Method called when the resolution is changed from the dropdown
    private void OnResolutionChanged(int resolutionIndex)
    {
        SetResolution(resolutionIndex);
    }
}

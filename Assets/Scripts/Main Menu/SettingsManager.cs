using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    // GameObject to enable/disable
    public GameObject SettingsToggle;

    // Method to disable the object
    public void DisableObject()
    {
        if (SettingsToggle != null)
        {
            SettingsToggle.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No object assigned to toggle.");
        }
    }

    // Method to enable the object
    public void EnableObject()
    {
        if (SettingsToggle != null)
        {
            SettingsToggle.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No object assigned to toggle.");
        }
    }
}

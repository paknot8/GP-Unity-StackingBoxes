using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    // GameObject to enable/disable
    public GameObject SettingsToggle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

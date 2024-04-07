using UnityEngine;

public class KeybindingLoader : MonoBehaviour
{
    // Reference to the Keybinding script
    public Keybinding keybindingScript;

    // Start is called before the first frame update
    void Start()
    {
        // Load keybindings from Keybinding script
        keybindingScript.LoadKeybindings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

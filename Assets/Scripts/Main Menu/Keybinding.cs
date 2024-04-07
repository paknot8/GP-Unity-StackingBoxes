using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Keybinding : MonoBehaviour
{
    public InputActionReference moveLeftAction;
    public InputActionReference moveRightAction;
    public InputActionReference dropAction;

    public TMP_InputField moveLeftInputField;
    public TMP_InputField moveRightInputField;
    public TMP_InputField dropInputField;

    private void Start()
    {
        // Load saved keybindings
        LoadKeybindings();
        dropInputField.text = dropAction.action.GetBindingDisplayString();
    }

    private void Update()
    {
        // Check for drop action
        if (dropAction.action.triggered)
        {
            OnDrop();
        }
    }

    public void OnMoveLeftInputChange(string newKey)
    {
        RebindAction(moveLeftInputField, moveLeftAction, newKey);
    }

    public void OnMoveRightInputChange(string newKey)
    {
        RebindAction(moveRightInputField, moveRightAction, newKey);
    }

    public void OnDropInputChange(string newKey)
    {
        RebindAction(dropInputField, dropAction, newKey);
    }

    private void RebindAction(TMP_InputField inputField, InputActionReference actionReference, string newKey)
    {
        inputField.text = newKey;
        actionReference.action.Disable();
        actionReference.action.ApplyBindingOverride(newKey);
        actionReference.action.Enable();
        SaveKeybindings();
    }

    private void OnDrop()
    {
        // Handle drop action here
    }

    public void LoadKeybindings()
    {
        // Load keybindings from player preferences or wherever they are saved
        moveLeftInputField.text = PlayerPrefs.GetString("MoveLeftKey");
        moveRightInputField.text = PlayerPrefs.GetString("MoveRightKey");
        Debug.Log(moveLeftInputField.text);
        Debug.Log(moveRightInputField.text);
        // Load drop keybinding from saved data
    }

    public void SaveKeybindings()
    {
        // Save keybindings to player preferences or wherever you want to save them
        PlayerPrefs.SetString("MoveLeftKey", moveLeftInputField.text);
        PlayerPrefs.SetString("MoveRightKey", moveRightInputField.text);

        Debug.Log(PlayerPrefs.GetString("MoveLeftKey"));
        Debug.Log(PlayerPrefs.GetString("MoveRightKey"));

        // Save drop keybinding
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        // Save keybindings when the application quits
        SaveKeybindings();
    }
}

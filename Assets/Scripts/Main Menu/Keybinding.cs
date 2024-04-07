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

        // Update UI to display current keybindings
        moveLeftInputField.text = "A"; // Set left key to "A"
        moveRightInputField.text = "D"; // Set right key to "D"
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
        actionReference.action.ApplyBindingOverride(0, newKey);
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
        moveLeftInputField.text = PlayerPrefs.GetString("MoveLeftKey", "A");
        moveRightInputField.text = PlayerPrefs.GetString("MoveRightKey", "D");
        // Load drop keybinding from saved data
    }

    private void SaveKeybindings()
    {
        // Save keybindings to player preferences or wherever you want to save them
        PlayerPrefs.SetString("MoveLeftKey", moveLeftInputField.text);
        PlayerPrefs.SetString("MoveRightKey", moveRightInputField.text);
        // Save drop keybinding
        PlayerPrefs.Save();
    }
}

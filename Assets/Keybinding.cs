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
        moveLeftInputField.text = moveLeftAction.action.GetBindingDisplayString();
        moveRightInputField.text = moveRightAction.action.GetBindingDisplayString();
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

    public void OnMoveLeftInputChange()
    {
        RebindAction(moveLeftInputField, moveLeftAction);
    }

    public void OnMoveRightInputChange()
    {
        RebindAction(moveRightInputField, moveRightAction);
    }

    public void OnDropInputChange()
    {
        RebindAction(dropInputField, dropAction);
    }

    private void RebindAction(TMP_InputField inputField, InputActionReference actionReference)
    {
        inputField.text = "Press any key...";
        actionReference.action.Disable();
        actionReference.action.performed += ctx =>
        {
            var newKey = ctx.ReadValueAsButton();
            if (newKey)
            {
                actionReference.action.ApplyBindingOverride(0, newKey.ToString());
                inputField.text = actionReference.action.GetBindingDisplayString();
                actionReference.action.Enable();
            }
        };
        actionReference.action.Enable();
    }

    private void OnDrop()
    {
        // Handle drop action here
    }

    private void LoadKeybindings()
    {
        // Load keybindings from player preferences or wherever they are saved
    }

    private void SaveKeybindings()
    {
        // Save keybindings to player preferences or wherever you want to save them
    }
}

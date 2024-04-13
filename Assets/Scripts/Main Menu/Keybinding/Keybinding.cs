using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class Keybinding : MonoBehaviour
{
    // --- Player Input --- //
    private PlayerInput input;
    private RebindingOperation rebindingOperation;

    // --- Scriptable Object --- //
    private KeybindingSO currentBinding;
    private InputAction currentAction;

    // --- Showing all buttons currently on the screen --- //
    [Header("Shows on Start")]
    public Button[] allButtons;

    void Start()
    {
        if (!TryGetComponent<PlayerInput>(out input))
        {
            Debug.LogError("PlayerInput component not found.");
            return;
        }

        allButtons = FindObjectsByType<Button>(FindObjectsSortMode.InstanceID);
        if (allButtons == null || allButtons.Length == 0)
        {
            Debug.LogError("No buttons found.");
            return;
        }

        if (PlayerPrefs.HasKey("controls"))
        {
            input.actions.LoadBindingOverridesFromJson(PlayerPrefs.GetString("controls"));
        }

        UpdateAllButtons();
    }

    public void RebindButton(Button button)
    {
        SetBinding(button);

        // Disable the action before rebinding
        currentAction.Disable();

        rebindingOperation = currentAction.PerformInteractiveRebinding().WithTargetBinding(currentBinding.compositeNumber)
            .WithBindingGroup("Keyboard&Mouse").WithControlsHavingToMatchPath("<Keyboard>")
            .WithControlsHavingToMatchPath("<Mouse>").WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f).OnPotentialMatch(operation => CheckBinding())
            .OnComplete(operation => { RebindComplete(button); }).OnCancel(operation => { RebindCancel(button); });

        button.GetComponentInChildren<TMP_Text>().text = "listening...";
        DisableAllButtons();
        rebindingOperation.Start();
    }

    private void CheckBinding()
    {
        string displayName = rebindingOperation.selectedControl.displayName;
        string shortDisplayName = rebindingOperation.selectedControl.shortDisplayName;

        // Iterate through all action maps in the PlayerInput
        foreach (var actionMap in input.actions.actionMaps)
        {
            // Iterate through all bindings in the action map
            foreach (var binding in actionMap.bindings)
            {
                // Check if the binding matches the selected control's display name or short display name
                if (binding.ToDisplayString().Equals(displayName) || binding.ToDisplayString().Equals(shortDisplayName))
                {
                    // If the control is already bound to another action, cancel the rebinding operation
                    rebindingOperation.Cancel();
                    return;
                }
            }
        }
        // If no conflicting bindings are found, complete the rebinding operation
        rebindingOperation.Complete();
    }

    private void UpdateButton(Button button)
    {
        button.GetComponentInChildren<TMP_Text>().text = button.GetComponent<AssignedBinding>().binding.GetBinding(input);
    }

    private void UpdateAllButtons()
    {
        foreach (Button button in allButtons)
        {
            if(!button.name.Equals("Back"))
            {
                button.GetComponentInChildren<TMP_Text>().text = button.GetComponent<AssignedBinding>().binding.GetBinding(input);
            }
        }
    }

    private void RebindComplete(Button button)
    {
        rebindingOperation.Dispose();
        UpdateButton(button);
        PlayerPrefs.SetString("controls", input.actions.SaveBindingOverridesAsJson());

        // Re-enable the action
        currentAction.Enable();

        EnableAllButtons();
    }

    private void RebindCancel(Button button)
    {
        rebindingOperation.Dispose();
        UpdateButton(button);

        // Re-enable the action
        currentAction.Enable();

        EnableAllButtons();
    }

    private void DisableAllButtons()
    {
        foreach (Button button in allButtons)
        {
            button.interactable = false;
        }
    }
    
    private void EnableAllButtons()
    {
        foreach (Button button in allButtons)
        {
            button.interactable = true;
        }
    }

    private void SetBinding(Button button)
    {
        currentBinding = button.GetComponent<AssignedBinding>().binding;
        SetAction(currentBinding.actionName);
    }
    private void SetAction(string actionName)
    {
        currentAction = input.actions.FindAction(actionName);
    }
}

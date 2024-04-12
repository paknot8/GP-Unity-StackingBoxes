using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class Keybinding : MonoBehaviour
{
    private PlayerInput input;
    private RebindingOperation rebindingOperation;

    private KeybindingSO currentBinding;
    private InputAction currentAction;

    public Button[] allButtons;

    void Start()
    {
        input = GetComponent<PlayerInput>();
        if (input == null)
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

        //foreach (var binding in input.actions.bindings)
        foreach (var control in input.actions.bindings)
        {
            if(control.groups.Contains("Keyboard&Mouse"))
            {
                if (control.ToDisplayString().Equals(displayName) || control.ToDisplayString().Equals(shortDisplayName))
                {
                    rebindingOperation.Cancel();
                    break;
                }
            }
        }
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

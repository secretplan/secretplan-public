using Godot;

namespace SecretPlanGodot.Core;

public static class InputUtilities
{
    /// <summary>
    ///     Gets all bound actions and events
    /// </summary>
    public static IEnumerable<(string actionName, InputEvent[] inputEvent)> AllBoundActionsAndEvents()
    {
        foreach (var actionName in GetAllGameplayActions())
        {
            if (actionName.StartsWith("ui_") || actionName.StartsWith("__debug"))
            {
                // ignore built-in actions and actions that come from FreeCam
                continue;
            }

            var inputEvents = GetInputEventsForAction(actionName);

            yield return (actionName, inputEvents);
        }
    }

    public static InputEvent[] GetInputEventsForAction(string actionName)
    {
        return InputMap.ActionGetEvents(actionName)?.ToArray() ?? [];
    }

    public static IEnumerable<string> GetAllGameplayActions()
    {
        foreach (var actionName in InputMap.GetActions())
        {
            if (actionName.ToString().StartsWith("ui_") || actionName.ToString().StartsWith("__debug"))
            {
                // ignore built-in actions and actions that come from FreeCam
                continue;
            }

            yield return actionName;
        }
    }
}
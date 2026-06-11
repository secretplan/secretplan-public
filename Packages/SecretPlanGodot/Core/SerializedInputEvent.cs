using Godot;
using Newtonsoft.Json;

namespace SecretPlanGodot.Core;

public readonly record struct SerializedInputEvent
{
    [JsonProperty("type")]
    public SerializedInputEventType Type { get; init; }

    [JsonProperty("keycode")]
    public Key PhysicalKeycode { get; init; }

    [JsonProperty("mouse_button")]
    public MouseButton MouseButton { get; init; }

    [JsonProperty("joy_button")]
    public JoyButton JoyButton { get; init; }

    [JsonProperty("joy_axis")]
    public JoyAxis JoyAxis { get; init; }

    [JsonProperty("axis_value")]
    public float AxisValue { get; init; }

    public static SerializedInputEvent Unbound { get; } = new();

    public static SerializedInputEvent FromInputEvent(InputEvent? inputEvent, bool normalizeAnalogueInput)
    {
        return FromInputEventNullable(inputEvent, normalizeAnalogueInput) ?? Unbound;
    }

    public static SerializedInputEvent? FromInputEventNullable(InputEvent? inputEvent, bool normalizeAnalogueInput)
    {
        return inputEvent switch
        {
            InputEventKey key => new SerializedInputEvent
            {
                Type = SerializedInputEventType.Key,
                PhysicalKeycode = key.PhysicalKeycode
            },
            InputEventMouseButton mouseButton => new SerializedInputEvent
            {
                Type = SerializedInputEventType.MouseButton,
                MouseButton = mouseButton.ButtonIndex
            },
            InputEventJoypadButton joypadButton => new SerializedInputEvent
            {
                Type = SerializedInputEventType.JoyButton,
                JoyButton = joypadButton.ButtonIndex
            },
            InputEventJoypadMotion joypadMotion => new SerializedInputEvent
            {
                Type = SerializedInputEventType.JoyAxis,
                JoyAxis = joypadMotion.Axis,
                AxisValue = normalizeAnalogueInput ? MathF.Sign(joypadMotion.AxisValue) : joypadMotion.AxisValue
            },
            _ => null
        };
    }

    public InputEvent? DeserializeToEvent()
    {
        return Type switch
        {
            SerializedInputEventType.Key => new InputEventKey { PhysicalKeycode = PhysicalKeycode },
            SerializedInputEventType.MouseButton => new InputEventMouseButton { ButtonIndex = MouseButton },
            SerializedInputEventType.JoyButton => new InputEventJoypadButton { ButtonIndex = JoyButton },
            SerializedInputEventType.JoyAxis => new InputEventJoypadMotion
            {
                Axis = JoyAxis, AxisValue = AxisValue
            },
            _ => null
        };
    }

    public string HumanReadableString()
    {
        return Type switch
        {
            SerializedInputEventType.Key => OS.GetKeycodeString(
                DisplayServer.KeyboardGetKeycodeFromPhysical(PhysicalKeycode)),
            SerializedInputEventType.MouseButton => HumanReadableMouseButton(MouseButton),
            SerializedInputEventType.JoyButton => JoyButton.ToString(),
            SerializedInputEventType.JoyAxis => $"{JoyAxis}",
            SerializedInputEventType.Unbound => "Unbound",
            _ => "[NOT_BOUND]"
        };
    }

    private string HumanReadableMouseButton(MouseButton mouseButton)
    {
        switch (mouseButton)
        {
            case MouseButton.Left:
                return "Left Click";
            case MouseButton.Right:
                return "Right Click";
            case MouseButton.Middle:
                return "Middle Click";
            case MouseButton.WheelUp:
                return "Scroll Up";
            case MouseButton.WheelDown:
                return "Scroll Down";
            case MouseButton.WheelLeft:
                return "Scroll Left";
            case MouseButton.WheelRight:
                return "Scroll Right";
            case MouseButton.Xbutton1:
                return "Xbutton1"; // idk what these are
            case MouseButton.Xbutton2:
                return "Xbutton2";
            case MouseButton.None:
            default:
                return "???";
        }
    }

    public bool MatchesInputMode(NavigationMode navigationMode)
    {
        return Type switch
        {
            SerializedInputEventType.Key => navigationMode is NavigationMode.Keyboard or NavigationMode.Mouse,
            SerializedInputEventType.MouseButton => navigationMode is NavigationMode.Keyboard or NavigationMode.Mouse,
            SerializedInputEventType.JoyButton => navigationMode is NavigationMode.Gamepad,
            SerializedInputEventType.JoyAxis => navigationMode is NavigationMode.Gamepad,
            _ => false
        };
    }
}
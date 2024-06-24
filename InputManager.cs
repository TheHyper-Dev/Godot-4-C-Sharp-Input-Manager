using System;
using Godot;

public static class InputManager
{
    public static bool Active = true;

    public enum InputType
    {
        Key,
        MouseMotion,
        MouseButton,
        LeftJoytickMotion,
        RightJoytickMotion,
        LeftTriggerMotion,
        RightTriggerMotion,
        GamePadButton,
        Other
    }
    public static InputType inputType { get; private set; }
    public static InputEvent inputEvent { get; private set; }
    public static InputEventKey inputEventKey { get; private set; }
    public static InputEventMouseButton inputEventMouseButton { get; private set; }
    public static InputEventMouseMotion inputEventMouseMotion { get; private set; }
    public static InputEventJoypadMotion inputEventJoypadMotion { get; private set; }
    public static InputEventJoypadButton inputEventJoypadButton { get; private set; }
    public static InputEventScreenDrag inputEventScreenDrag { get; private set; }
    public static InputEventScreenTouch inputEventScreenTouch { get; private set; }
    public static bool isPressed { get; private set; }
    public static bool isEcho { get; private set; } = false;
    public static Key keyCode { get; private set; }
    public static Vector2 relative { get; private set; } = Vector2.Zero;
    public static MouseButton mouseButton { get; private set; }
    public static JoyAxis joyAxis { get; private set; }
    public static float joyAxisValue { get; private set; }
    public static JoyButton joyButton { get; private set; }
    public static Vector2 LeftJoystickVector => _LeftJoystickVector;
    private static Vector2 _LeftJoystickVector = Vector2.Zero;
    public static Vector2 RightJoystick => _RightJoystick;
    private static Vector2 _RightJoystick = Vector2.Zero;
    public static float LeftTrigger { get; private set; } = 0f;
    public static float RightTrigger { get; private set; } = 0f;

    public static KeyModifierMask modifierMask => _modifierMask;
    private static KeyModifierMask _modifierMask = KeyModifierMask.CodeMask;
    public static event Action
    onKey,
    onMouseMotion,
    onMouseButton,
    onJoypadMotion,
    onJoypadButton,
    onScreenDrag,
    onScreenTouch;

    public static void SetInputs(InputEvent input)
    {
        if (!Active) return;

        Type input_type = input.GetType();

        if (input_type == typeof(InputEventKey))
        {
            inputEventKey = (InputEventKey)input;
            inputType = InputType.Key;
            keyCode = inputEventKey.PhysicalKeycode;
            isPressed = input.IsPressed();
            isEcho = input.IsEcho();
            _modifierMask = inputEventKey.GetModifiersMask();
            onKey?.Invoke();
        }
        else if (input_type == typeof(InputEventMouseMotion))
        {
            inputType = InputType.MouseMotion;
            inputEventMouseMotion = (InputEventMouseMotion)input;
            relative = inputEventMouseMotion.Relative;
            _modifierMask = inputEventMouseMotion.GetModifiersMask();
            onMouseMotion?.Invoke();
        }
        else if (input_type == typeof(InputEventMouseButton))
        {
            inputType = InputType.MouseButton;
            inputEventMouseButton = (InputEventMouseButton)input;
            mouseButton = inputEventMouseButton.ButtonIndex;
            isPressed = input.IsPressed();
            _modifierMask = inputEventMouseButton.GetModifiersMask();
            onMouseButton?.Invoke();
        }
        else if (input_type == typeof(InputEventJoypadMotion))
        {
            inputEventJoypadMotion = (InputEventJoypadMotion)input;
            joyAxis = inputEventJoypadMotion.Axis;
            joyAxisValue = inputEventJoypadMotion.AxisValue;
            switch (joyAxis)
            {
                case JoyAxis.LeftX:
                    _LeftJoystickVector.X = joyAxisValue;
                    inputType = InputType.LeftJoytickMotion;
                    break;
                case JoyAxis.LeftY:
                    _LeftJoystickVector.Y = joyAxisValue;
                    inputType = InputType.LeftJoytickMotion;
                    break;
                case JoyAxis.RightX:
                    _RightJoystick.X = joyAxisValue;
                    inputType = InputType.RightJoytickMotion;
                    break;
                case JoyAxis.RightY:
                    _RightJoystick.Y = joyAxisValue;
                    inputType = InputType.RightJoytickMotion;
                    break;
                case JoyAxis.TriggerLeft:
                    LeftTrigger = joyAxisValue;
                    inputType = InputType.LeftTriggerMotion;
                    break;
                case JoyAxis.TriggerRight:
                    RightTrigger = joyAxisValue;
                    inputType = InputType.RightTriggerMotion;
                    break;
                default: return;
            }
            onJoypadMotion?.Invoke();
        }
        else if (input_type == typeof(InputEventJoypadButton))
        {
            inputType = InputType.GamePadButton;
            inputEventJoypadButton = (InputEventJoypadButton)input;
            joyButton = inputEventJoypadButton.ButtonIndex;
            isPressed = input.IsPressed();
            onJoypadButton?.Invoke();
        }
        else if (input_type == typeof(InputEventScreenDrag))
        {
            inputEventScreenDrag = (InputEventScreenDrag)input;
            onScreenDrag?.Invoke();

        }
        else if (input_type == typeof(InputEventScreenTouch))
        {
            inputEventScreenTouch = (InputEventScreenTouch)input;
            isPressed = input.IsPressed();
            onScreenTouch?.Invoke();
        }
        else if (input_type == typeof(InputEventScreenTouch))
        {
            inputEventScreenTouch = (InputEventScreenTouch)input;
            isPressed = input.IsPressed();
            onScreenTouch?.Invoke();
        }
    }
    public static void ResetEvents()
    {
        onKey = null;
        onMouseMotion = null;
        onMouseButton = null;
        onJoypadMotion = null;
        onJoypadButton = null;
        onScreenDrag = null;
        onScreenTouch = null;
    }
}

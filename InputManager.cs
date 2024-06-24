using System;
using Godot;

public static class InputData
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
    public static InputType inputType;
    public static InputEvent inputEvent;
    public static InputEventKey inputEventKey;
    public static InputEventMouseButton inputEventMouseButton;
    public static InputEventMouseMotion inputEventMouseMotion;
    public static InputEventJoypadMotion inputEventJoypadMotion;
    public static InputEventJoypadButton inputEventJoypadButton;
    public static InputEventScreenDrag inputEventScreenDrag;
    public static InputEventScreenTouch inputEventScreenTouch;
    public static bool isPressed;
    public static bool isEcho = false;
    public static Key keyCode;
    public static Vector2 relative = Vector2.Zero;
    public static MouseButton mouseButton;
    public static JoyAxis joyAxis;
    public static float joyAxisValue;
    public static JoyButton joyButton;
    public static Vector2 LeftJoystickVector = Vector2.Zero;
    public static Vector2 RightJoystick = Vector2.Zero;
    public static float LeftTrigger = 0f;
    public static float RightTrigger = 0f;

    public static KeyModifierMask modifierMask = KeyModifierMask.CodeMask;
    public static Action
    onKey,
    onMouseMotion,
    onMouseButton,
    onJoypadMotion,
    onJoypadButton,
    onScreenDrag,
    onScreenTouch;

    public static void SetInputs(in InputEvent input)
    {
        if (!Active) return;

        Type input_type = input.GetType();

        if (input_type == typeof(InputEventKey))
        {
            inputEventKey = (InputEventKey)input;
            inputType = InputType.Key;
            keyCode = inputEventKey.GetPhysicalKeycodeWithModifiers();
            isPressed = input.IsPressed();
            isEcho = input.IsEcho();
            modifierMask = inputEventKey.GetModifiersMask();
            onKey?.Invoke();
        }
        else if (input_type == typeof(InputEventMouseMotion))
        {
            inputType = InputType.MouseMotion;
            inputEventMouseMotion = (InputEventMouseMotion)input;
            relative = inputEventMouseMotion.Relative;
            modifierMask = inputEventMouseMotion.GetModifiersMask();
            onMouseMotion?.Invoke();
        }
        else if (input_type == typeof(InputEventMouseButton))
        {
            inputType = InputType.MouseButton;
            inputEventMouseButton = (InputEventMouseButton)input;
            mouseButton = inputEventMouseButton.ButtonIndex;
            isPressed = input.IsPressed();
            modifierMask = inputEventMouseButton.GetModifiersMask();
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
                    LeftJoystickVector.X = joyAxisValue;
                    inputType = InputType.LeftJoytickMotion;
                    break;
                case JoyAxis.LeftY:
                    LeftJoystickVector.Y = joyAxisValue;
                    inputType = InputType.LeftJoytickMotion;
                    break;
                case JoyAxis.RightX:
                    RightJoystick.X = joyAxisValue;
                    inputType = InputType.RightJoytickMotion;
                    break;
                case JoyAxis.RightY:
                    RightJoystick.Y = joyAxisValue;
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
        else if (input_type == typeof(InputEvent))
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

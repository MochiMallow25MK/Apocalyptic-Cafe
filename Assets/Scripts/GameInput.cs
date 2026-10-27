using UnityEngine;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

// One place for all keyboard input. Works with BOTH the old Input Manager
// and the new Input System package, so you never have to change settings.
public static class GameInput
{
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
    public static Vector2 Move
    {
        get
        {
            Keyboard k = Keyboard.current;
            if (k == null) return Vector2.zero;
            float x = ((k.dKey.isPressed || k.rightArrowKey.isPressed) ? 1f : 0f) - ((k.aKey.isPressed || k.leftArrowKey.isPressed) ? 1f : 0f);
            float y = ((k.wKey.isPressed || k.upArrowKey.isPressed) ? 1f : 0f) - ((k.sKey.isPressed || k.downArrowKey.isPressed) ? 1f : 0f);
            return new Vector2(x, y);
        }
    }
    public static bool InteractDown { get { return Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame; } }
    public static bool FireHeld { get { return Keyboard.current != null && Keyboard.current.spaceKey.isPressed; } }
    public static bool RestartDown { get { return Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame; } }
#else
    public static Vector2 Move { get { return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); } }
    public static bool InteractDown { get { return Input.GetKeyDown(KeyCode.E); } }
    public static bool FireHeld { get { return Input.GetKey(KeyCode.Space); } }
    public static bool RestartDown { get { return Input.GetKeyDown(KeyCode.R); } }
#endif
}

using UnityEngine;
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
using UnityEngine.InputSystem;
#endif

public static class Keys
{
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
    static Keyboard k { get { return Keyboard.current; } }
    public static Vector2 Move { get { return k == null ? Vector2.zero : new Vector2((k.dKey.isPressed ? 1 : 0) - (k.aKey.isPressed ? 1 : 0), (k.wKey.isPressed ? 1 : 0) - (k.sKey.isPressed ? 1 : 0)); } }
    public static bool Interact { get { return k != null && k.eKey.wasPressedThisFrame; } }
    public static bool Fire { get { return k != null && k.spaceKey.isPressed; } }
    public static bool Restart { get { return k != null && k.rKey.wasPressedThisFrame; } }
#else
    public static Vector2 Move { get { return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")); } }
    public static bool Interact { get { return Input.GetKeyDown(KeyCode.E); } }
    public static bool Fire { get { return Input.GetKey(KeyCode.Space); } }
    public static bool Restart { get { return Input.GetKeyDown(KeyCode.R); } }
#endif
}

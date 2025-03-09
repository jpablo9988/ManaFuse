using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputMapExtension
{
    public static void ActivateMapControls(this InputActionMap inputMap, bool activate)
    {
        if (activate)
        {
            inputMap.Enable();
        }
        else
        {
            inputMap.Disable();
        }
    }
}
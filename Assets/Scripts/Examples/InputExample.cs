using System;
using Unity.VisualScripting;
using UnityEngine;

public class InputExample : MonoBehaviour
{
    InputManager inputManager;
    void Awake()
    {
        inputManager = GameContext.Instance.InputManager;
    }

    // Update is called once per frame
    void OnEnable()
    {
        InputManager.OnMoveInteracted += PrintVector;
    }

    void OnDisable()
    {
        InputManager.OnMoveInteracted -= PrintVector;
    }
    void Update()
    {
        if (inputManager.WasSprintPressedThisFrame)
        {
            Debug.Log("Pressed Sprint");
        }
        if (inputManager.Sprint.WasReleasedThisFrame())
        {
            Debug.Log("Released Sprint");
        }
    }
    private void PrintVector(Vector2 vectorToPrint)
    {
        Debug.Log(vectorToPrint);
    }
}

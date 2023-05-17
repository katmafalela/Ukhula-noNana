using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private PlayerInput playerInput;

    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    private void awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPressAction = playerInput.actions["TouchPress"];
        touchPositionAction = playerInput.actions["TouchPosition"];
    }

    private void onEnable()
    {
        touchPressAction.performed += TouchPressed;
    }

    private void onDisable()
    {
        touchPressAction.performed -= TouchPressed;
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
       Vector3 position = Camera.main.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
        position.z = player.transform.position.z;
        player.transform.position = position;
    }


}

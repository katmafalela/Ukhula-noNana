using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TouchScript : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private PlayerInput playerInput;
    private InputAction touchPosition;
    private InputAction touchPress;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        touchPosition = playerInput.actions["TouchPosition"];
        touchPress = playerInput.actions["TouchPress"];
    }

    private void OnEnable()
    {
        touchPress.performed += TouchPressed;
    }

    private void OnDisable()
    {
        touchPress.performed -= TouchPressed;
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        Vector3 position = Camera.main.ScreenToWorldPoint(touchPosition.ReadValue<Vector2>());
        position.z = player.transform.position.z;
        player.transform.position = position;
    }


}

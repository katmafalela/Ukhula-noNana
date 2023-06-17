using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchDetection : MonoBehaviour
{
    [SerializeField]
    private float CameraSpeed = 4f;
    /*
    private TouchControls inputActions;
    private InputManager inputManager;

    private Coroutine zoomCoroutine;
    private Transform cameraTransform;

    private void Awake()
    {
        //inputActions = new TouchControls(); // Create a new instance of InputActions
        inputManager = GetComponent<InputManager>();

        cameraTransform = Camera.main.transform; // Get the main camera transform
    }
    
    private void OnEnable()
    {
        inputActions.Enable(); // Enable the input actions
    }

    private void OnDisable()
    {
        inputActions.Disable(); // Disable the input actions
    }

    private void Start()
    {
        // Subscribe to the touch events for zooming
        inputActions.Touch.SecondaryTouchContact.started += _ => ZoomStart();
        inputActions.Touch.SecondaryTouchContact.canceled += _ => ZoomEnd();
    }

    private void ZoomStart()
    {
        zoomCoroutine = StartCoroutine(DetectZooming()); // Start the zoom detection coroutine
    }

    private void ZoomEnd()
    {
        StopCoroutine(zoomCoroutine); // Stop the zoom detection coroutine
    }

    IEnumerator DetectZooming()
    {
        float prevDistance = Vector2.Distance(inputActions.Touch.PrimaryFingerPosition.ReadValue<Vector2>(),
            inputActions.Touch.SecondaryFingerPosition.ReadValue<Vector2>());
        float currentDistance = 0f;

        while (true)
        {
            // Calculate the distance between the primary and secondary finger positions
            currentDistance = Vector2.Distance(inputActions.Touch.PrimaryFingerPosition.ReadValue<Vector2>(),
                inputActions.Touch.SecondaryFingerPosition.ReadValue<Vector2>());

            // Zoom in
            if (currentDistance < prevDistance)
            {
                Vector3 targetPosition = cameraTransform.position;
                targetPosition.z += 1;
                cameraTransform.position = Vector3.Slerp(cameraTransform.position, targetPosition, Time.deltaTime * CameraSpeed);
            }
            // Zoom out
            else if (currentDistance > prevDistance)
            {
                Vector3 targetPosition = cameraTransform.position;
                targetPosition.z -= 1;
                cameraTransform.position = Vector3.Slerp(cameraTransform.position, targetPosition, Time.deltaTime * CameraSpeed);
            }

            // Update the previous distance for the next iteration
            prevDistance = currentDistance;
            yield return null;
        }
    }
    */
}

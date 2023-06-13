using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomDetection : MonoBehaviour
{
    [SerializeField]
    private float CameraSpeed = 4f;

    private TouchControls controls;
    private Coroutine zoomCoroutine;
    private Transform cameraTransform;

    private void Awake()
    {
        controls = new TouchControls(); // Create a new instance of InputActions
        cameraTransform = Camera.main.transform; // Get the main camera transform
    }

    private void OnEnable()
    {
        controls.Enable(); // Enable the input actions
    }

    private void OnDisable()
    {
        controls.Disable(); // Disable the input actions
    }

    private void Start()
    {
        // Subscribe to the touch events for zooming
        controls.Touch.SecondaryTouchContact.started += _ => ZoomStart();
        controls.Touch.SecondaryTouchContact.canceled += _ => ZoomEnd();
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
        float prevDistance = Vector2.Distance(controls.Touch.PrimaryFingerPosition.ReadValue<Vector2>(),
            controls.Touch.SecondaryFingerPosition.ReadValue<Vector2>());
        float currentDistance = 0f;

        while (true)
        {
            // Calculate the distance between the primary and secondary finger positions
            currentDistance = Vector2.Distance(controls.Touch.PrimaryFingerPosition.ReadValue<Vector2>(),
                controls.Touch.SecondaryFingerPosition.ReadValue<Vector2>());

            // Zoom in
            if (currentDistance < prevDistance)
            {
                Vector3 targetPosition = cameraTransform.position;
                targetPosition.z += 1;
                cameraTransform.position = Vector3.Slerp(cameraTransform.position, targetPosition,
                    Time.deltaTime * CameraSpeed);
            }
            // Zoom out
            else if (currentDistance > prevDistance)
            {
                Vector3 targetPosition = cameraTransform.position;
                targetPosition.z -= 1;
                cameraTransform.position = Vector3.Slerp(cameraTransform.position, targetPosition,
                    Time.deltaTime * CameraSpeed);
            }

            // Update the previous distance for the next iteration
            prevDistance = currentDistance;
            yield return null;
        }
    }
}

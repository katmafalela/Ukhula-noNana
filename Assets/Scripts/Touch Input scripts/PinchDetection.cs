using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PinchDetection : MonoBehaviour
{
    [SerializeField]
    private float CameraSpeed = 4f;

    private InputActions controls;
    private Coroutine ZoomCoroutine;
    private Transform cameraTransform;

    private void Awake()
    {
        controls = new InputActions(); // Create a new instance of InputActions
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
        controls.Touch.SecondaryScreenContact.started += _ => ZoomStart();
        controls.Touch.SecondaryScreenContact.started += _ => ZoomEnd();
    }

    private void ZoomStart()
    {
        ZoomCoroutine = StartCoroutine(ZoomDetection()); // Start the zoom detection coroutine
    }

    private void ZoomEnd()
    {
        StopCoroutine(ZoomCoroutine); // Stop the zoom detection coroutine
    }

    IEnumerator ZoomDetection()
    {
        float prevDistance = 0f;
        float currentDistance = 0f;

        while (true)
        {
            // Calculate the distance between the primary and secondary finger positions
            currentDistance = Vector2.Distance(controls.Touch.PrimaryFingerPosition.ReadValue<Vector2>(),
                controls.Touch.SecondaryFingerTouch.ReadValue<Vector2>());

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

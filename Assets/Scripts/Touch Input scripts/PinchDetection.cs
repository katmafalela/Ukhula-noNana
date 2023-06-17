using System.Collections;
using UnityEngine;

public class PinchDetection : MonoBehaviour
{
    private InputManager inputManager;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
    }

    /*private void OnEnable()
    {
        inputManager.OnStartTouch += ZoomStart; //Subscribing (+=) to inputManager's OnStartTouch event to make touch & "pinch's" time & pos relative each other
        inputManager.OnEndTouch += ZoomEnd; //Subscribing (+=) to inputManager's OnEndTouch event to make touch & swipe's time & last pos before finger lift relative to each other
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= ZoomStart; //unsubscribing (-=) from inputManager's OnStartSecondaryTouch event  
        inputManager.OnEndTouch -= ZoomEnd; //unsubscribing (-=) from inputManager's OnEndSecondaryTouch event
    }

    private void ZoomStart()
    {
        StartCoroutine(ZoomDetection());
    }

    private void ZoomEnd()
    {
        StopCoroutine(ZoomDetection());
    }

    IEnumerator ZoomDetection()
    {
        float previousDistance = 0f;
        float currentDistance = 0f;

        while (true) //while secondaryTouchContact
        {
            currentDistance = Vector2.Distance(primaryTouchPosition.ReadValue<Vector2>(), secondaryTouchPosition.ReadValue<Vector2>());

            //"pinch" out
            if (currentDistance > previousDistance)
            {
                float targetSize = mainCamera.orthographicSize - 1f; //decrease to zoom in
                float minTargetSize = Mathf.Clamp(targetSize, 0.01f, mainCamera.orthographicSize); //small value instead of 0 to prevent screen position from being out of view frustrum
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, minTargetSize, Time.deltaTime * zoomSpeed);

            }

            //pinch in
            else if (currentDistance < previousDistance)
            {
                float targetSize = mainCamera.orthographicSize + 1f; //increase to zoom out
                float maxTargetSize = Mathf.Clamp(targetSize, mainCamera.orthographicSize, 30f);
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, maxTargetSize, Time.deltaTime * zoomSpeed);
            }

            previousDistance = currentDistance; // Updating the previous distance for the next loop

            yield return null; //waiting till next frame to continue executing loop
        }

    }
    */
}

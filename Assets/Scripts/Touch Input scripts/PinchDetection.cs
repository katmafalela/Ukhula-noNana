using System.Collections;
using UnityEngine;

public class PinchDetection : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 10f;
    private InputManager inputManager;
    private Camera mainCamera;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        mainCamera = Camera.main;
    }

    public void ZoomStart()
    {
        StartCoroutine(ZoomDetection());
    }

    public void ZoomEnd()
    {
        StopCoroutine(ZoomDetection());
    }

    IEnumerator ZoomDetection()
    {
        float previousDistance = 0f;
        float currentDistance = 0f;

        while (true) //while secondaryTouchContact
        {
            currentDistance = Vector2.Distance(inputManager.PrimaryTouchPosition(), inputManager.SecondaryTouchPosition());

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
}

using System.Collections;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 10f;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void ZoomIn()
    {
        float targetSize = mainCamera.orthographicSize - 1f; //decrease to zoom in
        float minTargetSize = Mathf.Clamp(targetSize, 0.01f, mainCamera.orthographicSize); //small value instead of 0 to prevent screen position from being out of view frustrum
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, minTargetSize, Time.deltaTime * zoomSpeed);
    }

    public void ZoomOut()
    {
        float targetSize = mainCamera.orthographicSize + 1f; //increase to zoom out
        float maxTargetSize = Mathf.Clamp(targetSize, mainCamera.orthographicSize, 30f);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, maxTargetSize, Time.deltaTime * zoomSpeed);
    }
}

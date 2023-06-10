using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrackeysTouchPositionOG : MonoBehaviour
{
    [SerializeField] private GameObject circle; //for debugging

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)//if screen touched
        {
            Touch touch = Input.GetTouch(0); //touch index
            Vector3 touchPosition = Camera.main.ScreenToViewportPoint(touch.position);
            touchPosition.z = Camera.main.nearClipPlane; //making z coordinate relative to nearest point that camera can see stuff (beyond this position)
            circle.transform.position = touchPosition;
            Debug.Log(touchPosition);
        }
    }
}

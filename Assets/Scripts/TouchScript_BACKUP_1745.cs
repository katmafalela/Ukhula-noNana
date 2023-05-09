using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.InputSystem;
=======
//using UnityEngine.InputSystem;
>>>>>>> a81b77686d85a4d9d85dc943a11758ad7415da08
using UnityEngine.UIElements;

public class TouchScript : MonoBehaviour
{
<<<<<<< HEAD
    [SerializeField]
    private GameObject player;

    private PlayerInput playerInput;
    private InputAction touchPosition;
    private InputAction touchPress;
    private void Awake()
=======
    /*[SerializeField]
    private GameObject player;
    private PlayerInput playerInput;
    private InputAction touchPosition;
    private InputAction touchPress;*/

    private void Update()
    {
        GetTouchInfo();

        LoopThroughTouches();

    }

    void ExecuteTrigger(string trigger)
    {

    }

    public void OnButtonDown()
    {

    }

    public void OnButtonUp()
    {
        if (Input.touchCount < 0)
        {
            //anchored pos = slot pos
            //snapping dragged object into slot
            //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }
     
    void GetTouchInfo()
    {
        //if screen touched
        if (Input.touchCount > 0)
        {
            //storing touch info
            Touch touch = Input.GetTouch(0);
            //storing touch pos
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            //preventing touch pos on z-axis from being = to camera
            touchPosition.z = 0f;
            //making game object pos relative to touch pos 
            transform.position = touchPosition;
            //Debug.Log(touchPosition);
        }
    }

    void LoopThroughTouches()
    {
        //for so long as screen is touched, add touch to array
        for (int touch = 0; touch < Input.touchCount; touch++)
        {
            //storing touch pos of every touch in array
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.touches[touch].position);
            //Debug.DrawLine(Vector3.zero, touchPosition, Color.red);
        }
    }



    /*private void Awake()
>>>>>>> a81b77686d85a4d9d85dc943a11758ad7415da08
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
<<<<<<< HEAD
    }
=======
    }*/
>>>>>>> a81b77686d85a4d9d85dc943a11758ad7415da08


}

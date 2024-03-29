using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEngine.UIElements.UxmlAttributeDescription;

public class InputManager : MonoBehaviour //Singleton<InputManager> //Making script/class a singleton (making it easier for other scripts to access it without need to reference it) 
{
    //events to get touch's change in position & time 
    #region Events  
    public delegate void OnStartTouchDelegate(Vector2 position, float time); //creating delegate event so other scripts can easily subscibe to it (Without need to refrence this script)
    public event OnStartTouchDelegate OnStartTouch; //storing delegate event as publicly accessible event
    public delegate void OnEndTouchDelegate(Vector2 position, float time);
    public event OnEndTouchDelegate OnEndTouch;
    #endregion

    private PlayerInput playerInput;
    private Zoom zoom;

    private InputAction primaryTouchContact;
    private InputAction secondaryTouchContact;
    private InputAction primaryTouchPosition;
    private InputAction secondaryTouchPosition;

    [SerializeField] private float zoomSpeed = 10f;
    private Camera mainCamera;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        zoom = GetComponent<Zoom>();

        primaryTouchContact = playerInput.actions["PrimaryTouchContact"];
        primaryTouchPosition = playerInput.actions["PrimaryTouchPosition"];
        secondaryTouchContact = playerInput.actions["SecondaryTouchContact"];
        secondaryTouchPosition = playerInput.actions["SecondaryTouchPosition"];

        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        primaryTouchContact.started += context => StartPrimaryTouch(context); //Subscribing (+=) to OnStartTouch event (through StartPrimaryTouch) when start touching screen to get event info (context) 
        primaryTouchContact.canceled += context => EndPrimaryTouch(context); //Subscribing (+=) to OnEndTouch event (through EndPrimaryTouch) when stop touching screen to get event info (context) 

        secondaryTouchContact.started += _ => PinchStart(); //Subscribing (+=) to event but ignoring parameter passed in (_) (coz just wanna know if event started or not)
        secondaryTouchContact.canceled += _ => PinchEnd();
    }

    private void PinchEnd()
    {
        throw new NotImplementedException();
    }

    private void PinchStart()
    {
        throw new NotImplementedException();
    }

    /*private void Update() //instead of events
    {
        if (primaryContactAction.WasPerformedThisFrame())
        {
            Vector2 touchPosition = primaryTouchPositionAction.ReadValue<Vector2>();
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
            worldPosition.z = circle.transform.position.z;
            circle.transform.position = worldPosition;
            //Debug.Log(worldPosition);
            Debug.Log(touchPosition);
        }
    }*/

    private void StartPrimaryTouch(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null) //checking if event has been subscribed to
        {
            //getting touch position & time during event
            OnStartTouch(WorldPrimaryTouchPosition(), (float)context.time);
        }
    }

    private void EndPrimaryTouch(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null) //if event has been subscribed to
        {
            //getting touch position & time during event
            OnEndTouch(WorldPrimaryTouchPosition(), (float)context.time);
        }
    }

    public Vector2 WorldPrimaryTouchPosition()
    {
        Vector2 touchPosition = primaryTouchPosition.ReadValue<Vector2>();
        Vector3 screenTouchPosition = new Vector3(touchPosition.x, touchPosition.y, mainCamera.nearClipPlane); //making z coordinate relative to nearest point that camera can see stuff (beyond this position)
        Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(screenTouchPosition);
        return new Vector2(worldTouchPosition.x, worldTouchPosition.y);
    }
}
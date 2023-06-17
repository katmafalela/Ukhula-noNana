
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

    [SerializeField] private GameObject circle; //for debugging

    private PlayerInput playerInput;

    private InputAction primaryTouchContact;
    private InputAction secondaryTouchContact;
    private InputAction primaryTouchPosition;
    private InputAction secondaryTouchPosition;

    [SerializeField] private float zoomSpeed = 10f;
    private Coroutine zoomCoroutine; //do this in traditional way
    private Camera mainCamera;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        primaryTouchContact = playerInput.actions["PrimaryTouchContact"];
        primaryTouchPosition = playerInput.actions["PrimaryTouchPosition"];
        secondaryTouchContact = playerInput.actions["SecondaryTouchContact"];
        secondaryTouchPosition = playerInput.actions["SecondaryTouchPosition"];

        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        //method without player input component
        primaryTouchContact.started += context => StartPrimaryTouch(context); //Subscribing (+=) to OnStartTouch event (through StartPrimaryTouch) when start touching screen to get event info (context) 
        primaryTouchContact.canceled += context => EndPrimaryTouch(context); //Subscribing (+=) to OnEndTouch event (through EndPrimaryTouch) when stop touching screen to get event info (context) 

        secondaryTouchContact.started += _ => ZoomStart(); //Subscribing (+=) to event but ignoring parameter passed in (_) (coz just wanna know if event started or not)
        //inputManager.OnStartSecondaryTouch += ZoomStart(); //Subscribing (+=) to inputManager's OnStartTouch event (unless another event) when start touching screen, but ignoring parameter passed in (_) (coz just cheking for touch)
        secondaryTouchContact.canceled += _ => ZoomEnd();
        //inputManager.OnEndSecondaryTouch += SwipeEnd; //Subscribing (+=) to inputManager's OnEndTouch event to make touch & swipe's time & last pos before finger lift relative to each other
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
            OnStartTouch(PrimaryTouchPosition(), (float)context.time);
        }
    }

    private void EndPrimaryTouch(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null) //if event has been subscribed to
        {
            //getting touch position & time during event
            OnEndTouch(PrimaryTouchPosition(), (float)context.time);
        }
    }

    public Vector2 PrimaryTouchPosition() //World coordinates (min = -1f; max = 1f)
    {
        Vector2 touchPosition = primaryTouchPosition.ReadValue<Vector2>();
        Vector3 screenTouchPosition = new Vector3(touchPosition.x, touchPosition.y, mainCamera.nearClipPlane); //making z coordinate relative to nearest point that camera can see stuff (beyond this position)
        Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(screenTouchPosition);
        return new Vector2(worldTouchPosition.x, worldTouchPosition.y);
    }

    private void ZoomStart()
    {
        zoomCoroutine = StartCoroutine(ZoomDetection()); // Start the zoom detection coroutine
    }

    private void ZoomEnd()
    {
        StopCoroutine(zoomCoroutine); // Stop the zoom detection coroutine
    }

    IEnumerator ZoomDetection()
    {


        float previousDistance = 0f;//Vector2.Distance(primaryTouchPosition.ReadValue<Vector2>(), secondaryTouchPosition.ReadValue<Vector2>());
        float currentDistance = 0f;

        while (true) //while finger down
        {
            currentDistance = Vector2.Distance(primaryTouchPosition.ReadValue<Vector2>(), secondaryTouchPosition.ReadValue<Vector2>());

            //zoom in
            if (currentDistance > previousDistance)
            {
                //camera.orthographicSize--;
                float targetSize = mainCamera.orthographicSize - 1f; 
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
                //print("zoomIn");
            }

            //zoom out
            else if (currentDistance < previousDistance)
            {
                //camera.orthographicSize++;
                float targetSize = mainCamera.orthographicSize + 1f;
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
                //print("zoomOut");
            }

            previousDistance = currentDistance;// Updating the previous distance for the next loop

            yield return null; //waiting till next frame to continue executing loop
        }

    }

}

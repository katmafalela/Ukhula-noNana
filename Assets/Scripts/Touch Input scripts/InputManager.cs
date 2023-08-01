using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
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

    private DragNDrop dragNdrop;
    private Vector2 startPosition;  // The starting position of the swipe
    private float startTime;  // The time when the swipe started
    private Vector2 endPosition;  // The ending position of the swipe
    private float endTime;  // The time when the swipe ended
    private GameObject touchedObject;

    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minDistance = 0.2f;  // The minimum distance required for a swipe to be detected
    [SerializeField] private float maxTime = 1f;  // The maximum time allowed for a swipe to be detected
    [SerializeField, Range(0f, 1f)] private float swipeDirectionSimilarityPercentage = 0.9f;  // The required similarity percentage for a swipe to be considered in a specific direction

    [SerializeField] private GameObject swipeTrail;  // The game object representing the trail of the swipe
    [SerializeField] private GameObject levelManager;

    private Camera mainCamera;


    private void Awake()
    {
        Debug.Log("Awake called");
        playerInput = GetComponent<PlayerInput>();
        zoom = GetComponent<Zoom>();
        dragNdrop = levelManager.GetComponent<DragNDrop>();


        primaryTouchContact = playerInput.actions["PrimaryTouchContact"];
        primaryTouchPosition = playerInput.actions["PrimaryTouchPosition"];
        secondaryTouchContact = playerInput.actions["SecondaryTouchContact"];
        secondaryTouchPosition = playerInput.actions["SecondaryTouchPosition"];



        mainCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start called");
        primaryTouchContact.started += context => StartTouch(context); //Subscribing (+=) to OnStartTouch event (through StartPrimaryTouch) when start touching screen to get event info (context) 
        primaryTouchContact.canceled += context => EndTouch(context); //Subscribing (+=) to OnEndTouch event (through EndPrimaryTouch) when stop touching screen to get event info (context) 

        secondaryTouchContact.started += _ => PinchStart(); //Subscribing (+=) to event but ignoring parameter passed in (_) (coz just wanna know if event started or not)
        secondaryTouchContact.canceled += _ => PinchEnd();
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

    private void StartTouch(InputAction.CallbackContext context)
    {
        Debug.Log("StartTouch called");
        startPosition = WorldPrimaryTouchPosition();
        startTime = (float)context.time;

        swipeTrail.SetActive(true);

        Collider2D[] draggeble = Physics2D.OverlapPointAll(startPosition);
        if (OnStartTouch != null) //checking if event has been subscribed to
        {
            //getting touch position & time during event
            OnStartTouch(WorldPrimaryTouchPosition(), (float)context.time);
            foreach(Collider2D collider in  draggeble)
            {
                if(collider.CompareTag("Dragable"))
                {
                    touchedObject = collider.gameObject;
                    break;
                }
            }

            
        }
        StartCoroutine(DetectTouch());
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        Debug.Log("EndTouch called");
        endPosition = WorldPrimaryTouchPosition();
        endTime = (float)context.time;

        swipeTrail.SetActive(false);
        DetectSwipe();
        StopCoroutine(DetectTouch());

        touchedObject = null;
    }

    private void DetectSwipe()
    {
        Debug.Log("DetectSwipe called");
        // Check if the swipe distance is greater than the minimum distance and the swipe time is within the maximum time limit
        if (Vector3.Distance(startPosition, endPosition) >= minDistance && (endTime - startTime) <= maxTime)
        {
            Vector3 swipeDirection = endPosition - startPosition;  // Calculate the direction vector of the swipe
            Vector2 swipeDirection2D = new Vector2(swipeDirection.x, swipeDirection.y).normalized;  // Normalize the swipe direction vector

            StandardizeSwipeDirection(swipeDirection2D);  // Standardize the swipe direction to up/down/left/right
        }
    }

    private void StandardizeSwipeDirection(Vector2 swipeDirection2D)
    {
        Debug.Log("StandardizeSwipeDirection called");
        // Compare the similarity of the swipe direction with up/down/left/right using dot product
        if (Vector2.Dot(Vector2.up, swipeDirection2D) > swipeDirectionSimilarityPercentage)
        {
            print("Swipe Up -> do something");
        }
        else if (Vector2.Dot(Vector2.down, swipeDirection2D) > swipeDirectionSimilarityPercentage)
        {
            print("Swipe Down -> do something");
        }
        else if (Vector2.Dot(Vector2.left, swipeDirection2D) > swipeDirectionSimilarityPercentage)
        {
            print("Swipe Left -> do something");
        }
        else if (Vector2.Dot(Vector2.right, swipeDirection2D) > swipeDirectionSimilarityPercentage)
        {
            print("Swipe Right -> do something");
        }
    }

    public Vector2 WorldPrimaryTouchPosition()
    {
        Debug.Log("ScreenToWorld called");
        Vector2 touchPosition = primaryTouchPosition.ReadValue<Vector2>();
        Vector3 screenTouchPosition = new Vector3(touchPosition.x, touchPosition.y, mainCamera.nearClipPlane); //making z coordinate relative to nearest point that camera can see stuff (beyond this position)
        Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(screenTouchPosition);
        return new Vector2(worldTouchPosition.x, worldTouchPosition.y);
    }

        /*private void StartSecondaryTouch(InputAction.CallbackContext context)
        {

            if (OnStartTouch != null) //checking if event has been subscribed to
            {
                //getting touch position & time during event
                OnStartTouch(PrimaryTouchPosition(), (float)context.time);
            }
        }*/

    private void PinchStart()
    {
        Debug.Log("PinchStart called");
        StartCoroutine(DetectTouch()); 
    }

    private void PinchEnd()
    {
        Debug.Log("PinchEnd called");
        StopCoroutine(DetectTouch());
    }

    IEnumerator DetectTouch()
    {
        Debug.Log("DetectTouch called");
        float pinchDistance = 0f;
        float previousPinchDistance = 0f; 

        while (true) //while secondaryTouchContact
        {
            pinchDistance = Vector2.Distance(primaryTouchPosition.ReadValue<Vector2>(), secondaryTouchPosition.ReadValue<Vector2>());

            Vector2 touchPosition = WorldPrimaryTouchPosition();
            swipeTrail.transform.position = touchPosition;

            if(touchedObject!=null)
            {
                Debug.Log("Check if the DragNDrop works");
                touchedObject.transform.position =touchPosition;
                dragNdrop.DropOntoSlot();
            }
             
            //"pinch" out
           if (pinchDistance > previousPinchDistance)
            {
                //zoom.ZoomIn(); 

            }

            //pinch in
            else if (pinchDistance < previousPinchDistance)
            {
                //zoom.ZoomOut();
            }

            previousPinchDistance = pinchDistance; // Updating the previous distance for the next loop

            yield return null; //waiting till next frame to continue executing loop
        }

    }
    
}

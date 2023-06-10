
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEngine.UIElements.UxmlAttributeDescription;

//[DefaultExecutionOrder(-1)] //ensuring this script runs before any other script

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

    //private InputActions inputActions; //method without player input component
    private PlayerInput playerInput;

    private InputAction primaryContactAction;
    private InputAction primaryTouchPositionAction;


    private void Awake()
    {
        //inputActions = new InputActions(); //method without player input component
        playerInput = GetComponent<PlayerInput>();

        primaryContactAction = playerInput.actions["PrimaryContactAction"];
        primaryTouchPositionAction = playerInput.actions["PrimaryTouchPosition"];

    }

   private void OnEnable()
    {
        //inputActions.Enable();//method without player input component

        //primaryContactAction.performed += StartPrimaryTouch2; //Subscribing (+=) to OnStartTouch event (through StartPrimaryTouch) when start touching screen to get event info (context) (listening for functions that can subsribe to event)
    }

    private void OnDisable()
    {
        //inputActions.Disable();//method without player input component

        //primaryContactAction.performed -= StartPrimaryTouch2; //unsubscribing (-=) from OnEndTouch event (through EndPrimaryTouch) when stop touching screen to get event info (context) (stop listening for functions that can subsribe to event)
    }

        // Start is called before the first frame update
    void Start()
    {
        //method without player input component
        primaryContactAction.started += context => StartPrimaryTouch(context); //Subscribing (+=) to OnStartTouch event (through StartPrimaryTouch) when start touching screen to get event info (context) 
        primaryContactAction.canceled += context => EndPrimaryTouch(context); //Subscribing (+=) to OnEndTouch event (through EndPrimaryTouch) when stop touching screen to get event info (context) 
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

    /*private void StartPrimaryTouch2(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = primaryTouchPositionAction.ReadValue<Vector2>();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        worldPosition.z = circle.transform.position.z;
        circle.transform.position = worldPosition;
        Debug.Log(worldPosition);
    }*/

     private void StartPrimaryTouch(InputAction.CallbackContext context)
    {

        if (OnStartTouch != null) //checking if event has been subscribed to
        {
            //getting touch position & time during event
            OnStartTouch(PrimaryTouchPosition(), (float)context.time);
  
            //Debug.Log(PrimaryTouchPosition());
            circle.transform.position = PrimaryTouchPosition();

            //converting touch position (action type's value) from screen to world coordinates; using Utilities class' static Vector3 (ScreenToWorldPosition)
            //OnStartTouch(Utilities.ScreenToWorldPosition(mainCamera, playerControls.TouchMap.PrimaryTouchPosition.ReadValue<Vector2>()), (float) context.startTime);
        }
    }

    private void EndPrimaryTouch(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null) //if event has been subscribed to
        {
            //getting touch position & time during event
            OnEndTouch(PrimaryTouchPosition(), (float)context.time);

            //converting touch position (action type's value) from screen to world coordinates; using Utilities class' static Vector3 (ScreenToWorldPosition) //try do this manually coz don't understant utilities yet
            // OnEndTouch(Utilities.ScreenToWorldPosition(mainCamera, playerControls.TouchMap.PrimaryTouchPosition.ReadValue<Vector2>()), (float) context.time);
        }
    }

    //this is where the problem is (Screen to world conversion)
    public Vector2 PrimaryTouchPosition() //returns World coordinates (min = -1f; max = 1f) which limits range of swipe or tap regardless of where you tap. e.g. tap at edge of screen, doesn't return those exact coordinates
    {
        //return Utilities.ScreenToWorldPosition(mainCamera, playerControls.TouchMap.PrimaryTouchPosition.ReadValue<Vector2>());

        Vector2 touchPosition = primaryTouchPositionAction.ReadValue<Vector2>();
        Vector3 screenPosition = new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane); //making z coordinate relative to nearest point that camera can see stuff (beyond this position)
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        //Vector3 worldPosition = Camera.main.ScreenToViewportPoint(screenPosition);
        return new Vector2(worldPosition.x, worldPosition.y);
    }

}

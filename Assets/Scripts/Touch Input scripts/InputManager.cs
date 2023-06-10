
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

    private PlayerInput playerInput;

    private InputAction primaryContactAction;
    private InputAction primaryTouchPositionAction;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        primaryContactAction = playerInput.actions["PrimaryContactAction"];
        primaryTouchPositionAction = playerInput.actions["PrimaryTouchPosition"];

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
        Vector2 touchPosition = primaryTouchPositionAction.ReadValue<Vector2>();
        Vector3 screenTouchPosition = new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane); //making z coordinate relative to nearest point that camera can see stuff (beyond this position)
        Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(screenTouchPosition);
        return new Vector2(worldTouchPosition.x, worldTouchPosition.y);
    }

}

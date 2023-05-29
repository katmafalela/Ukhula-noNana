
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEngine.UIElements.UxmlAttributeDescription;

[DefaultExecutionOrder(-1)] //ensuring this script runs before any other script

public class InputManager : MonoBehaviour //Singleton<InputManager> //Making script/class a singleton (making it easier for other scripts to access it without need to reference it) 
{
    public GameObject circle;

    //events to get touch's change in position & time 
    #region Events  
    public delegate void OnStartTouchDelegate(Vector2 position, float time); //creating delegate event so other scripts can easily subscibe to it (Without need to refrence this script)
    public event OnStartTouchDelegate OnStartTouch; //storing delegate event as publicly accessible event
    public delegate void OnEndTouchDelegate(Vector2 position, float time); 
    public event OnEndTouchDelegate OnEndTouch; 
    #endregion

    private PlayerControls playerControls;
    private Camera mainCamera;

    private void Awake()
    {
        playerControls = new PlayerControls();
        mainCamera = Camera.main;
    }

   private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerControls.TouchMap.PrimaryContactAction.started += context => StartPrimaryTouch(context); //Subscribing (+=) to started event when start touching screen to get info (context) about OnStartTouch event
        playerControls.TouchMap.PrimaryContactAction.canceled += context => EndPrimaryTouch(context); //Subscribing (+=) to canceled event when stop touching screen to get info (context) about OnEndTouch event
    }

    private void Update()
    {
        circle.transform.position = PrimaryTouchPosition(); //scaling factor chat GPT
    }

    private void StartPrimaryTouch(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null) //checking if event has been subscribed to
        {
            //getting touch position & time during event
            OnStartTouch(PrimaryTouchPosition(), (float)context.time);
            //Vector3 newPosition = new Vector3(PrimaryTouchPosition().x, PrimaryTouchPosition().y, 0f); // Assuming z position of the game object remains constant
            Debug.Log(PrimaryTouchPosition());

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

    public Vector2 PrimaryTouchPosition()
    {
        //return Utilities.ScreenToWorldPosition(mainCamera, playerControls.TouchMap.PrimaryTouchPosition.ReadValue<Vector2>());

        Vector2 touchPosition = playerControls.TouchMap.PrimaryTouchPosition.ReadValue<Vector2>();
        Vector3 screenPosition = new Vector3(touchPosition.x, touchPosition.y, mainCamera.nearClipPlane); //making z coordinate relative to nearest point that camera can see geometry (beyond this position)
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        return new Vector2(worldPosition.x, worldPosition.y);

        //float scalingFactor = 4f; // Adjust this value to control the movement range
        //Vector3 scaledWorldPosition = worldPosition * scalingFactor;
        //return new Vector2(scaledWorldPosition.x, scaledWorldPosition.y);
    }

}

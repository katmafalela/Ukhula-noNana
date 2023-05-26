
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEngine.UIElements.UxmlAttributeDescription;

[DefaultExecutionOrder(-1)] //ensuring this script runs before any other script

public class InputManager : Singleton<InputManager> //Making script/class a singleton (making it easier for other scripts to access it without need to reference it) //MonoBehaviour
{
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
        playerControls.TouchMap.PrimaryContactAction.started += ctx => StartPrimaryTouch(ctx); //Subscribing (+=) to started event when start touching screen to get touch info (context) & defining OnStartTouch event
        playerControls.TouchMap.PrimaryContactAction.canceled += ctx => EndPrimaryTouch(ctx); //Subscribing (+=) to canceled event when stop touching screen to get touch info (context) & defining OnEndTouch event
    }

   

    private void StartPrimaryTouch(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null) //if event has been subscribed to
        {
            //getting touch position & time 
                //converting touch position (action type's value) from screen to world coordinates; using Utilities class' static Vector3 (ScreenToWorldPosition)
            OnStartTouch(Utilities.ScreenToWorldPosition(mainCamera, playerControls.TouchMap.PrimaryTouchPosition.ReadValue<Vector2>()), (float) context.startTime);
        }
    }

    private void EndPrimaryTouch(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null) //if event has been subscribed to
        {
            //getting touch position & time 
                //converting touch position (action type's value) from screen to world coordinates; using Utilities class' static Vector3 (ScreenToWorldPosition) //try do this manually coz don't understant utilities yet
            OnEndTouch(Utilities.ScreenToWorldPosition(mainCamera, playerControls.TouchMap.PrimaryTouchPosition.ReadValue<Vector2>()), (float) context.time);
        }
    }

    //getting touch position to update trail renderer //get this in my conventional way (ask chat GPT, lol)
    public Vector2 PrimaryTouchPosition()
    {
        return Utilities.ScreenToWorldPosition(mainCamera, playerControls.TouchMap.PrimaryTouchPosition.ReadValue<Vector2>());
    }

}

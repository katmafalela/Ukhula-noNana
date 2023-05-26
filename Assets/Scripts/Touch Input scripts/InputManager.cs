
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEngine.UIElements.UxmlAttributeDescription;

[DefaultExecutionOrder(-1)] //ensuring this script runs before any other script

public class InputManager : Singleton<InputManager> //Making script/class a singleton (making it easier for other scripts to access it without need to reference it)
{
    //events to get touch's change in position & time (see if i can reorder them for my understanding)
    #region Events  
    public delegate void StartTouch(Vector2 position, float time);//delegating OnStartTouch event to other scripts so they easily subscibe to it (Without need to refrence this script)
    public event StartTouch OnStartTouch; 
    public delegate void EndTouch(Vector2 position, float time);//delegating OnStartTouch event to other scripts so they easily subscibe to it (Without need to refrence this script)
    public event EndTouch OnEndTouch;
    #endregion
    
    private PlayerController playerControls;
    private Camera mainCamera;

    private void Awake()
    {
        playerControls = new PlayerController();
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
        //Subscribing to started & canceled events (+=) when start pressing screen to get finger touch info (context)
        playerControls.TouchMap.PrimaryContactAction.started += ctx => StartTouchPrimary(ctx);
        playerControls.TouchMap.PrimaryContactAction.canceled += ctx => EndTouchPrimary(ctx);
    }

   

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null) //if something has subscribed to event
        {
            //getting touch position & time 
            //converting touch position (action type's value) from screen to world coordinates; using Utilities class' static Vector3 (ScreenToWorldPosition)
            OnStartTouch(Utilities.ScreenToWorldPosition(mainCamera, playerControls.TouchMap.PrimaryTouchPosition.ReadValue<Vector2>()), (float) context.startTime);
        }
    }

    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null) //if something has subscribed to event
        {
            //getting touch position & time 
            //converting touch position (action type's value) from screen to world coordinates; using Utilities class' static Vector3 (ScreenToWorldPosition)
            OnEndTouch(Utilities.ScreenToWorldPosition(mainCamera, playerControls.TouchMap.PrimaryTouchPosition.ReadValue<Vector2>()), (float) context.time);
        }
    }

    //getting finger position to update trail renderer
    public Vector2 PrimaryTouchPosition()
    {
        return Utilities.ScreenToWorldPosition(mainCamera, playerControls.TouchMap.PrimaryTouchPosition.ReadValue<Vector2>());
    }

}

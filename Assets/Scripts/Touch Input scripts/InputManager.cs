
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //events to get touch's change in position & time (see if i can reorder them for my understanding)
    #region Events  
    public delegate void OnStartTouch(Vector2 position, float time);//delegating OnStartTouch event to other scripts so they easily subscibe to it (Without need to refrence this script)
    public event OnStartTouch OnStartEvent;
    public delegate void EndTouch(Vector2 position, float time);//delegating OnStartTouch event to other scripts so they easily subscibe to it (Without need to refrence this script)
    public event EndTouch OnEndTouch;
    #endregion
/*
    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
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
        //Subscibing to event (+=) when start pressing screen to get finger touch info & call function 
        playerControls.TouchMap.PrimaryContactAction.started += ctx => StartTouchPrimary(ctx);
        playerControls.TouchMap.PrimaryContactAction.canceled += ctx => EndTouchPrimary(ctx);
    }

   

    private void StartTouchPrimary(InputAction.CallbackContext.context)
    {
        //if something has subscribed to event -> send out event
        if (OnStartTouch != null) OnStartTouch;
        {

        }
    }
*/
}

using System.Collections;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private float minDistance = 0.2f;
    [SerializeField] private float maxTime = 1f;
    
    private InputManager inputManager;
    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;

    private void Awake()
    {
        //Try this instead of Singleton
        //inputManager = GetComponent<InputManager>();
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart; //Subscribing to inputManager's OnStartTouch event (+=) to get time & pos of touch
        inputManager.OnEndTouch += SwipeEnd; //Subscribing to inputManager's OnStartTouch event (+=) to get time & last pos before finger lift
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart; //unsubscribing (-=) to inputManager's OnStartTouch event  
        inputManager.OnEndTouch -= SwipeEnd; //unsubscribing (-=) to inputManager's OnStartTouch event 
    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        DetectSwipe(); //detecting swipe direction after lifted finger (coz need end position)
    }

    private void DetectSwipe() //try convert screen to world point do this manually coz don't understant utilities yet
    {
        //checking if finger has moved far enough & if touch time is short enough to qualify as swipe 
        if (Vector3.Distance(startPosition, endPosition) >= minDistance && (endTime - startTime) <= maxTime)
        {
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            //Debug.Log()
        }
    }

}

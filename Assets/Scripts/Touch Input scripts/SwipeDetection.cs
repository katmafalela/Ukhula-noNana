using System.Collections;
using System.Drawing;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private float minDistance = 0.2f;
    [SerializeField] private float maxTime = 1f;
    [SerializeField, Range(0f,1f)] private float swipeDirectionSimilarityPercentage = 0.9f;
    [SerializeField] private GameObject swipeTrail;

    private InputManager inputManager;
    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;

    //private Coroutine coroutine;

    private void Awake()
    {
        //Try this instead of Singleton
        inputManager = GetComponent<InputManager>();
        //inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart; //Subscribing to inputManager's OnStartTouch event (+=) to make touch & swipe's time & pos relative each other
        inputManager.OnEndTouch += SwipeEnd; //Subscribing to inputManager's OnStartTouch event (+=) to make touch & swipe's time & last pos before finger lift relative to each other
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

        //enabling trail
        swipeTrail.SetActive(true);
        swipeTrail.transform.position = position;
        StartCoroutine(UpdateTrailPosition());
        //coroutine = StartCoroutine(HandleSwipeTrail());
    }

    //making trail relative to touch position
    private IEnumerator UpdateTrailPosition()
    {
        while (true) 
        {
            swipeTrail.transform.position = inputManager.PrimaryTouchPosition();
            yield return null; //waiting for next frame to update trail's position
        }
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        DetectSwipe(); //detecting swipe after lifted finger (coz need end position)

        //disabling trail
        swipeTrail.SetActive(false);
        StopCoroutine(UpdateTrailPosition());
        //StopCoroutine(coroutine);
    }

    private void DetectSwipe()
    {
        //checking if finger has moved far enough & if touch time is short enough to qualify as swipe 
        if (Vector3.Distance(startPosition, endPosition) >= minDistance && (endTime - startTime) <= maxTime)
        {
            Debug.DrawLine(startPosition, endPosition, UnityEngine.Color.red, 5f); 

            Vector3 swipeDirection = endPosition - startPosition;
            Vector2 direction2D = new Vector2(swipeDirection.x, swipeDirection.y).normalized; //normalizing coz don't need length, of vector
            
            StandardizeSwipeDirection(direction2D); 
        }
    }

    //standardizing swipe direction to up/down/left/right
    private void StandardizeSwipeDirection (Vector2 swipeDirection) 
    {
        //Comparing how similar swipe direction is to up/down/left/right; using dot product (see API)
        if (Vector2.Dot(Vector2.up, swipeDirection) > swipeDirectionSimilarityPercentage) 
        {
            print("Swipe Up -> do something");
        }
        else if (Vector2.Dot(Vector2.down, swipeDirection) > swipeDirectionSimilarityPercentage)
        {
            print("Swipe Down -> do something");
        }
        else if (Vector2.Dot(Vector2.left, swipeDirection) > swipeDirectionSimilarityPercentage)
        {
            print("Swipe left -> do something");
        }
        else if (Vector2.Dot(Vector2.right, swipeDirection) > swipeDirectionSimilarityPercentage)
        {
            print("Swipe right -> do something");
        }
    }



}

using System.Collections;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private float minDistance = 0.2f;
    [SerializeField] private float maxTime = 1f;
    [SerializeField, Range(0f,1f)] private float swipeDirectionSimilarityPercentage = 0.9f;

    [SerializeField] private GameObject swipeTrail;

    private InputManager inputManager;
    private DragNDrop dropInToSlot;
    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        dropInToSlot = GetComponent<DragNDrop>();
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart; //Subscribing (+=) to inputManager's OnStartTouch event to make touch & swipe's time & pos relative each other
        inputManager.OnEndTouch += SwipeEnd; //Subscribing (+=) to inputManager's OnEndTouch event to make touch & swipe's time & last pos before finger lift relative to each other
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart; //unsubscribing (-=) from inputManager's OnStartTouch event  
        inputManager.OnEndTouch -= SwipeEnd; //unsubscribing (-=) from inputManager's OnEndTouch event
    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;

        dropInToSlot.GetSlotPositions(); // Call to initialize slot positions

        //enabling trail
        swipeTrail.SetActive(true);
        //swipeTrail.transform.position = position; //redundant?
        StartCoroutine(FollowSwipe());
    }

    //moving soemthing relative to touch position
    private IEnumerator FollowSwipe()
    {
        while (true) 
        {
            swipeTrail.transform.position = inputManager.WorldPrimaryTouchPosition(); 
            dropInToSlot.dragableObject.transform.position = inputManager.WorldPrimaryTouchPosition();

            dropInToSlot.DropIntoSlot();

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
        StopCoroutine(FollowSwipe());
    }

    private void DetectSwipe()
    {
        //checking if finger has moved far enough & if touch time is short enough to qualify as swipe 
        if (Vector3.Distance(startPosition, endPosition) >= minDistance && (endTime - startTime) <= maxTime)
        {
            //Debug.DrawLine(startPosition, endPosition, UnityEngine.Color.red, 5f); 

            Vector3 swipeDirection = endPosition - startPosition;
            Vector2 swipeDirection2D = new Vector2(swipeDirection.x, swipeDirection.y).normalized; //normalizing coz don't need length, of vector
            
            StandardizeSwipeDirection(swipeDirection2D);
        }

    }

    //standardizing swipe direction to up/down/left/right (for quick swipes)
    private void StandardizeSwipeDirection (Vector2 swipeDirection2D) 
    {
        //Comparing how similar swipe direction is to up/down/left/right; using dot product (see API)
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
            print("Swipe left -> do something");
        }
        else if (Vector2.Dot(Vector2.right, swipeDirection2D) > swipeDirectionSimilarityPercentage)
        {
            print("Swipe right -> do something");
        }
    }
}

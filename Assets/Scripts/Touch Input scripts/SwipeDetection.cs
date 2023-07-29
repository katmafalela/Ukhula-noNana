using System.Collections;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private float minDistance = 0.2f;  // The minimum distance required for a swipe to be detected
    [SerializeField] private float maxTime = 1f;  // The maximum time allowed for a swipe to be detected
    [SerializeField, Range(0f, 1f)] private float swipeDirectionSimilarityPercentage = 0.9f;  // The required similarity percentage for a swipe to be considered in a specific direction

    [SerializeField] private GameObject swipeTrail;  // The game object representing the trail of the swipe
    [SerializeField] private GameObject levelManager;

    //private TouchDraw   touchDraw;
    private InputManager inputManager;  
    private DragNDrop dragNdrop;  
    private Vector2 startPosition;  // The starting position of the swipe
    private float startTime;  // The time when the swipe started
    private Vector2 endPosition;  // The ending position of the swipe
    private float endTime;  // The time when the swipe ended
    private GameObject touchedObject;

    private void Awake()
    {
        //touchDraw    = GetComponent<TouchDraw>();
        inputManager = GetComponent<InputManager>();  // Get the InputManager component attached to the same game object
        dragNdrop = levelManager.GetComponent<DragNDrop>();  // Get the DragNDrop component attached to the same game object
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;  // Subscribe to the OnStartTouch event of the inputManager to handle the start of a touch
        inputManager.OnEndTouch += SwipeEnd;  // Subscribe to the OnEndTouch event of the inputManager to handle the end of a touch
    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;  // Unsubscribe from the OnStartTouch event of the inputManager
        inputManager.OnEndTouch -= SwipeEnd;  // Unsubscribe from the OnEndTouch event of the inputManager
    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;  
        startTime = time; 

       

        swipeTrail.SetActive(true);

        // Check if the touch position overlaps with any draggable object //put in IsTouchOverlappingDraggable bool
        Collider2D[] dragableColliders = Physics2D.OverlapPointAll(position);
        foreach (Collider2D collider in dragableColliders)
        {
            if (collider.CompareTag("Dragable"))
            {
                touchedObject = collider.gameObject;
                break;
            }
        }

        StartCoroutine(FollowSwipe()); 
    }
    
     private IEnumerator FollowSwipe()
    {
        while (true)
        {
            Vector2 touchPosition = inputManager.WorldPrimaryTouchPosition();  // Get the current touch position
            swipeTrail.transform.position = inputManager.WorldPrimaryTouchPosition();  // Update the position of the swipe trail to match the current touch position

            /*dropInToSlot.dragableObject.transform.position = inputManager.WorldPrimaryTouchPosition();  // Move the draggable object to the current touch position
            dropInToSlot.DropIntoSlot(); */

            // Check if the touch position overlaps with the draggable object
            /*if (IsTouchOverlappingDraggable(touchPosition))
            {
                dropInToSlot.dragableObject.transform.position = touchPosition;  // Move the draggable object to the current touch position
                dropInToSlot.DropIntoSlot(); 
            }*/

            if (touchedObject != null)
            {
                touchedObject.transform.position = touchPosition;  // Move the touched draggable object to the current touch position
                dragNdrop.DropOntoSlot();  // Check if the touched draggable object should be dropped into a slot
            }

            yield return null;  // Wait for the next frame to update the position of the swipe trail
        }
    }

   /* private bool IsTouchOverlappingDraggable(Vector2 touchPosition)
    {
        Collider2D draggableCollider = dropInToSlot.dragableObject.GetComponent<Collider2D>();  // Get the collider of the draggable object

        // Check if the touch position overlaps with the draggable object's collider
        return draggableCollider.OverlapPoint(touchPosition);
    }*/

    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;  // Store the ending position of the swipe
        endTime = time;  // Store the ending time of the swipe
        DetectSwipe();  // Detect the direction of the swipe based on the start and end positions

        swipeTrail.SetActive(false);  // Disable the swipe trail game object
        StopCoroutine(FollowSwipe());  // Stop the coroutine responsible for updating the position of the swipe trail

        touchedObject = null;  // Reset the touched object
    }

    private void DetectSwipe()
    {
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
}

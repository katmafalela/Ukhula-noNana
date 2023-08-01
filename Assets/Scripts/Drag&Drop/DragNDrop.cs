using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    // Public variables that can be set in the Inspector
    [SerializeField] private GameObject matchText;             // The GameObject representing the match text to be shown when the matchCounter reaches the target
    [SerializeField] private float minCircleSlotDistance = 1f; // The minimum distance from the slot position at which the draggable object can be dropped
    [SerializeField] private int matchCounter = 0;            // The current number of matches found
    [SerializeField] private int matchCountTarget = 10;       // The target number of matches to win the game
    [SerializeField] private Color OGColor;                   // The original color of the draggable object's sprite renderer
    [SerializeField] private Color matchingColor;             // The color to change the sprite renderer to when it matches the slot

    // Private variables to store references to GameObjects and positions
    private GameObject[] totalDragableObjects;                // Array to store all draggable objects in the scene
    private GameObject[] totalSlots;                          // Array to store all slot positions in the scene
    private GameObject[] wordSlotContainer;                   // Array to store all containers for word slots in the scene
    private Vector3[] totalSlotPositions;                     // Array to store the positions of all slot positions

    // Dictionaries to keep track of which draggable object is in a slot and which slot is occupied by a draggable object
    private Dictionary<GameObject, bool> isDraggableObjectInSlot = new Dictionary<GameObject, bool>();
    private Dictionary<GameObject, bool> isSlotOccupied = new Dictionary<GameObject, bool>();

    private void Awake()
    {
        // Find all draggable objects, slot positions, and word slot containers in the scene
        totalDragableObjects = GameObject.FindGameObjectsWithTag("Dragable");
        totalSlots = GameObject.FindGameObjectsWithTag("Slots");
        wordSlotContainer = GameObject.FindGameObjectsWithTag("Word Slot Container");

        // Get the positions of all slot positions
        GetSlotPositions();

        // Initialize the isDraggableObjectInSlot dictionary
        foreach (GameObject draggableObject in totalDragableObjects)
        {
            // Initially, set all draggable objects as not in a slot
            isDraggableObjectInSlot.Add(draggableObject, false);
        }

        // Initialize the isSlotOccupied dictionary
        foreach (GameObject slot in totalSlots)
        {
            // Initially, set all slots as not occupied by any draggable object
            isSlotOccupied.Add(slot, false);
        }
    }

    // Function to get the positions of all slot positions
    public void GetSlotPositions()
    {
        totalSlotPositions = new Vector3[totalSlots.Length]; // Create an array to store the positions of all slot positions

        // Loop through each slot to get its position and store it in the array
        for (int slot = 0; slot < totalSlots.Length; slot++)
        {
            totalSlotPositions[slot] = totalSlots[slot].transform.position;
        }
    }

    // Function to handle dropping a draggable object onto a slot
    public void DropOntoSlot()
    {
        // Loop through each draggable object to check for matches and dropping into slots
        foreach (GameObject draggableObject in totalDragableObjects)
        {
            CustomTags dragableCustomTags = draggableObject.GetComponent<CustomTags>(); // Get each draggable's custom tag component

            if (dragableCustomTags != null) // Check if the draggable object has a custom tag component
            {
                string letterTag = dragableCustomTags.customTagsList[0]; // Get the first custom letter tag assigned to the draggable

                // Loop through each slot position to find a match for the draggable object
                for (int slot = 0; slot < totalSlotPositions.Length; slot++)
                {
                    Vector3 slotPosition = totalSlotPositions[slot];
                    CustomTags slotCustomTags = totalSlots[slot].GetComponent<CustomTags>(); // Get each slot position's custom tag component

                    // Check if the slot position has a custom tag component and if it has a matching letter tag with the draggable object
                    if (slotCustomTags != null && slotCustomTags.HasTag(letterTag))
                    {
                        // Calculate the distance between the draggable object and the slot position
                        float objectToSlotDistance = Vector3.Distance(draggableObject.transform.position, slotPosition);

                        // Check if the draggable object is close enough to the slot to be dropped into it
                        if (objectToSlotDistance <= minCircleSlotDistance)
                        {
                            // Check if the slot is already occupied by the same draggable object
                            if (isSlotOccupied[totalSlots[slot]] && isDraggableObjectInSlot[draggableObject])
                            {
                                // If the slot is occupied by the same draggable object, do nothing and continue checking other slots
                                continue;
                            }

                            // Remove the draggable object from its previous slot if it was in a different slot
                            foreach (var entry in isSlotOccupied)
                            {
                                if (entry.Value && isDraggableObjectInSlot[draggableObject] && entry.Key != totalSlots[slot])
                                {
                                    isSlotOccupied[entry.Key] = false;
                                    break;
                                }
                            }

                            // Set the draggable object position to the slot position (lerp for smoothness)
                            draggableObject.transform.position = slotPosition;

                            // Update the dictionaries to mark the slot and draggable object as occupied
                            isSlotOccupied[totalSlots[slot]] = true;
                            isDraggableObjectInSlot[draggableObject] = true;

                            // Change the color of the draggable object's sprite renderer to the matching color
                            SpriteRenderer spriteRenderer = draggableObject.GetComponent<SpriteRenderer>();
                            if (spriteRenderer != null)
                            {
                                spriteRenderer.color = matchingColor;
                            }

                            break; // Ensure the draggable object is dropped into only one slot even if it's close to multiple slots
                        }
                        else
                        {
                            // Change the color of the draggable object's sprite renderer back to the original color
                            SpriteRenderer spriteRenderer = draggableObject.GetComponent<SpriteRenderer>();
                            if (spriteRenderer != null)
                            {
                                spriteRenderer.color = OGColor;
                            }

                            // If the draggable object was in a slot, mark it as unoccupied
                            if (isDraggableObjectInSlot[draggableObject])
                            {
                                isDraggableObjectInSlot[draggableObject] = false;
                            }
                        }
                    }
                }
            }
        }

        // Check if the matchCounter reaches the target value and set the match text active accordingly
        if (matchCounter >= matchCountTarget)
        {
            matchText.SetActive(true);
        }
        else
        {
            matchText.SetActive(false);
        }
    }
}

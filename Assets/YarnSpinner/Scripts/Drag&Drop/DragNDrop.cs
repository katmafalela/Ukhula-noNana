using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    [SerializeField] private Transform triangle;
   [SerializeField] private GameObject matchText; //put in level manager
    [SerializeField] private float minCircleSlotDistance = 1f;
    [SerializeField] private int matchCounter = 0; //put in level manager
    [SerializeField] private int matchCountTarget = 10; //put in level manager
    [SerializeField] private Color OGColor;
    [SerializeField] private Color matchingColor; // The color to change the sprite renderer to when it matches the slot

    private GameObject[] totalDragableObjects;
    private GameObject[] totalSlots;
    private GameObject[] wordSlotContainer;
    private Vector3[] totalSlotPositions;

    private Dictionary<GameObject, bool> isDraggableObjectInSlot = new Dictionary<GameObject, bool>();
    private Dictionary<GameObject, Vector3> previousMatchingSlotPosition = new Dictionary<GameObject, Vector3>();

    private void Awake()
    {
        totalDragableObjects = GameObject.FindGameObjectsWithTag("Dragable");
        totalSlots = GameObject.FindGameObjectsWithTag("Slots");
        wordSlotContainer = GameObject.FindGameObjectsWithTag("Word Slot Container");
        GetSlotPositions();

        // Initialize the draggableObjectInSlot dictionary
        foreach (GameObject draggableObject in totalDragableObjects)
        {
            isDraggableObjectInSlot.Add(draggableObject, false);
            previousMatchingSlotPosition.Add(draggableObject, draggableObject.transform.position);
        }
    }

    public void GetSlotPositions()
    {
        totalSlotPositions = new Vector3[totalSlots.Length]; //total stored positions = total game objects found

        for (int slot = 0; slot < totalSlots.Length; slot++) //for so long as there's more than 1 slot
        {
            totalSlotPositions[slot] = totalSlots[slot].transform.position; // Each stored position = position of each slot found
        }
    }

    public void DropOntoSlot()
    {
        foreach (GameObject draggableObject in totalDragableObjects)
        {
            CustomTags dragableCustomTags = draggableObject.GetComponent<CustomTags>(); //Get each draggable's custom tag component

            if (dragableCustomTags != null) //ensuring slot position has custom tag component
            {
                string letterTag = dragableCustomTags.customTagsList[0]; //Get each draggable's custom tag (Assuming only one custom letter tag assigned)
                bool dropped = false;

                // Loop through slots to find the match
                for (int slot = 0; slot < totalSlotPositions.Length; slot++)
                {
                    Vector3 slotPosition = totalSlotPositions[slot];
                    CustomTags slotCustomTags = totalSlots[slot].GetComponent<CustomTags>(); //Get each slot position's custom tag component

                    // Check if slot & draggableObject have matching letter tags & ensuring slot has custom tag component
                    if (slotCustomTags != null && slotCustomTags.HasTag(letterTag))
                    {
                        //

                        float objectToSlotDistance = Vector3.Distance(draggableObject.transform.position, slotPosition);

                        //checking if draggableObject is close enough to drop in slot 
                        if (objectToSlotDistance <= minCircleSlotDistance)
                        {
                            // Check if the draggable object is already in a slot
                            if (!isDraggableObjectInSlot[draggableObject])
                            {
                                matchCounter++; // If not, increase the matchCounter
                            }

                            // Change the color of the draggable object's sprite renderer to the matching color
                            SpriteRenderer spriteRenderer = draggableObject.GetComponent<SpriteRenderer>();
                            if (spriteRenderer != null)
                            {
                                spriteRenderer.color = matchingColor;
                            }

                            dropped = true;
                            draggableObject.transform.position = slotPosition; //lerp for smoothness
                            isDraggableObjectInSlot[draggableObject] = true; // Set the draggable object's state to "in slot"
                            previousMatchingSlotPosition[draggableObject] = slotPosition; // Update the last known slot position
                            break; //ensuring draggable dropped into only 1 slot, even if it's close enough to multiple slots.
                        }
                    }
                }

                if (!dropped)
                {
                    // Check if the draggable object was in a slot before dragging
                    if (isDraggableObjectInSlot[draggableObject])
                    {
                        // If it was, decrease the matchCounter and set its state to "not in slot"
                        matchCounter--;
                        isDraggableObjectInSlot[draggableObject] = false;
                    }

                    // Change the color of the draggable object's sprite renderer to the matching color
                    SpriteRenderer spriteRenderer = draggableObject.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = OGColor;
                    }
                }
            }
        }

        // Check if the matchCounter reaches a value of 10 and set the match text active accordingly
        if (matchCounter == matchCountTarget)
        {
            matchText.SetActive(true);
            triangle.position = new Vector3(8f, -2f, 0f); // call triangle movement function 
        }
        else
        {
            matchText.SetActive(false);
        }
    }


}


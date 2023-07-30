using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    [SerializeField] private GameObject matchText;
    [SerializeField] private float minCircleSlotDistance = 1f;
    [SerializeField] private int matchCounter = 0;
    [SerializeField] private int matchCountTarget = 10;
    [SerializeField] private Color OGColor;
    [SerializeField] private Color matchingColor; // The color to change the sprite renderer to when it matches the slot

    private GameObject[] totalDragableObjects;
    private GameObject[] totalSlots;
    //private GameObject[] wordSlotContainer;
    private Vector3[] totalSlotPositions;

    private Dictionary<GameObject, bool> isDraggableObjectInSlot = new Dictionary<GameObject, bool>();
    private Dictionary<GameObject, bool> isSlotOccupied = new Dictionary<GameObject, bool>();

    private void Awake()
    {
        totalDragableObjects = GameObject.FindGameObjectsWithTag("Dragable");
        totalSlots = GameObject.FindGameObjectsWithTag("Slots");
        //wordSlotContainer = GameObject.FindGameObjectsWithTag("Word Slot Container");
        GetSlotPositions();

        // Initialize dictionary
        foreach (GameObject draggableObject in totalDragableObjects)
        {
            isDraggableObjectInSlot.Add(draggableObject, false); 
        }

        foreach (GameObject slot in totalSlots)
        {
            isSlotOccupied.Add(slot, false);
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

                // Loop through slots to find a match
                for (int slot = 0; slot < totalSlotPositions.Length; slot++)
                {
                    Vector3 slotPosition = totalSlotPositions[slot];
                    CustomTags slotCustomTags = totalSlots[slot].GetComponent<CustomTags>(); //Get each slot position's custom tag component

                    // Check if slot & draggableObject have matching letter tags & ensuring slot has custom tag component
                    if (slotCustomTags != null && slotCustomTags.HasTag(letterTag))
                    {
                        float objectToSlotDistance = Vector3.Distance(draggableObject.transform.position, slotPosition);

                        //checking if draggableObject is close enough to drop in slot 
                        if (objectToSlotDistance <= minCircleSlotDistance)
                        {
                            // Check if the slot is already occupied by a draggable object
                            /*if (isSlotOccupied[totalSlots[slot]])
                            {
                                // If the slot is occupied by the same draggable object, do nothing
                                continue;
                            }*/

                           

                            draggableObject.transform.position = slotPosition; //lerp for smoothness

                            //Checking if not removing the draggable object from a slot
                            if (!isDraggableObjectInSlot[draggableObject])
                            {
                                matchCounter++; // If not, increase the matchCounter
                            }

                            isDraggableObjectInSlot[draggableObject] = true;
                            isSlotOccupied[totalSlots[slot]] = true;

                            //Changing colour
                            SpriteRenderer spriteRenderer = draggableObject.GetComponent<SpriteRenderer>();
                            if (spriteRenderer != null)
                            {
                                spriteRenderer.color = matchingColor;
                            }

                            break; //ensuring draggable dropped into only 1 slot, even if it's close enough to multiple slots.
                        }

                        else
                        {
                            //resetting colour
                            SpriteRenderer spriteRenderer = draggableObject.GetComponent<SpriteRenderer>();
                            if (spriteRenderer != null)
                            {
                                spriteRenderer.color = OGColor;
                            }

                            //removing draggable object from a slot
                            if (isDraggableObjectInSlot[draggableObject])
                            {
                                matchCounter--;
                                isDraggableObjectInSlot[draggableObject] = false;
                            }
                        }
                    }
                }
            }
        }

        // Check if the matchCounter reaches a value of 10 and set the match text active accordingly
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


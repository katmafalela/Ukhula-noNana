using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    private string id;

    [SerializeField] private GameObject matchText;
    [SerializeField] private float minCircleSlotDistance = 1f;
    [SerializeField] private int matchCounter = 0;
    [SerializeField] private int matchCountTarget = 10;
    [SerializeField] private Color OGColor;
    [SerializeField] private Color matchingColor;

    private GameObject[] totalDragableObjects;
    private GameObject[] totalSlots;
    private GameObject[] wordSlotContainer;
    private Vector3[] totalSlotPositions;

    private Dictionary<GameObject, bool> isDraggableObjectInSlot = new Dictionary<GameObject, bool>();
    private string draggbleItemId;

    private void Awake()
    {
        // Find all dragable objects, slots, and word slot containers
        totalDragableObjects = GameObject.FindGameObjectsWithTag("Dragable");
        totalSlots = GameObject.FindGameObjectsWithTag("Slots");
        wordSlotContainer = GameObject.FindGameObjectsWithTag("Word Slot Container");

        // Get positions of slots
        GetSlotPositions();

        // Initialize isDraggableObjectInSlot dictionary
        foreach (GameObject draggableObject in totalDragableObjects)
        {
            isDraggableObjectInSlot.Add(draggableObject, false);
        }
    }

    public void GetSlotPositions()
    {
        // Store positions of slots
        totalSlotPositions = new Vector3[totalSlots.Length];

        for (int slot = 0; slot < totalSlots.Length; slot++)
        {
            totalSlotPositions[slot] = totalSlots[slot].transform.position;
        }
    }

    public void DropOntoSlot()
    {
        foreach (GameObject draggableObject in totalDragableObjects)
        {
            CustomTags dragableCustomTags = draggableObject.GetComponent<CustomTags>();

            if (dragableCustomTags != null)
            {
                string letterTag = dragableCustomTags.customTagsList[0];

                foreach (GameObject slot in totalSlots)
                {
                    Vector3 slotPosition = slot.GetComponent<RectTransform>().anchoredPosition;
                    CustomTags slotCustomTags = slot.GetComponent<CustomTags>();

                    if (slotCustomTags != null && slotCustomTags.HasTag(letterTag))
                    {
                        float objectToSlotDistance = Vector3.Distance(draggableObject.transform.position, slotPosition);

                        // Check if the slot is already occupied
                        bool isOccupied = false;
                        foreach (GameObject otherDraggableObject in totalDragableObjects)
                        {
                            if (isDraggableObjectInSlot[otherDraggableObject] && otherDraggableObject != draggableObject)
                            {
                                Vector3 otherSlotPosition = otherDraggableObject.transform.position;
                                float distanceToOtherSlot = Vector3.Distance(otherSlotPosition, slotPosition);
                                if (distanceToOtherSlot <= minCircleSlotDistance)
                                {
                                    isOccupied = true;
                                    print("Slot occupied");
                                    slotPosition = otherSlotPosition;
                                    break;
                                }
                            }
                        }

                        if (!isOccupied && objectToSlotDistance <= minCircleSlotDistance)
                        {
                            // Move draggable object to the slot
                            draggableObject.transform.position = slotPosition;

                            if (!isDraggableObjectInSlot[draggableObject])
                            {
                                matchCounter++;
                            }

                            // Change color to matching color
                            SpriteRenderer spriteRenderer = draggableObject.GetComponent<SpriteRenderer>();
                            if (spriteRenderer != null)
                            {
                                spriteRenderer.color = matchingColor;
                            }

                            isDraggableObjectInSlot[draggableObject] = true;
                        }
                        else
                        {
                            // Change color to red for incorrect slot occupancy
                            SpriteRenderer spriteRenderer = draggableObject.GetComponent<SpriteRenderer>();
                            if (spriteRenderer != null)
                            {
                                spriteRenderer.color = Color.red;
                            }

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

        // Update matchText visibility
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

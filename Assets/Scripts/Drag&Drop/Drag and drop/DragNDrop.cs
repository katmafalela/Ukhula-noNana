using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private Dictionary<GameObject, GameObject> slotOccupiedBy = new Dictionary<GameObject, GameObject>();
    private string draggbleItemId;

    private void Awake()
    {
        // Find all dragable objects, slots, and word slot containers
        totalDragableObjects = GameObject.FindGameObjectsWithTag("Dragable");
        totalSlots = GameObject.FindGameObjectsWithTag("Slots");
        wordSlotContainer = GameObject.FindGameObjectsWithTag("Word Slot Container");

        

        // Initialize isDraggableObjectInSlot dictionary
        foreach (GameObject draggadObject in totalDragableObjects)
        {
            isDraggableObjectInSlot.Add(draggadObject, false);
        }
        
        // Get positions of slots
        GetSlotPositions();

        // Create a fake PointerEventData
        PointerEventData fakeEventData = new PointerEventData(EventSystem.current);
        fakeEventData.pointerId = -1;

        DropOntoSlot(fakeEventData);
    }

    private void Start()
    {
        OGColor = totalDragableObjects[0].GetComponent<SpriteRenderer>().color;
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

    public void DropOntoSlot(BaseEventData eventData)
    {
        PointerEventData pointerVentData = eventData as PointerEventData;
   
        if(pointerVentData==null)
        {
            Debug.Log("Pointereventdata returns null");
            return;
        }

        GameObject draggableObject = pointerVentData.pointerDrag;
        if(draggableObject==null)
        {
            Debug.Log("Draggble object retuns null");
            return;
        }

        CustomTags dragableCustomTags = draggableObject.GetComponent<CustomTags>();

       
        foreach (GameObject draggadObject in totalDragableObjects)
        {
            CustomTags dragableCustomTag = draggableObject.GetComponent<CustomTags>();

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
                            if (slotOccupiedBy.ContainsKey(slot) && slotOccupiedBy[slot] == draggableObject)
                            {
                                slotOccupiedBy.Remove(slot);
                            }
                        }
                    }
                }
            }
        }

        // Update matchText visibility
        matchText.SetActive(matchCounter >= matchCountTarget);
    }

    public void ResetColors()
    {
        foreach (GameObject draggableObject in totalDragableObjects)
        {
            SpriteRenderer spriteRenderer = draggableObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = OGColor;
            }
        }
    }
}



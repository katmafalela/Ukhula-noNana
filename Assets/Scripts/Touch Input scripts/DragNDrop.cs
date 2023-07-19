using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    [SerializeField] private GameObject matchText;
    [SerializeField] private float minCircleSlotDistance = 1f;

    //private CustomTags dragableObjectCustomTags;
    private GameObject[] totalDragableObjects;
    private GameObject[] totalSlots;
    private Vector3[] totalSlotPositions;

    private void Awake()
    {
        totalDragableObjects = GameObject.FindGameObjectsWithTag("Dragable");
        totalSlots = GameObject.FindGameObjectsWithTag("Slots");
        GetSlotPositions();
    }

    public void GetSlotPositions()
    {
        totalSlotPositions = new Vector3[totalSlots.Length]; //total stored positions = total game objects found

        for (int slot = 0; slot < totalSlots.Length; slot++) //for so long as there's more than 1 slot
        {
            CustomTags slotCustomTags = totalSlots[slot].GetComponent<CustomTags>(); //Get each slot position's custom tag component

            if (slotCustomTags != null) //ensuring slot has custom tag component
            {
                string letterTag = slotCustomTags.customTagsList[0]; //Get each slot's custom tag Assuming only one custom letter tag assigned
                totalSlotPositions[slot] = totalSlots[slot].transform.position; // Each stored position = position of each slot found
            }
        }
    }

    public void DropOntoSlot()
    {
        foreach (GameObject draggableObject in totalDragableObjects)
        {
            /*foreach (Vector3 slotPosition in totalSlotPositions)
            {
                float objectToSlotDistance = Vector3.Distance(draggableObject.transform.position, slotPosition);
                //checking if draggableObject is close enough to drop in slot 
                if (objectToSlotDistance <= minCircleSlotDistance)  
                {
                    //is it possible to give a game object more than 1 tag? otherwise make slot & object names match & check if names match (gnag if statements)
                    //if (draggableObject.name == ... && totalSlots[slot ==)
                    draggableObject.transform.position = slotPosition;
                    matchText.SetActive(true);
                    break; //ensureing circle dropped into only 1 slot, even if its colse enough to multiple slots.
                }
                else
                {
                    matchText.SetActive(false);
                }
            }*/

            CustomTags dragableCustomTags = draggableObject.GetComponent<CustomTags>(); //Get each draggable's custom tag component

            if (dragableCustomTags != null) //ensuring slot position has custom tag component
            {
                string letterTag = dragableCustomTags.customTagsList[0]; //Get each draggable's custom tag Assuming only one custom letter tag assigned
                bool dropped = false;

                // Loop through slots to find the match
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
                            draggableObject.transform.position = slotPosition; //lerp for smoothness
                            matchText.SetActive(true);
                            dropped = true;
                            break; //ensuring circle dropped into only 1 slot, even if it's close enough to multiple slots.
                        }
                    }
                }

                if (!dropped)
                {
                    matchText.SetActive(false);
                }
            }
        }
    }
}


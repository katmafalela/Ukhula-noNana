using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    [SerializeField] private GameObject matchText;
    [SerializeField] private float minCircleSlotDistance = 1f;

    private GameObject[] totalDraggableObjects;
    private GameObject[] totalSlots;
    private Vector3[] totalSlotPositions;

    private void Awake()
    {
        totalDraggableObjects = GameObject.FindGameObjectsWithTag("Dragable");
        GetSlotPositions();
    }

    public void GetSlotPositions()
    {
        totalSlots = GameObject.FindGameObjectsWithTag("Slots");
        totalSlotPositions = new Vector3[totalSlots.Length]; //total stored positions = total game objects found

        for (int slotPosition = 0; slotPosition < totalSlots.Length; slotPosition++) //for so long as there's more than 1 slot
        {
            totalSlotPositions[slotPosition] = totalSlots[slotPosition].transform.position; //each stored position = position of each slot found
        }
    }

    public void DropIntoSlot()
    {
        foreach (GameObject draggableObject in totalDraggableObjects)
        {
            foreach (Vector3 slotPosition in totalSlotPositions)
            {
                float objectToSlotDistance = Vector3.Distance(draggableObject.transform.position, slotPosition);
                //checking if circle is close enough to drop in slot 
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
            }
        }
    }
}


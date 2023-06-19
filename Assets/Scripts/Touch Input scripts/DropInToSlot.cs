using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropInToSlot : MonoBehaviour
{
    public GameObject circle; //for debugging
    [SerializeField] private float minCircleSlotDistance = 1f;

    private Vector3[] totalSlotPositions;

    public void GetSlotPositions()
    {
        GameObject[] totalSlots = GameObject.FindGameObjectsWithTag("Slot1");
        totalSlotPositions = new Vector3[totalSlots.Length]; //total stored positions = total game objects found

        for (int slotPosition = 0; slotPosition < totalSlots.Length; slotPosition++) //for so long as there's more than 1 slot
        {
            totalSlotPositions[slotPosition] = totalSlots[slotPosition].transform.position; //each stored position = position of each slot found
        }
    }

    public void Drop()
    {
        //checking if circle is close enough to drop in slot 
        foreach (Vector3 slotPosition in totalSlotPositions)
        {
            float circleSlotDistance = Vector3.Distance(circle.transform.position, slotPosition);
            if (circleSlotDistance <= minCircleSlotDistance)
            {
                //DropIntoSlot(slotPosition);
                circle.transform.position = slotPosition;
                break; //ensureing circle dropped into only 1 slot, even if its colse enough to multiple slots.
            }
        }
    }
}

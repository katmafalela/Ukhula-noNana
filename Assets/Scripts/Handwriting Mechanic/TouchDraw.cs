using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchDraw : MonoBehaviour
{
    private Coroutine drawing;
    public static List<LineRenderer> drawnLineRenderes;
    private Color lineColor = Color.white;

    void Update()
    {
        //Check if the Mouse or Screen is touched
        if(Input.GetMouseButtonDown(0))
        {
            //Check if the click is not a UI element
            if(EventSystem.current.currentSelectedGameObject == null) 
            {
                //Start drawing the line
                StartLine();
            }
        }

        //Check if the left mouse is released 
        if(Input.GetMouseButtonUp(0))
        {
            //Stop the current drawing
            FinishLine();
        }
    }

    public void FinishLine()
    {
        if(drawing != null)
        {        
            StopCoroutine(drawing);
        }
    }

     public void StartLine()
    {
        if(drawing!=null)
        {
            StopCoroutine(drawing);
        }

        Coroutine coroutine = StartCoroutine(DrawLine());
        drawing = coroutine;
    }

    IEnumerator DrawLine()
    {
        // Instatiate a new game object  with the line renderer components to reperesent a drawn line
        GameObject newGameObject = Instantiate(Resources.Load("Prefab/Line") as GameObject, new Vector3(0,0,0), Quaternion.identity);
        LineRenderer line = newGameObject.GetComponent<LineRenderer>();

        //Adding the new drawn line in the list of line renderer to keep track of it
        drawnLineRenderes.Add(line);
        //Set the initial position of the drawn line to 0
        line.positionCount = 0;

        while(true)
        {
            //Get the current position of the mouse in the game world
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;

            //Add the current mouse positon as a point to the LineRenderer
            line.positionCount++;
            line.SetPosition(line.positionCount-1, position);
            line.startColor = lineColor; 
            line.endColor = lineColor;
            yield return null;
        }
    }

    internal void SetLineColor(Color color)
    {
        lineColor = color;
    }
}

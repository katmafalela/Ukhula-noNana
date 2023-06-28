using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDraw : MonoBehaviour
{
    Coroutine drawing;

    // This method is called once per frame
    void Update()
    {
        // Check if the left mouse button is pressed down
        if (Input.GetMouseButtonDown(0))
        {
            StartLine();
        }
        // Check if the left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            FinishLine();
        }
    }

    // This method is called when the user releases the left mouse button
    private void FinishLine()
    {
        // Stop the coroutine responsible for drawing the line
        StopCoroutine(drawing);
    }

    // This method is called when the user presses the left mouse button
    private void StartLine()
    {
        // Check if there is already a drawing in progress
        if (drawing != null)
        {
            // If there is, stop the coroutine responsible for drawing the previous line
            StopCoroutine(drawing);
        }

        // Start a new coroutine to draw a line
        Coroutine coroutine = StartCoroutine(DrawLine());
        drawing = coroutine;
    }

    // Coroutine responsible for drawing a line
    IEnumerator DrawLine()
    {
        // Instantiate a new game object from a prefab called "Line"
        GameObject newGameObject = Instantiate(Resources.Load("Assets/Resources/PreFab/Line") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
        // Get the LineRenderer component attached to the new game object
        LineRenderer line = newGameObject.GetComponent<LineRenderer>();
        // Set the initial position count of the line to 0
        line.positionCount = 0;

        // Enter an infinite loop to continuously update the line's position
        while (true)
        {
            // Convert the mouse position from screen coordinates to world coordinates
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;
            // Increase the position count of the line by 1
            line.positionCount++;
            // Set the latest position of the line to the current mouse position
            line.SetPosition(line.positionCount - 1, position);

            // Pause the execution of the coroutine and return control to the Update method
            yield return null;
        }
    }
}

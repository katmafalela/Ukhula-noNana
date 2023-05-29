using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{

    //converting Screen coordinates to world coordinates so scripts can use them directly instead of converting them in scripts
        //get this in my conventional way (ask chat GPT, lol)
    public static Vector3 ScreenToWorldPosition(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane; //making z pos relative to nearest point camera can see geometry (beyond this position); coz using 2D
        return camera.ScreenToWorldPoint(position); 
    }
}

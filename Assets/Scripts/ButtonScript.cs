 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField]
        private Transform puzzleField; //To control the position of the player
    [SerializeField]
    private GameObject btn;
    public void Awake()
    {
        //To create buttons in the game
        for(int i=0; i<8; i++)
        {
            GameObject button = Instantiate(btn);//Creates a copy of the btn and assign it to the button object 
            button.name = "Button " + i; // Name the buttons
            button.transform.SetParent(puzzleField,false); //Set the puzzle field as the parent of the button object and the boolean parameter will make the object appear in the game
        }
    }
}

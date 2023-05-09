using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<Button> btns = new();//This allows us to get our buttons


    public void Start()
    {
        //Debug.Log("Hello");
        GetButton();
        AddListeners();
    }
    
    public void GetButton()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");//Uses the tag to find a game object

       //Goes through the button list and adds the button component
        for(int i=0; i<objects.Length;i++)
        {
           btns.Add(objects[i].GetComponent<Button>());
        }
    }

     public  void AddListeners()
    {
        //Add functionality to the buttons each time they are created
        
        //For each buttons pressed in the button list add a listener
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(()=> PickAPuzzle());
        }
    }
    public void PickAPuzzle()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("Pressed+ " + name);
    }
}

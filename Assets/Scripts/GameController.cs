using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<Button> btns = new();//This allows us to get our buttons
    [SerializeField]
    private Sprite bgImage;

    public Sprite[] puzzles;
    public List<Sprite> puzzles2 = new();

    private bool firstGuess;
    private bool secondGuess;

    private int firstGuessIndex;
    private int secondGuessIndex;

    private int guessCount;
    private int countCorrectGuesses;
    private int gameGuesses;

    private string firstGuessPuzzle;
    private string secondGuessPuzzle;
    public void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Sprites/EmojiOne");  
    }
    public void Start()
    {
        //Debug.Log("Hello");
        GetButton();
        AddListeners();
        addPuzzlePieces();
    }
    
    public void GetButton()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");//Uses the tag to find a game object

       //Goes through the button list and adds the button component
        for(int i=0; i<objects.Length;i++)
        {
           btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage; // Assign the bgImage to in the background of all the buttons created
        }
    }
    public void addPuzzlePieces()
    {
        int counter = btns.Count;
        int _index = 0;

        for(int i=0;i<counter;i++)
        {
            if(_index==counter/2)
            {
                _index = 0;
            }
            puzzles2.Add(puzzles[_index]);
            _index++;
        }
    }
     public  void AddListeners()
    {
        //Add functionality to the buttons each time they are created
        
        //For each button pressed in the button list add a listener
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(()=> PickAPuzzle());
        }
    }
    public void PickAPuzzle()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(name);
            firstGuessPuzzle = puzzles2[firstGuessIndex].name;
            btns[firstGuessIndex].image.sprite = puzzles2[firstGuessIndex];
        }else if(!secondGuess)
        {

            secondGuess = true;
            secondGuessIndex = int.Parse(name);
            secondGuessPuzzle = puzzles2[secondGuessIndex].name;
            btns[secondGuessIndex].image.sprite = puzzles2[secondGuessIndex];

            if(firstGuessPuzzle == secondGuessPuzzle && firstGuessIndex != secondGuessIndex)
            {
                Debug.Log("Puzzles match");
            }
            else
            {
                Debug.Log("Puzzles don't match");
            }
        }
        //Debug.Log("Pressed+ " + name);
    }
}

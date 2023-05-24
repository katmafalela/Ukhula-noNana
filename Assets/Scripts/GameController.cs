using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    private bool canFlip; // Flag to prevent flipping more than two tiles at a time
    private int firstFlippedIndex; // Index of the first flipped tile

    private int matchesFound; // Count of matched pairs
    private int totalMatches; // Total number of pairs to be matched

    public List<Button> btns = new List<Button>(); // List of buttons that represent the image tiles
    [SerializeField]
    private Sprite bgImage;

    public Sprite[] puzzles;
    public List<Sprite> puzzles2 = new List<Sprite>();
    public void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Sprites/EmojiOne");
    }

    public void Start()
    {
        canFlip = true;
        firstFlippedIndex = -1;
        matchesFound = 0;
        totalMatches = puzzles.Length / 2;

        GetButton();
        AssignImages();
        AddListeners();
    }

    public void GetButton()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

        for (int i = 0; i < objects.Length; i++)
        {
            btns.Add(objects[i].GetComponent<Button>());
            btns[i].image.sprite = bgImage;
        }
    }
    private void AssignImages()
    {
        int counter = btns.Count;
        int index = 0;
        for (int i = 0; i < counter; i++)
        {
            if (index == counter / 2)
            {
                index = 0;
            }
            puzzles2.Add(puzzles[index]);
            index++;
        }
    }

    public void AddListeners()
    {
        foreach (Button button in btns)
        {
            button.onClick.AddListener(() => StartCoroutine(FlipTile(button)));
        }
    }

    private IEnumerator FlipTile(Button button)
    {
        int buttonIndex = btns.IndexOf(button);

        if (!canFlip || buttonIndex == firstFlippedIndex)
            yield break;

        button.image.sprite = puzzles2[buttonIndex];

        if (firstFlippedIndex == -1)
        {
            firstFlippedIndex = buttonIndex;
        }
        else
        {
            canFlip = false;
            yield return new WaitForSeconds(1f);

            if (puzzles2[buttonIndex] == puzzles2[firstFlippedIndex])
            {
                // Match found
                button.interactable = false;
                btns[firstFlippedIndex].interactable = false;
                matchesFound++;

                if (matchesFound == totalMatches)
                {
                    Debug.Log("Congratulations! You've matched all pairs.");
                    // Add game completion logic here
                }
            }
            else
            {
                // No match
                button.image.sprite = bgImage;
                btns[firstFlippedIndex].image.sprite = bgImage;
            }

            firstFlippedIndex = -1;
            canFlip = true;
        }
    }
}

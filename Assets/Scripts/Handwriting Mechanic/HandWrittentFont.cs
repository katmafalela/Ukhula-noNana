using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HandWrittentFont : MonoBehaviour
{
    #region CustomClasses

    [Serializable]
    public class Line
    {
        public Vector3[] line;

        public Line(Vector3[] newLine)
        {
            line = newLine;
        }
    }

    [Serializable]
    public class LetterData
    {
        public List<Line> lines;
        
        public LetterData(List<LineRenderer> lineRenderers) 
        {
            lines = new List<Line> ();
            foreach(LineRenderer lineRenderer in lineRenderers)
            {
                Vector3[] points = new Vector3[lineRenderer.positionCount];
                lineRenderer.GetPositions(points);
                lines.Add(new Line(points));
            }
        }
    }

    [Serializable]
    public class FontData
    {
        public List<string> letterKeys;
        public List<LetterData> letterDataList;

        public FontData(Dictionary<string,LetterData> characterDictionary)
        {
            letterDataList = new List<LetterData> ();
            letterKeys = new List<string>();

            foreach(KeyValuePair<string,LetterData> entry in characterDictionary)
            {
                letterKeys.Add(entry.Key);
                letterDataList.Add(entry.Value);
            }
        }
    }
    #endregion

    #region FontDataVariables
    // Array containing all the uppercase and lowercase letters of the alphabet
   public string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
                        "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
                        "U", "V", "W", "X", "Y", "Z", "a", "b", "c", "d",
                        "e", "f", "g", "h", "i", "j", "k", "l", "m", "n",
                        "o", "p", "q", "r", "s", "t", "u", "v", "w", "x",
                        "y", "z" };
    
    //Dictionary to store the letterData objects for each letter
    Dictionary<string,LetterData> letterDictionary = new Dictionary<string,LetterData> ();

    //Serialized objects that stores the FontData for saving and loading purposes
    public FontData fontData;
    #endregion


    #region CreateFontVariables
    // Index to keep track of the current letter being created
   public int currentLetterIndex;

    // UI Text to display the current letter being worked on
    public TMPro.TMP_Text letterText;
    #endregion

    #region WriteTextVariables
    // Flag to indicate if a letter is currently being drawn for the WriteText functionality
    bool drawingLetter;
    #endregion

    #region Initialization
    void Start()
    {
        // Check if the font data is already saved. If yes, load it; otherwise, create a new font
        if (HasFont())
        {
            LoadFont();
        }
        else
        {
            CreateFont();
        }
    }

    void Update()
    {
        // Continuously update the FontData object whenever there's a change in the letterDictionary
        fontData = new FontData(letterDictionary);
    }

    // Check if the font data has been saved
    bool HasFont()
    {
        if (PlayerPrefs.HasKey("Saved"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region CreateFont
    // Method to enable the UI elements and start the process of creating the font
    void CreateFont()
    {
        EnableChildren(true);
        currentLetterIndex = 0;
        DisplayCurrentLetter(letters[currentLetterIndex]);
    }

    // Method to update the UI text to display the current letter being worked on
    void DisplayCurrentLetter(string letter)
    {
        letterText.text = letter.ToString();
    }

    // Method to store the drawn lines for the current letter in the letterDictionary, clear the drawn lines, and proceed to the next letter or save the font if all letters have been processed
    public void StoreLetter()
    {
        string currentLetter = letters[currentLetterIndex];
        letterDictionary[currentLetter] = new LetterData(TouchDraw.drawnLineRenderes);

        ClearLetter();

        GoToNextOrSaveFont();
    }

    // Method to destroy the drawn line renderers and clear the TouchDraw.drawnLineRenderers list
    void ClearLetter()
    {
        foreach (LineRenderer lineRenderer in TouchDraw.drawnLineRenderes)
        {
            Destroy(lineRenderer.gameObject);
        }
        TouchDraw.drawnLineRenderes.Clear();
    }

    // Method to move to the next letter or save the font data if all letters have been processed
    void GoToNextOrSaveFont()
    {
        currentLetterIndex++;
        if (currentLetterIndex < letters.Length)
        {
            DisplayCurrentLetter(letters[currentLetterIndex]);
        }
        else
        {
            SaveFont();
        }
    }
    #endregion

    #region WriteText
    // Method to initiate the process of writing a string using the handwritten font with a default starting position
    public void WriteTextWrapper(string stringToWrite)
    {
        StartCoroutine(WriteText(stringToWrite, new Vector3(-5, 0, 0)));
    }

    // Method to initiate the process of writing a string using the handwritten font from a specific starting position
    public void WriteTextWrapper(string stringToWrite, Vector3 startingPosition)
    {
        StartCoroutine(WriteText(stringToWrite, startingPosition));
    }

    // Coroutine to write the specified string one character at a time
    IEnumerator WriteText(string stringToWrite, Vector3 startingPosition)
    {
        int i = 0;
        while (i < stringToWrite.Length)
        {
            string letter = stringToWrite[i].ToString();

            // Skip writing spaces
            if (letter != " ")
            {
                drawingLetter = true;
                StartCoroutine(AnimateLetter(letterDictionary[letter], startingPosition + Vector3.right * i));
            }

            // Wait for the letter animation to complete before moving to the next character
            while (drawingLetter)
            {
                yield return null;
            }

            i++;

            yield return null;
        }
        yield return null;
    }

    // Coroutine to animate the drawing of a single letter using the stored letter data
    IEnumerator AnimateLetter(LetterData letterData, Vector3 position)
    {
        foreach (Line line in letterData.lines)
        {
            GameObject newLineGameObject = Instantiate(Resources.Load("Line") as GameObject, position, Quaternion.identity);
            LineRenderer lineRenderer = newLineGameObject.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
            int pointIndex = 0;
            while (pointIndex < line.line.Length)
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(pointIndex, line.line[pointIndex]);
                pointIndex++;
                yield return null;
            }
            yield return null;
        }
        drawingLetter = false;
        yield return null;
    }
    #endregion

    #region Save
    // Method to save the font data as a JSON string and store it in a file
    public void SaveFont()
    {
        string jsonString = JsonUtility.ToJson(new FontData(letterDictionary));
        string fileName = Application.persistentDataPath + "/HandWrittenFont.txt";
        File.WriteAllText(fileName, jsonString);
        PlayerPrefs.SetInt("Saved", 1);
    }
    #endregion

    #region Load
    // Method to load the font data from a saved JSON file
    void LoadFont()
    {
        EnableChildren(false);

        string completeFileName = Application.persistentDataPath + "/HandWrittenFont.txt";
        if (File.Exists(completeFileName))
            JsonToData(File.ReadAllText(completeFileName));
        else
            Debug.Log("Need a valid name");
    }

    // Method to convert JSON data back to FontData and populate the letterDictionary
    void JsonToData(string jsonString)
    {
        FontData loadedFont = JsonUtility.FromJson<FontData>(jsonString);
        int i = 0;
        letters = loadedFont.letterKeys.ToArray();
        foreach (LetterData letter in loadedFont.letterDataList)
        {
            letterDictionary[letters[i]] = letter;
            i++;
        }
    }
    #endregion

    #region HelperFunctions
    // Method to enable or disable all child objects of this GameObject
    void EnableChildren(bool isActive)
    {
        foreach (Transform child in this.transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }
    #endregion
}

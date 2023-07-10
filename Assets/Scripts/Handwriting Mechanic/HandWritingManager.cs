using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandwritingManager : MonoBehaviour
{
    public TouchDraw touchDraw;
    public HandWrittentFont handwrittenFont;
    public Text letterText;
    public Button saveButton;

    private List<Color> letterColors = new List<Color>();

    private void Start()
    {
        // Add colors for each letter (you can customize this list as you like)
        letterColors.Add(Color.red);
        letterColors.Add(Color.green);
        letterColors.Add(Color.blue);
        // ... add more colors as needed ...

        // Subscribe to the button's click event to trigger saving the font data
        saveButton.onClick.AddListener(SaveFontData);
    }

    private void Update()
    {
        // Display the current letter in the UI Text component
        letterText.text = handwrittenFont.letters[handwrittenFont.currentLetterIndex];
    }

    public void DrawLetter()
    {
        // Set the color for the current letter based on the index
        int colorIndex = handwrittenFont.currentLetterIndex % letterColors.Count;
        touchDraw.SetLineColor(letterColors[colorIndex]);

        // Start drawing the line
        touchDraw.StartLine();
    }

    public void SaveFontData()
    {
        // Save the font data using HandWrittenFont's SaveFont method
        handwrittenFont.SaveFont();

        // Optionally, you can display a message or perform other actions after saving the font data.
    }
}

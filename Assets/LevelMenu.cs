using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;

    public void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("Unlocked Level", 1);
        for(int i =0; i<buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for(int i=0; i<unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void selectLevel(int levekId)
    {
        string lelvelName = "Level " + levekId;
        SceneManager.LoadScene(lelvelName);
    }
}

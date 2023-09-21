using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_UI : MonoBehaviour
{
    public Button button;
   public void plyBtn()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void quitBtn()
    {
        Application.Quit();
        Debug.Log("Game quit");
    }

    public void cancelBtn()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void settingBtn()
    {
        Button mybtn = button.GetComponent<Button>();
        mybtn.onClick.AddListener(btnPressed);
    }

    public void btnPressed()
    {
        GameObject btn = GameObject.Find("Settings");
        Vector3 pos = btn.transform.position;
        pos.x -= 10f;
        btn.transform.position = pos;

    }

    public void settingCancel()
    {
        SceneManager.LoadSceneAsync(0);
        Debug.Log("The settings cancel pressed");
    }
}

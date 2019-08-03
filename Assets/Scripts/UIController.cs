using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum UIMode
{
    Start,
    Game,
    Defeat,
    Vitory
}

[System.Serializable]
public class UIPage
{
    public UIMode Mode;
    public GameObject Root;
}

public class UIController : MonoBehaviour
{
    public UIPage[] pages;
    public TextMeshProUGUI TimerLabel;
    string TimerFormat ="";
    public void Start()
    {
        UpdatePages(UIMode.Start);
    }

    public void StartGame()
    {
        GameRule.get.StartGame();
        UpdatePages(UIMode.Game);
    }

    public void UpdatePages(UIMode mode)
    {
        foreach(UIPage uIPage in pages)
        {
            if(uIPage.Mode != mode)
            {
                uIPage.Root.SetActive(false);
            }
            else
            {
                uIPage.Root.SetActive(true);
            }
        }
    }

    public void Update()
    {
        float timer = GameRule.get.GetTimeLeft();
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        float minutes = timeSpan.Minutes;
        float seconds = timeSpan.Seconds;
        float miliseconds = timeSpan.Milliseconds/10;

        if (TimerFormat =="")
        {
            TimerFormat = TimerLabel.text;
        }

        string niceTime = string.Format(TimerFormat, minutes, seconds, miliseconds);
        TimerLabel.text = niceTime;
    }
}

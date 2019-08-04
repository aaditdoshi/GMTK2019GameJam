using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public AudioClip audio;
}

public class UIController : MonoBehaviour
{
    public UIPage[] pages;
    public TextMeshProUGUI TimerLabel;
    public Image HealthProgressBar;
    public float HealthProgressWidth;
    public AudioClip ButtonClick;
    public AudioSource MusicSource;
    string TimerFormat ="";
    public void Start()
    {
        MusicSource = GetComponent<AudioSource>();
        UpdatePages(UIMode.Start);
        HealthProgressWidth = HealthProgressBar.rectTransform.rect.width;
    }

    public void StartGame()
    {
        GameRule.get.StartGame();
        GameRule.get.GetAudio().PlayOneShot(ButtonClick);
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
                if(uIPage.audio)
                {
                    MusicSource.Stop();
                    MusicSource.clip = uIPage.audio;
                    MusicSource.Play();
                }
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

        float HealthRatio = GameRule.get.GetBossHealth();
        HealthProgressBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, HealthRatio * HealthProgressWidth);
    }

}

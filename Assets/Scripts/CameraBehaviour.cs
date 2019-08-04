using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    BoxCollider2D ScreenBounds;

    private Vector2 resolution;
    private Vector3 oldPosition;
    Vector3 startCamera;
    Vector3 finishCamera;
    float startSize;
    float finishSize;
    bool LerpToCom = false;
    private float LerpFactor = 0.0f;
    public float LerpSpeed = 1.0f;
    private void Awake()
    {
        oldPosition = transform.position;
    }

    private void Update()
    {
        if (resolution.x != Screen.width || resolution.y != Screen.height)
        {

            //Do stuff
            UpdateScreen();
            resolution.x = Screen.width;
            resolution.y = Screen.height;

        }

        if(LerpToCom)
        {
            UpdateLerp();
        }

    }

    private void UpdateLerp()
    {
        LerpFactor += Time.deltaTime * LerpSpeed;
        transform.position = Vector3.Lerp(startCamera, finishCamera, Mathf.Clamp(LerpFactor, 0.0f, 1.0f));
        Camera.main.orthographicSize = Mathf.Lerp(startSize, finishSize, Mathf.Clamp(LerpFactor, 0.0f, 1.0f));

        if(LerpFactor > 1.0f)
        {
            LerpToCom = false;
            LerpFactor = 0.0f;
        }
    }

    // Start is called before the first frame update
    public void Start()
    {

        resolution = new Vector2(Screen.width, Screen.height);
        ScreenBounds = GetComponent<BoxCollider2D>();        
        UpdateScreen();
    }

    public void ResetCamera()
    {
        transform.position = oldPosition;
        LerpToCom = false;
        UpdateScreen();
    }

    public void FocusCamera(Vector3 player, Vector3 prince)
    {
        player += Vector3.up * 1.0f;
        prince += Vector3.up * 1.0f;
        Vector3 Max = Vector3.zero;
        Max.x = Mathf.Max(player.x, prince.x);
        Max.y = Mathf.Max(player.y, prince.y);

        Vector3 Min = Vector3.zero;
        Min.x = Mathf.Min(player.x, prince.x);
        Min.y = Mathf.Min(player.y, prince.y);


        Vector3 ScreenMax = Camera.main.WorldToScreenPoint(Max);
        Vector3 ScreenMin = Camera.main.WorldToScreenPoint(Min);

        float width = Mathf.Max(ScreenMax.x - ScreenMin.x + 100.0f, 500.0f);
        float height = Mathf.Max(ScreenMax.y - ScreenMin.y + 100.0f, 500.0f);

        float ratio = Mathf.Max(width / Screen.width, height / Screen.height);


        Vector3 COM =(player + prince) / 2;
        COM.z = transform.position.z;

        transform.position = COM;

        startCamera = transform.position;
        finishCamera = COM;
        startSize = Camera.main.orthographicSize;
        finishSize = Camera.main.orthographicSize * ratio;
        LerpToCom  = true;
    }

    void UpdateScreen()
    { 

        Vector3 ScreenMax = Camera.main.WorldToScreenPoint(ScreenBounds.bounds.max);
        Vector3 ScreenMin = Camera.main.WorldToScreenPoint(ScreenBounds.bounds.min);

        float width = ScreenMax.x - ScreenMin.x;
        float height = ScreenMax.y - ScreenMin.y;

        //Debug.Log(width + " " + height);

        float ratio = Mathf.Max(width / Screen.width, height / Screen.height);

        Camera.main.orthographicSize = Camera.main.orthographicSize * ratio;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    BoxCollider2D ScreenBounds;

    private Vector2 resolution;

    private void Awake()
    {
        
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

    }
    // Start is called before the first frame update
    public void Start()
    {

        resolution = new Vector2(Screen.width, Screen.height);
        ScreenBounds = GetComponent<BoxCollider2D>();
        UpdateScreen();
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

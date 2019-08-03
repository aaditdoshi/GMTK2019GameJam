using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueBehaviour : MonoBehaviour
{
    [HideInInspector]
    public BoxCollider2D BoxCollider;

    private void Awake()
    {
        BoxCollider = GetComponentInChildren<BoxCollider2D>();
    }

    public void Start()
    {
        GameRule.get.AddStatue(this);
    }
}
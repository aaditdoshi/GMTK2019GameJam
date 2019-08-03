using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArrowStatus
{
    InHand,
    Flying,
    Landed
}

public class ArrowBehaviour : MonoBehaviour
{
    public ArrowStatus Status;

    public void Start()
    {
        GameRule.get.AddArrow(this);
    }
}

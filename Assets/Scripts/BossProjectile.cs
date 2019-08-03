using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    Vector3 VelcotiyDirection;
    Rigidbody2D Rigidbody;

    public float Speed;

    public void SetupVelocity(Vector3 offset)
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.velocity =  offset.normalized * Speed;
        Destroy(gameObject, 20.0f);
    }
    

    void OnCollisionEnter2D(Collision2D col)
    {
        Impact(col);
    }

    private void Impact(Collision2D col)
    {
        Destroy(gameObject);
        PlayerMovementScript player = col.gameObject.GetComponent<PlayerMovementScript>();
        if (player)
        {
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        Impact(col);
    }
}

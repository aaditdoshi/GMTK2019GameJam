using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField]
    private float PlayerMovementSpeed= 100;
    Rigidbody2D rigidbody2d;
    SkeletonAnimation skeletonAnimation;
    Animator animator;
    public enum EnumPlayerDirection
    {
        Up,
        Right,
        Down,
        Left
    }
    Vector2 LastVelocity;
    bool bInAttack = false;
    [HideInInspector]
    public EnumPlayerDirection PlayerDirection;

    public void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        animator = GetComponentInChildren<Animator>();
        LastVelocity = Vector2.zero;
        PlayerDirection = EnumPlayerDirection.Right;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator.Play("Idle");
    }

    public void TeleportTo(Vector3 respawnPosition)
    {
        rigidbody2d.position = respawnPosition;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!GameRule.get.IsGameActive())
        {
            rigidbody2d.velocity = Vector3.zero;
            LastVelocity = Vector3.zero;
            return;
        }

        Vector2 PlayerInput = GetPlayerInputVector();
        PlayerDirection = GetPlayerDirection(PlayerInput);
        rigidbody2d.velocity = Vector3.zero;
        rigidbody2d.velocity = PlayerInput * PlayerMovementSpeed;
        LastVelocity = rigidbody2d.velocity;


    }

    private void Update()
    {
        if (!GameRule.get.IsGameActive())
        {
            return;
        }
        if (!bInAttack)
        {
            if (Mathf.Approximately(LastVelocity.sqrMagnitude, 0.0f))
            {
                animator.Play("Idle");
            }
            else
            {
                animator.Play("Run");
            }
        }
        if (LastVelocity.x != 0)
        {
            if (LastVelocity.x > 0)
            {
                if (skeletonAnimation.Skeleton.ScaleX < 0)
                {
                    skeletonAnimation.Skeleton.ScaleX = -skeletonAnimation.Skeleton.ScaleX;
                }
            }
            else
            {
                if (skeletonAnimation.Skeleton.ScaleX > 0)
                {
                    skeletonAnimation.Skeleton.ScaleX = -skeletonAnimation.Skeleton.ScaleX;
                }
            }
        }
      
    }

    public SkeletonAnimation GetSkeleton()
    {
        return skeletonAnimation;
    }

    public void SetInAttack(bool value)
    {
        bInAttack = value;
    }

    Vector2 GetPlayerInputVector()
    {
        Vector2 InputVector = new Vector2();
        if ( Mathf.Abs(Input.GetAxis("Vertical")) > 0)
        {
            InputVector.y = Input.GetAxis("Vertical");
        }

        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            InputVector.x = Input.GetAxis("Horizontal");
        }
       // Debug.Log(InputVector);
        
        return InputVector;
    }

    EnumPlayerDirection GetPlayerDirection(Vector2 PlayerMovementVector)
    {

        if (Mathf.Abs(PlayerMovementVector.y)> Mathf.Abs(PlayerMovementVector.x))
        {
            if(PlayerMovementVector.y>0)
            {
                PlayerDirection = EnumPlayerDirection.Up;
            }
            if(PlayerMovementVector.y < 0)
            {
                PlayerDirection = EnumPlayerDirection.Down;
            }
        }
        else
        {
            if (PlayerMovementVector.x > 0)
            {
                PlayerDirection = EnumPlayerDirection.Right;
            }
            if (PlayerMovementVector.x < 0)
            {
                PlayerDirection = EnumPlayerDirection.Left;
            }
        }

        return PlayerDirection;
    }
}

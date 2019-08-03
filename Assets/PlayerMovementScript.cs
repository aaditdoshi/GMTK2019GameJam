using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField]
    private float PlayerMovementSpeed= 100;

    public enum EnumPlayerDirection
    {
        Up,
        Right,
        Down,
        Left
    }

    public EnumPlayerDirection PlayerDirection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

       Vector3 PlayerInput = GetPlayerInputVector();
        PlayerDirection = GetPlayerDirection(PlayerInput);

        gameObject.transform.position += PlayerInput * Time.deltaTime * PlayerMovementSpeed;
        
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
        Debug.Log(PlayerDirection);
        return PlayerDirection;
    }
}

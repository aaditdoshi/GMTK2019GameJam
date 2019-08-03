using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
   public Vector3 MovementDirection;

    [SerializeField]
    private float LaunchSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLaunchDirection(Vector3 LaunchDir)
    {
        Rigidbody2D rigidbodycomp = GetComponent<Rigidbody2D>();
        rigidbodycomp.velocity = LaunchDir * LaunchSpeed;
    }
}

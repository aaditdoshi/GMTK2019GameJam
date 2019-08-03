using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
   public Vector3 MovementDirection;

    public int BossProjectileCollisionLayer;
    public int PlayerProjectileCollisionLayer;
    [SerializeField]
    private float LaunchSpeed = 2;

<<<<<<< HEAD
    [SerializeField]
    private float AngularVelocity = 2;

    // Start is called before the first frame update
    void Start()
    {

        Rigidbody2D rigidbodycomp = GetComponent<Rigidbody2D>();
        rigidbodycomp.angularVelocity = AngularVelocity;
=======

    public void SetLaunchDirection(Vector3 LaunchDir)
    {
        Rigidbody2D rigidbodycomp = GetComponent<Rigidbody2D>();
        rigidbodycomp.velocity = LaunchDir * LaunchSpeed;
    }
        
    void OnCollisionEnter2D(Collision2D col)
    {
        Impact(col);
>>>>>>> Temp
    }

    private void Impact(Collision2D col)
    {
       
        BossBehaviour bossBehaviour = col.gameObject.GetComponent<BossBehaviour>();
        if(bossBehaviour)
        {
            bossBehaviour.TakeDamage(col.GetContact(0).point, col.GetContact(0).collider);
        }
        
    }

    void OnCollisionStay2D(Collision2D col)
    {
        Impact(col);
    }

    void OnCollisionEnter2D(Collision2D Col)
    {
        Debug.Log("HIT");
        gameObject.layer = BossProjectileCollisionLayer;
    }
}

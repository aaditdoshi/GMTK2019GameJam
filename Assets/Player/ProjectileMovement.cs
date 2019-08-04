using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArrowStatus
{
    InHand,
    Flying,
    Landed
}

public class ProjectileMovement : MonoBehaviour
{


    public ArrowStatus Status;

    public Vector3 MovementDirection;

    public int BossProjectileCollisionLayer;
    public int PlayerProjectileCollisionLayer;
    [SerializeField]
    private float LaunchSpeed = 2;

    Vector3 PostHitLandingLocation;
    bool bReturning = false;

    [SerializeField]
    private float AngularVelocity = 2;

    // Start is called before the first frame update
    void Start()
    {
        GameRule.get.RegisterPlayerProjectile(this);
        Rigidbody2D rigidbodycomp = GetComponent<Rigidbody2D>();
        rigidbodycomp.angularVelocity = AngularVelocity;
        Status = ArrowStatus.Flying;
    }

    void Update()
    {
        if (bReturning && Status == ArrowStatus.Flying)
        {
        
            Rigidbody2D rigidbodycomp = GetComponent<Rigidbody2D>();
            rigidbodycomp.velocity = (PostHitLandingLocation - gameObject.transform.position).normalized * LaunchSpeed;

            if((PostHitLandingLocation - gameObject.transform.position).sqrMagnitude<0.5)
            {
                Status = ArrowStatus.Landed;
                GameRule.get.RepathBoss();
                rigidbodycomp.velocity = Vector3.zero;
                rigidbodycomp.angularVelocity = 0;
            }

        }
    }

    public void SetLaunchDirection(Vector3 LaunchDir)
    {
        Rigidbody2D rigidbodycomp = GetComponent<Rigidbody2D>();
        rigidbodycomp.velocity = LaunchDir * LaunchSpeed;
    }

    private void Impact(Collision2D col)
    {
        Impact(col.collider);

    }

    void OnCollisionStay2D(Collision2D col)
    {
        Impact(col);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Impact(collider);
       
    }

    private void Impact(Collider2D collider)
    {

        gameObject.layer = BossProjectileCollisionLayer;
        BossBehaviour bossBehaviour = collider.GetComponent<BossBehaviour>();
        if (bossBehaviour)
        {
            bossBehaviour.TakeDamage(transform.position, collider);
           
        }
        bReturning = true;
        PostHitLandingLocation = GameRule.get.GetPositionInBounds(0, GameRule.get.GetDragonAvoidanceRadius());
        
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        Impact(collider);
      
    }
}


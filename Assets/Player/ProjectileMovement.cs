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


    [SerializeField]
    private float AngularVelocity = 2;

    // Start is called before the first frame update
    void Start()
    {
        GameRule.get.RegisterPlayerProjectile(this);
        Rigidbody2D rigidbodycomp = GetComponent<Rigidbody2D>();
        rigidbodycomp.angularVelocity = AngularVelocity;
    }

    public void SetLaunchDirection(Vector3 LaunchDir)
    {
        Rigidbody2D rigidbodycomp = GetComponent<Rigidbody2D>();
        rigidbodycomp.velocity = LaunchDir * LaunchSpeed;
    }

    private void Impact(Collision2D col)
    {
        gameObject.layer = BossProjectileCollisionLayer;
        BossBehaviour bossBehaviour = col.gameObject.GetComponent<BossBehaviour>();
        if (bossBehaviour)
        {
            bossBehaviour.TakeDamage(col.GetContact(0).point, col.GetContact(0).collider);
        }

    }

    void OnCollisionStay2D(Collision2D col)
    {
        Impact(col);
    }
}


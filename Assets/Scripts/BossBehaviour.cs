using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{

    float Health;
    public float MaxHealth;
    public float WeakSpotDamage = 10.0f;
    private bool bAllowMovement;
    private Vector3 MovementTarget = Vector3.negativeInfinity;
    Animator Animator;
    public float ArrowAvoidance = 5.0f;
    public float MovementSpeed = 1.0f;
    public float AttackCooldown = 5.0f;
    public float AngleStepBetweenAttack = 45.0f;
    public int AmountOfProjectile = 5;
    public float PathAcceptableDistance = 0.5f;
    public float GoalAcceptableDistance = 1.0f;
    public float Radius = 1.0f;
    SkeletonAnimation skeletonAnimation;
    Vector3 LastVelocity;
    public GameObject Projectile;
    public GameObject HitVFX;
    public GameObject WeakPointHitVFX;
    private float TimeBeforeAttack;
    private float AngleOffset= 0.0f;
    private float IdleSoundTimer = 0.0f;
    private float TimeFromLastSound = 0.0f;
    private Vector3Int[] CurrentPath;
    private int CurrentSegment = 0;
    public Transform DebugNavTarget;
    public bool bShouldInterruptAttack = true;
    public bool bDebugAttack = false;
    private bool bDebugAttackActive = false;
    private Rigidbody2D rigidbody2D;
    AudioSource audioSource;
    public float IdleSoundMaxCooldown = 5.0f;
    public AudioClip[] AttackSound;
    public AudioClip[] IdleSound;
    public AudioClip[] HitSound;
    public AudioClip[] WeakPointHitSound;
    public float WeakPointRadius = 0.5f;
    bool bSoundPlayed = false;
    bool bWeakPointActive = false;
    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        WeakPointState(false);
        audioSource = GetComponent<AudioSource>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    public float GetHealth()
    {
        return Health;
    }

    public void SetAllowMovement(bool value)
    {
        bAllowMovement = value;
    }

    public void Revieve()
    {
        Health = MaxHealth;
        TimeBeforeAttack = AttackCooldown;
        GenerateMovementTarget();
        Animator.Play("Navigation");
        WeakPointState(false);
    }

    public void WeakPointState(bool state)
    {
        bWeakPointActive = state;
    }

    public void GameOver()
    {
        Animator.Play("Win");
    }

    public void Attack()
    {
        if (!bSoundPlayed)
        {
            bSoundPlayed = true;
            if (AttackSound.Length > 0)
            {
                audioSource.PlayOneShot(AttackSound[UnityEngine.Random.Range(0, AttackSound.Length)]);
            }
        }

        Spine.Bone bone = skeletonAnimation.skeleton.FindBone("drag_head5");
        Vector3 weaponSlotPosition = skeletonAnimation.transform.TransformPoint(new Vector3(bone.WorldX, bone.WorldY, 0f));
        Vector2 ProjectileSpawnLocation = weaponSlotPosition;

        float angleStepSize = 360.0f / AmountOfProjectile;
        for(int angleStep = 0; angleStep < AmountOfProjectile; angleStep++)
        {
            float rad = (angleStepSize * angleStep + AngleOffset) * Mathf.Deg2Rad ;
            Vector2 Offset;
            Offset.x = -Radius *  Mathf.Sin(rad);
            Offset.y = Radius *  Mathf.Cos(rad);            

            GameObject go = GameObject.Instantiate<GameObject>(Projectile, ProjectileSpawnLocation + Offset, Quaternion.identity);
            BossProjectile bossProjectile = go.GetComponent<BossProjectile>();
            bossProjectile.SetupVelocity(Offset);
        }

        AngleOffset += AngleStepBetweenAttack;
    }

    void FixedUpdate()
    {
        if (bAllowMovement)
        {
            bDebugAttackActive = true;
            if ((transform.position - MovementTarget).sqrMagnitude < GoalAcceptableDistance * GoalAcceptableDistance || CurrentSegment < 0)
            {
                GenerateMovementTarget();
            }


            if (CurrentSegment >= 0)
            {
                if (CurrentSegment < CurrentPath.Length)
                {

                    Vector3 goal = CurrentPath[CurrentSegment];
                    Vector3 direction = (goal - transform.position).normalized;

                    LastVelocity = direction * Time.deltaTime * MovementSpeed; ;
                    transform.position += LastVelocity;
                    float distance = (goal - transform.position).sqrMagnitude;

                    if (distance < PathAcceptableDistance * PathAcceptableDistance)
                    {
                        CurrentSegment++;
                    }
                }
                else
                {
                    Vector3 direction = (MovementTarget - transform.position).normalized;
                    LastVelocity = direction * Time.deltaTime * MovementSpeed; ;
                    transform.position += LastVelocity;
                }

            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        TimeBeforeAttack -= Time.deltaTime;

        if (LastVelocity.x != 0)
        {
            if (LastVelocity.x > 0)
            {
                if (skeletonAnimation.Skeleton.ScaleX > 0)
                {
                    skeletonAnimation.Skeleton.ScaleX = -skeletonAnimation.Skeleton.ScaleX;
                }
            }
            else
            {
                if (skeletonAnimation.Skeleton.ScaleX < 0)
                {
                    skeletonAnimation.Skeleton.ScaleX = -skeletonAnimation.Skeleton.ScaleX;
                }
            }
        }



        if (bAllowMovement)
        {
            if (TimeBeforeAttack <= 0.0f)
            {
                StartAttack();
              
            }
            if (!audioSource.isPlaying)
            {
                TimeFromLastSound += Time.deltaTime;
                if (TimeFromLastSound > IdleSoundTimer)
                {
                    if (IdleSound.Length > 0)
                    {
                        audioSource.PlayOneShot(IdleSound[UnityEngine.Random.Range(0, IdleSound.Length)]);
                    }
                }
            }
            else
            {
                if (TimeFromLastSound > 0.0f)
                {
                    IdleSoundTimer = UnityEngine.Random.Range(IdleSoundMaxCooldown / 2, IdleSoundMaxCooldown);
                    TimeFromLastSound = 0.0f;
                }
            }
        }     

        if(bDebugAttack && bDebugAttackActive)
        {
            CheckForDebugAttack();
        }     
    }

    private void CheckForDebugAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D result = Physics2D.Raycast(ray.origin, ray.direction, 100.0f,LayerMask.GetMask("Boss"));
            if (result)
            {
                if(result.collider.gameObject == gameObject)
                {
                    TakeDamage(result.point, result.collider);
                }
            }
        }
    }

    public void StartAttack()
    {
        AngleOffset = 0.0f;
        TimeBeforeAttack = AttackCooldown;
        Animator.Play("Attack");
        bSoundPlayed = false;
    }

    public void GenerateMovementTarget()
    {
        bool pointFound = false;
        int attempt = 0;
        CurrentSegment = -1;
        while (pointFound == false && attempt < 5)
        {
            attempt++;
            Vector3 NewMovementTarget = GameRule.get.GetPositionInBounds(ArrowAvoidance);
            if(DebugNavTarget != null)
            {
                NewMovementTarget = DebugNavTarget.position;
            }
            Vector3Int[] path = GameRule.get.GetNavigationGrid().GetPath(NewMovementTarget, transform.position, ArrowAvoidance);
            if (path.Length != 0)
            {
                pointFound = true;
                MovementTarget = NewMovementTarget;
                CurrentPath = path;
                CurrentSegment = 0;
            }
        }
    }

    public void OnDrawGizmos()
    {
        if (CurrentPath != null)
        {
            Gizmos.color = Color.green;
            foreach (Vector3Int segment in CurrentPath)
            {
                Gizmos.DrawSphere(segment, 0.5f);
            }
        }
    }

    public void TakeDamage(Vector3 ImpactPoint, Collider2D collider)
    {
        bool bWeakSpot = false;
        Vector2 ImpactPoint2D = ImpactPoint;


        Spine.Bone bone = skeletonAnimation.skeleton.FindBone("drag_navel");
        Vector3 weaponSlotPosition = skeletonAnimation.transform.TransformPoint(new Vector3(bone.WorldX, bone.WorldY, 0f));
        Vector2 ProjectileSpawnLocation = weaponSlotPosition;
        Vector2 direction = (ImpactPoint - transform.position).normalized;
        if (skeletonAnimation.Skeleton.ScaleX > 0)
        {
            if (direction.x < 0)
            {               
                if ((ProjectileSpawnLocation - ImpactPoint2D).sqrMagnitude < WeakPointRadius * WeakPointRadius)
                {
                    bWeakSpot = true;
                }
            }
        }
        else
        {
            if (direction.x > 0)
            {
                if ((ProjectileSpawnLocation - ImpactPoint2D).sqrMagnitude < WeakPointRadius * WeakPointRadius)
                {
                    bWeakSpot = false;
                }

            }
        }

        if (bWeakSpot)
        {
            Health -= WeakSpotDamage;
        }
        else
        {
            Health -= 1.0f;
        }
        if (Health > 0.0f)
        {
            GameObject VFXprefab;
            if(bWeakSpot)
            {
                VFXprefab = WeakPointHitVFX;
                if (WeakPointHitSound.Length > 0)
                {
                    audioSource.PlayOneShot(WeakPointHitSound[UnityEngine.Random.Range(0, WeakPointHitSound.Length)]);
                }
            }
            else
            {
                VFXprefab = HitVFX;
                if (HitSound.Length > 0)
                {
                    audioSource.PlayOneShot(HitSound[UnityEngine.Random.Range(0, HitSound.Length)]);
                }
            }

            GameObject go =Instantiate<GameObject>(VFXprefab, ImpactPoint - Vector3.forward, Quaternion.identity);
            if(go)
            {
                Destroy(go, 5.0f);
            }
            if (bWeakSpot && (bAllowMovement || bShouldInterruptAttack))
            {
                Animator.Play("HitReaction");
            }
        }
        else
        {
            Animator.Play("Death");
            GameRule.get.DragonDead();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Impact(col);
    }

    private void Impact(Collision2D col)
    {

        PlayerMovementScript player = col.gameObject.GetComponent<PlayerMovementScript>();
        if (player)
        {
            Debug.Log("Damage player" + player);
        }

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

        PlayerMovementScript player = collider.GetComponent<PlayerMovementScript>();
        if (player)
        {
            Debug.Log("Damage player" + player);
        }

    }

    void OnTriggerStay2D(Collider2D collider)
    {
        Impact(collider);
    }
}

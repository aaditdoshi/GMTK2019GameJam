using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootProjectile : MonoBehaviour
{
    [SerializeField]
    private GameObject ProjectilePrefab;

    GameObject SpawnedProjectile;

    public GameObject AimLocation;
    [SerializeField]
    private float SpawnOffset = 10;

    PlayerMovementScript playerMovementComponent;
    public bool HasProjectile = true;
    AudioSource audioSource;
    public AudioClip[] AttackSounds;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMovementComponent = gameObject.GetComponent<PlayerMovementScript>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameRule.get.IsGameActive())
        {
            return;
        }

        if(Input.GetButtonDown("Jump"))
        {
            if (HasProjectile)
            {
                if (!playerMovementComponent.GetAnimator().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    playerMovementComponent.GetAnimator().Play("Attack");
                }
            }
        }

        if(HasProjectile == false && SpawnedProjectile == null)
        {
            HasProjectile = true;
        }

        
    }

    public void Attack()
    {
        HasProjectile = false;
        FireProjectile();
        if (AttackSounds.Length > 0)
        {
            audioSource.PlayOneShot(AttackSounds[UnityEngine.Random.Range(0, AttackSounds.Length)]);
        }
    }

    void FireProjectile()
    {
        Vector3 ProjectileDirection = Vector3.zero;
        if (playerMovementComponent)
        {
           // Debug.Log("Im shooting in the direction " + playerMovementComponent.PlayerDirection);
            switch(playerMovementComponent.PlayerDirection)
            {
                case PlayerMovementScript.EnumPlayerDirection.Up:
                    ProjectileDirection.y = 1;
                    break;
                case PlayerMovementScript.EnumPlayerDirection.Left:
                    ProjectileDirection.x = -1;
                    break;
                case PlayerMovementScript.EnumPlayerDirection.Right:
                    ProjectileDirection.x = 1;
                    break;
                case PlayerMovementScript.EnumPlayerDirection.Down:
                    ProjectileDirection.y = -1;
                    break;
            }
        }

        SkeletonAnimation skeletonAnimation = playerMovementComponent.GetSkeleton();
        Spine.Bone bone = skeletonAnimation.skeleton.FindBone("Prcs_arm_back_empty5");
        Vector3 weaponSlotPosition = skeletonAnimation.transform.TransformPoint(new Vector3(bone.WorldX, bone.WorldY, 0f));
        Vector2 ProjectileSpawnLocation = weaponSlotPosition;



        GameObject CreatedProjectile = Instantiate(ProjectilePrefab, ProjectileSpawnLocation, Quaternion.identity, null);
        ProjectileMovement projectileMovement = CreatedProjectile.GetComponent<ProjectileMovement>();
        if(projectileMovement)
        {
           // Debug.Log(ProjectileDirection);
            projectileMovement.SetLaunchDirection( ProjectileDirection);
        }

        SpawnedProjectile = CreatedProjectile;
    }
    
    public void Reset()
    {
        if (HasProjectile == false)
        {
            HasProjectile = true;
            Destroy(SpawnedProjectile);
        }
    }

    void OnCollisionEnter2D(Collision2D Col)
    {
        if (Col.gameObject.GetComponent<ProjectileMovement>())
        {
            Destroy(Col.gameObject);
            HasProjectile = true;
        }
    }
}

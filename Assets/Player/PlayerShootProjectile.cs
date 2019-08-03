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
    bool HasProjectile = true;
    // Start is called before the first frame update
    void Start()
    {
        playerMovementComponent = gameObject.GetComponent<PlayerMovementScript>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            if(HasProjectile)
            {
               // Debug.Log("FIRE");
                FireProjectile();
                HasProjectile = false;
            }
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
        Vector2 ProjectileSpawnLocation = gameObject.transform.position + ProjectileDirection * SpawnOffset;

       
        GameObject CreatedProjectile = Instantiate(ProjectilePrefab, ProjectileSpawnLocation, Quaternion.identity, null);
        ProjectileMovement projectileMovement = CreatedProjectile.GetComponent<ProjectileMovement>();
        if(projectileMovement)
        {
           // Debug.Log(ProjectileDirection);
            projectileMovement.SetLaunchDirection( ProjectileDirection);
        }

        SpawnedProjectile = CreatedProjectile;
    }

    void OnCollisionEnter2D(Collision2D Col)
    {
        if (Col.gameObject == SpawnedProjectile)
        {
            Destroy(SpawnedProjectile);
            HasProjectile = true;
        }
    }
}

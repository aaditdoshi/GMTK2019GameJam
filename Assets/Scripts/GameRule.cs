using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRule : MonoBehaviour
{
    static GameRule sGameRule;
    public float Timer = 60.0f;
    public float DragonAvoidanceRadius = 10.0f;
    float RuntimeTimer = 0.0f;
    NavigationGrid NavigationGrid;
    BoxCollider2D BoxCollider;

    public static GameRule get{
        get
        {
            return sGameRule;
        }
    }

    List<StatueBehaviour> AllStatues = new List<StatueBehaviour>();
    public  ProjectileMovement Boomerang;
    public GameObject PrincePrefab;
    public GameObject WinVFX;
    BossBehaviour bossBehaviour;
    CameraBehaviour cameraBehaviour;
    UIController UIController;
    AudioSource audioSource;
    bool bTimerActive = false;
    GameObject princeGO;
    private Vector3 DebugLastTargetPosition;
    private Vector3 DebugLastFixedTargetPosition;

    // Start is called before the first frame update
    void Awake()
    {
        sGameRule = this;
        BoxCollider = GetComponent<BoxCollider2D>();
        NavigationGrid = GetComponent<NavigationGrid>();
        UIController = FindObjectOfType<UIController>();
        audioSource = GetComponent<AudioSource>();
        RuntimeTimer = Timer;
        cameraBehaviour = FindObjectOfType<CameraBehaviour>();
    }

    public AudioSource GetAudio()
    {
        return audioSource;
    }

    public void StartGame()
    {
        if (bossBehaviour == null)
        {
            bossBehaviour = FindObjectOfType<BossBehaviour>();
        }
        RuntimeTimer = Timer;
        bossBehaviour.Revieve();
        bTimerActive = true;

        PlayerShootProjectile player = FindObjectOfType<PlayerShootProjectile>();
        player.Reset();
        cameraBehaviour.ResetCamera();
        if (princeGO)
        {
            Destroy(princeGO);
        }

        foreach (StatueBehaviour statue in AllStatues)
        {
            Destroy(statue.gameObject);
        }
        AllStatues.Clear();

    }

    public void RepathBoss()
    {
        bossBehaviour.Repath();
    }

    public float GetTimeLeft()
    {
        if(RuntimeTimer <0.0f)
        {
            return 0.0f;
        }
        return RuntimeTimer;
    }

    public float GetBossHealth()
    {
        if (bossBehaviour)
        {
            return bossBehaviour.GetHealth() / bossBehaviour.MaxHealth;
        }
        else
        {
            return 1.0f;
        }
    }

    public void DragonDead()
    {
        UIController.UpdatePages(UIMode.Vitory);
        bTimerActive = false;
        PlayerMovementScript playerMovement = FindObjectOfType<PlayerMovementScript>();

        playerMovement.GetAnimator().Play("Win");
        Vector3 Direction = bossBehaviour.transform.position - playerMovement.transform.position;
        if (Direction.x > 0)
        {
            if (playerMovement.GetSkeleton().Skeleton.ScaleX < 0)
            {
                playerMovement.GetSkeleton().Skeleton.ScaleX = -playerMovement.GetSkeleton().Skeleton.ScaleX;
            }
        }
        else
        {
            if (playerMovement.GetSkeleton().Skeleton.ScaleX > 0)
            {
                playerMovement.GetSkeleton().Skeleton.ScaleX = -playerMovement.GetSkeleton().Skeleton.ScaleX;
            }
        }

        if (PrincePrefab)
        {

            princeGO = Instantiate(PrincePrefab, bossBehaviour.transform.position, Quaternion.identity);

            GameObject VFX = Instantiate(WinVFX, bossBehaviour.transform.position, Quaternion.identity);
            Destroy(VFX, 5.0f);
            bossBehaviour.gameObject.SetActive(false);

            if (princeGO)
            {
                SkeletonAnimation PrinceSkeleton = princeGO.GetComponentInChildren<SkeletonAnimation>();
                if (Direction.x > 0)
                {
                    if (PrinceSkeleton.Skeleton.ScaleX < 0)
                    {
                        PrinceSkeleton.Skeleton.ScaleX = -PrinceSkeleton.Skeleton.ScaleX;
                    }
                }
                else
                {
                    if (PrinceSkeleton.Skeleton.ScaleX > 0)
                    {
                        PrinceSkeleton.Skeleton.ScaleX = -PrinceSkeleton.Skeleton.ScaleX;
                    }
                }
            }
        }

        cameraBehaviour.FocusCamera(playerMovement.transform.position, bossBehaviour.transform.position);
    }

    public bool IsGameActive()
    {
        return bTimerActive;
    }

    public void GameOver()
    {
        UIController.UpdatePages(UIMode.Defeat);
        bossBehaviour.GameOver();
        bTimerActive = false;
    }

    public float GetDragonAvoidanceRadius()
    {
        return DragonAvoidanceRadius;
    }

    public Vector3 GetPositionInBounds(float arrowAvoidance, float dragonAvoidanceRadius = 0.0f)
    {
        Vector3 positon = BoxCollider.transform.position;
        Vector2 colliderPos =new Vector2(positon .x, positon .y) + BoxCollider.offset;
        float randomPosX = UnityEngine.Random.Range(colliderPos.x - BoxCollider.size.x / 2, colliderPos.x + BoxCollider.size.x / 2);
        float randomPosY = UnityEngine.Random.Range(colliderPos.y - BoxCollider.size.y / 2, colliderPos.y + BoxCollider.size.y / 2);
        Vector3 target = new Vector3(randomPosX, randomPosY, positon.z);

        if (dragonAvoidanceRadius > 0.0f)
        {       
            DebugLastTargetPosition = target;
        }
        if(Boomerang && Boomerang.Status == ArrowStatus.Landed)
        {
            if ((Boomerang.transform.position - target).sqrMagnitude < arrowAvoidance * arrowAvoidance)
            {
                const int MAX_ATTEMPT = 10;
                for (int attempt = 0; attempt < MAX_ATTEMPT; attempt++) 
                {
                    float angleStepSize = 360.0f / MAX_ATTEMPT;
                    float rad = angleStepSize * attempt * Mathf.Deg2Rad;
                    Vector3 Offset;
                    Offset.x = -(arrowAvoidance + 0.1f )* Mathf.Sin(rad);
                    Offset.y = (arrowAvoidance + 0.1f )* Mathf.Cos(rad);
                    Offset.z = 0.0f;

                    if(BoxCollider.bounds.Contains(Boomerang.transform.position + Offset))
                    {
                        target = Boomerang.transform.position + Offset;
                        break;
                    }
                }
            }
        }

        Vector3 bossPosition = bossBehaviour.transform.position + bossBehaviour.GetHalfSize();
        if ((bossPosition - target).sqrMagnitude < dragonAvoidanceRadius * dragonAvoidanceRadius)
        {
            const int MAX_ATTEMPT = 10;
            for (int attempt = 0; attempt < MAX_ATTEMPT; attempt++)
            {
                float angleStepSize = 360.0f / MAX_ATTEMPT;
                float rad = angleStepSize * attempt * Mathf.Deg2Rad;
                Vector3 Offset;
                Offset.x = -(dragonAvoidanceRadius + 0.1f) * Mathf.Sin(rad);
                Offset.y = (dragonAvoidanceRadius + 0.1f) * Mathf.Cos(rad);
                Offset.z = 0.0f;

                if (BoxCollider.bounds.Contains(bossPosition + Offset))
                {
                    target = bossPosition + Offset;
                    break;
                }
            }
        }
        if (dragonAvoidanceRadius > 0.0f)
            DebugLastFixedTargetPosition = target;

        return target;
    }

    public void AddStatue(StatueBehaviour statue)
    {
        AllStatues.Add(statue);
        bossBehaviour.Repath();
    }

    public bool HasStatue(Bounds node)
    {
        foreach (StatueBehaviour statue in AllStatues)
        {
            if(statue.BoxCollider.bounds.Intersects(node))
            {
                return true;
            }
        }

        return false;
    }

    public void RegisterPlayerProjectile(ProjectileMovement boomerang)
    {
        Boomerang = boomerang;
    }

    public bool NearBoomerang(Vector3Int node, float boomerangAvoidance)
    {
        if(Boomerang != null && Boomerang.Status == ArrowStatus.Landed)
        {
            return (Boomerang.transform.position - node).sqrMagnitude < boomerangAvoidance * boomerangAvoidance;
        }
        return false;
    }

    public NavigationGrid GetNavigationGrid()
    {
        return NavigationGrid;
    }

    public Bounds GetBounds()
    {
        return BoxCollider.bounds;
    }

    public void Update()
    {
        if(bTimerActive)
        {
            RuntimeTimer -= Time.deltaTime;
            if(RuntimeTimer < 0.0f)
            {
                GameOver();
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(DebugLastTargetPosition, DragonAvoidanceRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(DebugLastFixedTargetPosition, 0.2f);

    }
}

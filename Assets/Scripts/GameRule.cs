using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRule : MonoBehaviour
{
    static GameRule sGameRule;
    public float Timer = 60.0f;
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
    ProjectileMovement Boomerang;
    BossBehaviour bossBehaviour;
    UIController UIController;
    AudioSource audioSource;
    public AudioClip BattleSong;
    public AudioClip VictorySong;
    bool bTimerActive = false;
    // Start is called before the first frame update
    void Awake()
    {
        sGameRule = this;
        BoxCollider = GetComponent<BoxCollider2D>();
        NavigationGrid = GetComponent<NavigationGrid>();
        UIController = FindObjectOfType<UIController>();
        audioSource = GetComponent<AudioSource>();
        RuntimeTimer = Timer;
    }

    public AudioSource GetAudio()
    {
        return audioSource;
}

    public void StartGame()
    {
        bossBehaviour = FindObjectOfType<BossBehaviour>();
        RuntimeTimer = Timer;
        bossBehaviour.Revieve();
        bTimerActive = true;
        if(audioSource.clip == VictorySong)
        {
            audioSource.clip = BattleSong;
        }
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
        audioSource.clip = VictorySong;
    }

    public bool IsGameActive()
    {
        return bTimerActive;
    }

    public void GameOver()
    {
        foreach(StatueBehaviour statue in AllStatues)
        {
            Destroy(statue.gameObject);
        }
        UIController.UpdatePages(UIMode.Defeat);
        bossBehaviour.GameOver();
        bTimerActive = false;
    }

    public Vector3 GetPositionInBounds(float arrowAvoidance)
    {
        Vector3 positon = BoxCollider.transform.position;
        Vector2 colliderPos =new Vector2(positon .x, positon .y) + BoxCollider.offset;
        float randomPosX = UnityEngine.Random.Range(colliderPos.x - BoxCollider.size.x / 2, colliderPos.x + BoxCollider.size.x / 2);
        float randomPosY = UnityEngine.Random.Range(colliderPos.y - BoxCollider.size.y / 2, colliderPos.y + BoxCollider.size.y / 2);
        Vector3 target = new Vector3(randomPosX, randomPosY, positon.z);

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

                    if(BoxCollider.bounds.Contains(target + Offset))
                    {
                        target = target + Offset;
                        break;
                    }
                }
            }
        }

        return target;
    }

    public void AddStatue(StatueBehaviour statue)
    {
        AllStatues.Add(statue);
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
}

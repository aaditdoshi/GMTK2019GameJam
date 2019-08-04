using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathComponent : MonoBehaviour
{
    public GameObject StatuePrefab;
    public GameObject DeathVFX;
    public bool bDmagable = true;
    PlayerMovementScript playerMovementScript;

    private void Awake()
    {
        playerMovementScript = GetComponent<PlayerMovementScript>();
    }

    void OnCollisionEnter2D(Collision2D Col)
    {
        if (Col.gameObject.GetComponent<BossBehaviour>() || Col.gameObject.GetComponent<BossProjectile>())
        {           
            OnDeath();            
        }
    }

    public void OnDeath()
    {
        if (bDmagable)
        {
            GameObject Statue = Instantiate(StatuePrefab, gameObject.transform.position, Quaternion.identity);

            GameObject VFX = Instantiate(DeathVFX, gameObject.transform.position, Quaternion.identity);
            Destroy(VFX, 5.0f);
            GameRule.get.AddStatue(Statue.GetComponent<StatueBehaviour>());
            Vector3 RespawnPosition = GameRule.get.GetPositionInBounds(0.0f, GameRule.get.GetDragonAvoidanceRadius());
            StartInvulTimer();
            playerMovementScript.TeleportTo(RespawnPosition);
        }
    }

    private void StartInvulTimer()
    {
        bDmagable = false;
        StartCoroutine(EnableDamageAfterDelay(0.5f));
    }

    private IEnumerator EnableDamageAfterDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        bDmagable = true; 
    }
}

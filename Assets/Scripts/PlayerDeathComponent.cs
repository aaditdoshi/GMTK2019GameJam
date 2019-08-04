using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathComponent : MonoBehaviour
{
    public GameObject StatuePrefab;
    public GameObject DeathVFX;
    public GameObject SpawnVFX;
    public bool bDmagable = true;
    PlayerMovementScript playerMovementScript;
    PlayerShootProjectile playerShootProjectile;
    AudioSource audioSource;
    public AudioClip DeathSFX;
    private void Awake()
    {
        playerMovementScript = GetComponent<PlayerMovementScript>();
        audioSource = GetComponent<AudioSource>();
        playerShootProjectile = GetComponent<PlayerShootProjectile>();
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

            GameObject SpawnVFXGO = Instantiate(SpawnVFX, RespawnPosition, Quaternion.identity);
            Destroy(SpawnVFXGO, 5.0f);
            StartInvulTimer();
            playerMovementScript.TeleportTo(RespawnPosition);
            if (DeathSFX)
            {
                audioSource.PlayOneShot(DeathSFX);
            }
        }
    }

    private void StartInvulTimer()
    {
        bDmagable = false;
        StartCoroutine(EnableDamageAfterDelay(1.0f));
    }

    private IEnumerator EnableDamageAfterDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        bDmagable = true; 
    }
}

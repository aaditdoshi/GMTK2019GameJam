using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathComponent : MonoBehaviour
{
    public GameObject StatuePrefab;

  
    void OnCollisionEnter2D(Collision2D Col)
    {
        if (Col.gameObject.GetComponent<BossBehaviour>() || Col.gameObject.GetComponent<BossProjectile>())
        {
            OnDeath();
        }
    }

    void OnDeath()
    {
        GameObject Statue = Instantiate(StatuePrefab,gameObject.transform.position,Quaternion.identity);
        
        GameRule.get.AddStatue(Statue.GetComponent<StatueBehaviour>());
        Vector3 RespawnPosition = GameRule.get.GetPositionInBounds(0.0f);
        Instantiate(gameObject, RespawnPosition, Quaternion.identity, null);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountEnemy : MonoBehaviour
{
    EnemyCountSpawner spawner;
    EnemyBase enemyBase;
    private void Awake()
    {
        spawner=GetComponentInParent<EnemyCountSpawner>();
        enemyBase=GetComponent<EnemyBase>();
        if(enemyBase == null )
            enemyBase=GetComponentInChildren<EnemyBase>();  
    }

    private void OnEnable()
    {
        StartCoroutine(DeadWait());
    }
    IEnumerator DeadWait()
    {
        WaitUntil wait = new WaitUntil(() => enemyBase.HP < 1);

        yield return wait;
        spawner.count--;
    }
}

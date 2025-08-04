using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /// <summary>
    /// 소환될 적
    /// </summary>
    [SerializeField] GameObject enemy;
    [SerializeField] Transform spawnPos;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Instantiate(enemy, spawnPos.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingFishSpanwer : MonoBehaviour
{
    [SerializeField] GameObject flyfish;
    [SerializeField] float delay;
    [SerializeField] int spawnCount;
    [SerializeField] Transform spawnPosition;
    [SerializeField] BackGroundMove backGroundMove;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            transform.parent=collision.transform;
            StartCoroutine(SpawnStart());
            backGroundMove.moveSpeed = 0.2f;
        }
    }

    IEnumerator SpawnStart()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(flyfish,spawnPosition.position,Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObject_Item : EnemyHP
{
    /// <summary>
    /// 떨어뜨릴 아이템
    /// </summary>
    [SerializeField] GameObject dropItem;
    /// <summary>
    /// 생성될 아이템의 개수(횟수)
    /// </summary>
    [SerializeField] int itemCount=1;
    /// <summary>
    /// 아이템의 생성 긴격
    /// </summary>
    [SerializeField] float itemSpawnDealy;
    [SerializeField] BoxCollider2D col;
    private void Awake()
    {
        col= GetComponent<BoxCollider2D>();
    }

    protected override void OnDie()
    {
        col.enabled = false;
        StartCoroutine(SpawnItems());
    }
    /// <summary>
    /// 아이템을 일정 간격으로 스폰하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnItems()
    {
        for(int i = 0; i < itemCount; i++)
        {
            Instantiate(dropItem,transform.position,Quaternion.identity);
            yield return new WaitForSeconds(itemSpawnDealy);
        }
        gameObject.SetActive(false);
    }
}

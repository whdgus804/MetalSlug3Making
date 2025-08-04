using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObject_Item : EnemyHP
{
    /// <summary>
    /// ����߸� ������
    /// </summary>
    [SerializeField] GameObject dropItem;
    /// <summary>
    /// ������ �������� ����(Ƚ��)
    /// </summary>
    [SerializeField] int itemCount=1;
    /// <summary>
    /// �������� ���� ���
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
    /// �������� ���� �������� �����ϴ� �ڷ�ƾ �Լ�
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

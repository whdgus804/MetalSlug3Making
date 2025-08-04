using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObject_Enemy : EnemyHP
{
    /// <summary>
    /// �÷��̾ ������ Ʈ����
    /// </summary>
    [SerializeField]BoxCollider2D trigger;

    /// <summary>
    /// �߼��� ������ ������Ʈ Ȥ�� ���
    /// </summary>
    [SerializeField] GameObject destroiedObj;

    /// <summary>
    /// ��ȯ�� ��
    /// </summary>
    [SerializeField] GameObject enemy;
    /// <summary>
    /// ���� ��ȯ�� ��ġ
    /// </summary>
    [SerializeField] Transform spawnPos;
    /// <summary>
    /// ��ȯ ������
    /// </summary>
    [SerializeField] float spawnDelay;
    /// <summary>
    /// ��ȯ�� ������ Ȥ�� ���� ��
    /// </summary>
    [SerializeField] int count;
    /// <summary>
    /// ������ �ı��ܾ����� Ƣ��� ����
    /// </summary>
    [SerializeField] GameObject part1;
    [SerializeField] GameObject part2;
    [SerializeField] GameObject part3;
    /// <summary>
    /// ��ȯ�� ���� ��
    /// </summary>
    int spawnEnmyCount;

    /// <summary>
    /// �ı��� ������ ������
    /// </summary>
    [SerializeField]GameObject item;

    /// <summary>
    /// ���� ���� ��ġ ���ʻ��
    /// </summary>
    [SerializeField]Transform leftUpDustRange;  
    /// <summary>
    /// ���� ���� ��ġ ������ �ϴ�
    /// </summary>
    [SerializeField]Transform rightDownDustRange;

    /// <summary>
    /// ������Ʈ�� �ν����� ���� �����ϰԵ� ������Ʈ�� ��ġ
    /// </summary>
    [Space(20.0f)]
    [SerializeField] Transform destroidSpawn;
    /// <summary>
    /// �ν������� �����ϰԵ� ������Ʈ 
    /// </summary>
    [SerializeField] GameObject destroidOBJ;

    EffectItemSpawner iTemSpawner;

    PlayerHealth playerHealth;

    /// <summary>
    /// �÷��̾��� ī�޶� ������Ű�� ��ũ��Ʈ
    /// </summary>
    CameraPointReset cameraPoint;
    private void Awake()
    {
        iTemSpawner=GetComponent<EffectItemSpawner>();
        playerHealth = GameManager.Instance.PlayerHealth;
        cameraPoint = GetComponentInChildren<CameraPointReset>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            trigger.enabled = false;        //Ʈ���� �ߺ� ���� ����

            StartCoroutine(Spawn(0));        //��ȯ �ڷ�ƾ ����
        }
    }
    protected override void OnDie()
    {
        StopAllCoroutines();                                                //��� �ڷ�ƾ ����
        if (iTemSpawner != null)
            iTemSpawner.ItemSpawner();
        if (spawnEnmyCount != count)                                   //��ȯ�ؾ��� ���� ����������
        {
            int leftEnemyCount= count - spawnEnmyCount;                   //�����ִ� ���� ���� ����
            for(int i = 0; i < leftEnemyCount; i++)                         //�����ִ� ���� ����ŭ ����
            {
                if(part1 != null)                                           //������ ������ ���� ��ȯ
                {
                    Instantiate(part1,spawnPos.position, Quaternion.identity);
                }
                if (part2 != null)
                {
                    Instantiate(part2, spawnPos.position, Quaternion.identity);
                }
                if (part3 != null)
                {
                    Instantiate(part3, spawnPos.position, Quaternion.identity);
                }
            }
        }
        if(destroiedObj != null)                                            //�ν��� ������Ʈ�� ����Ǿ� ������
            destroiedObj.SetActive(true);                                   //Ȱ��ȭ

        if(item != null)
        {
            for(int i = 0;i < count; i++)
            {
                Instantiate(item,spawnPos.position, Quaternion.identity);   
            }
        }
        if(destroidSpawn != null)
          Instantiate(destroidOBJ, destroidSpawn.position, Quaternion.identity);
        DustSpawn();
        if(cameraPoint != null)
            cameraPoint.ResetCameraPos();
        gameObject.SetActive(false);                                        //�ش� ������Ʈ ��Ȱ��ȭ
    }
    /// <summary>
    /// ���� ��ȯ�ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawn(int spawnCount)
    {
        if (enemy != null)
        {
            
            for(spawnEnmyCount = spawnCount; spawnEnmyCount < count; spawnEnmyCount++)          //���� ������ ����ŭ ������ �ð� �������� ��ȯ
            {
                //ebug.Log(spawnEnmyCount);
                if (playerHealth.HP > 0)
                {
                    Instantiate(enemy, spawnPos.position, Quaternion.identity);             //�� ��ȯ
                    yield return new WaitForSeconds(spawnDelay);                            //��ٸ���
                }
                else
                {
                    yield return new WaitUntil(() => playerHealth.HP > 0);
                }
            }
        }

    }

    /// <summary>
    /// ������ �����ϴ� �Լ�
    /// </summary>
    void DustSpawn()
    {

        Vector3 vec = Vector3.zero;     //���� ���� ��������
        for(int i = 0;i < 10; i++)
        {
            vec.x = Random.Range(leftUpDustRange.position.x, rightDownDustRange.position.x);  //x�� ��ġ
            vec.y=Random.Range(leftUpDustRange.position.y, rightDownDustRange.position.y);    //y�� ��ġ
            vec += transform.position;
            PoolFactory.Instance.GetDust(vec);                                  //���� ����

        }

    }
   
}

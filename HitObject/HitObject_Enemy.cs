using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObject_Enemy : EnemyHP
{
    /// <summary>
    /// 플레이어를 감지할 트리거
    /// </summary>
    [SerializeField]BoxCollider2D trigger;

    /// <summary>
    /// 뷔서진 상태의 오브젝트 혹은 배경
    /// </summary>
    [SerializeField] GameObject destroiedObj;

    /// <summary>
    /// 소환할 적
    /// </summary>
    [SerializeField] GameObject enemy;
    /// <summary>
    /// 적을 소환할 위치
    /// </summary>
    [SerializeField] Transform spawnPos;
    /// <summary>
    /// 소환 딜레이
    /// </summary>
    [SerializeField] float spawnDelay;
    /// <summary>
    /// 소환할 아이템 혹은 적의 수
    /// </summary>
    [SerializeField] int count;
    /// <summary>
    /// 둥지가 파괴외었을때 튀어나갈 파츠
    /// </summary>
    [SerializeField] GameObject part1;
    [SerializeField] GameObject part2;
    [SerializeField] GameObject part3;
    /// <summary>
    /// 소환한 적의 수
    /// </summary>
    int spawnEnmyCount;

    /// <summary>
    /// 파괴시 생성될 아이템
    /// </summary>
    [SerializeField]GameObject item;

    /// <summary>
    /// 먼지 생성 위치 왼쪽상단
    /// </summary>
    [SerializeField]Transform leftUpDustRange;  
    /// <summary>
    /// 먼지 생성 위치 오른쪽 하단
    /// </summary>
    [SerializeField]Transform rightDownDustRange;

    /// <summary>
    /// 오브젝트가 부숴지고 나서 스폰하게될 오브젝트의 위치
    /// </summary>
    [Space(20.0f)]
    [SerializeField] Transform destroidSpawn;
    /// <summary>
    /// 부숴지고나서 스폰하게될 오브젝트 
    /// </summary>
    [SerializeField] GameObject destroidOBJ;

    EffectItemSpawner iTemSpawner;

    PlayerHealth playerHealth;

    /// <summary>
    /// 플레이어의 카메라를 고정시키는 스크립트
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
            trigger.enabled = false;        //트리고 중복 실행 방지

            StartCoroutine(Spawn(0));        //소환 코루틴 실행
        }
    }
    protected override void OnDie()
    {
        StopAllCoroutines();                                                //모든 코루틴 정지
        if (iTemSpawner != null)
            iTemSpawner.ItemSpawner();
        if (spawnEnmyCount != count)                                   //소환해야할 적이 남아있으면
        {
            int leftEnemyCount= count - spawnEnmyCount;                   //남아있는 적의 수를 저장
            for(int i = 0; i < leftEnemyCount; i++)                         //남아있는 적의 수만큼 저장
            {
                if(part1 != null)                                           //파츠가 있으면 파츠 소환
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
        if(destroiedObj != null)                                            //부숴진 오브젝트가 저장되어 있으면
            destroiedObj.SetActive(true);                                   //활성화

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
        gameObject.SetActive(false);                                        //해당 오브젝트 비활성화
    }
    /// <summary>
    /// 적을 소환하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawn(int spawnCount)
    {
        if (enemy != null)
        {
            
            for(spawnEnmyCount = spawnCount; spawnEnmyCount < count; spawnEnmyCount++)          //적을 지정된 수만큼 지정된 시간 간격으로 소환
            {
                //ebug.Log(spawnEnmyCount);
                if (playerHealth.HP > 0)
                {
                    Instantiate(enemy, spawnPos.position, Quaternion.identity);             //적 소환
                    yield return new WaitForSeconds(spawnDelay);                            //기다리기
                }
                else
                {
                    yield return new WaitUntil(() => playerHealth.HP > 0);
                }
            }
        }

    }

    /// <summary>
    /// 먼지를 생성하는 함수
    /// </summary>
    void DustSpawn()
    {

        Vector3 vec = Vector3.zero;     //먼지 생성 범위백터
        for(int i = 0;i < 10; i++)
        {
            vec.x = Random.Range(leftUpDustRange.position.x, rightDownDustRange.position.x);  //x축 위치
            vec.y=Random.Range(leftUpDustRange.position.y, rightDownDustRange.position.y);    //y축 위치
            vec += transform.position;
            PoolFactory.Instance.GetDust(vec);                                  //먼지 생성

        }

    }
   
}

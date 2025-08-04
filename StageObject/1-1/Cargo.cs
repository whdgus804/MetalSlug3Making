using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cargo : MonoBehaviour
{
    //플레이어가 가까이오면 열리기
    //적 소환 2곳
    //일정 시간 이후 없어지기

    /// <summary>
    /// 행동 시작을 알릴 트리거
    /// </summary>
    [SerializeField] BoxCollider2D trigger;

    /// <summary>
    /// 위 쪽에서 스폰될 적
    /// </summary>
    [SerializeField] GameObject upEnemy;
    /// <summary>
    /// 위에 적이 생성될 위치
    /// </summary>
    [SerializeField] Transform upPos;
    /// <summary>
    /// 아래쪽에서 스폰될 적
    /// </summary>
    [SerializeField] GameObject downEnemy;
    /// <summary>
    /// 아래 적이 생성될 위치
    /// </summary>
    [SerializeField] Transform downPos;
    /// <summary>
    /// 적이 생성될 수
    /// </summary>
    [SerializeField] int count;
    /// <summary>
    /// 적이 생성될 딜레이
    /// </summary>
    [SerializeField] float spawnDelay;

    /// <summary>
    /// 폭발이펙트
    /// </summary>
    [SerializeField] GameObject explosion;
    /// <summary>
    /// 폭발 위치
    /// </summary>
    [SerializeField] Transform[] explosionPos;
    /// <summary>
    /// 부숴질때 생성될 npc
    /// </summary>
    [SerializeField] GameObject supplyNPC;
    /// <summary>
    /// 부숴질때 생성된 npc의 생성위치
    /// </summary>
    [SerializeField] Transform supplyPos;

    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator anim;
    /// <summary>
    /// 플레이어를 따라다니는 카메라
    /// </summary>
    CameraPointReset playerCamera;
    PlayerHealth player;
    private void Awake()
    {
        anim = GetComponent<Animator>();        //애니메이터 받기
        playerCamera = GetComponentInChildren<CameraPointReset>();
        player=GameManager.Instance.PlayerHealth;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))             //플레이어가 가까이오면
        {
            trigger.enabled = false;                    //중복 실행 방지
            anim.SetTrigger("Player");                  //문열리기
        }
         
    }

    /// <summary>
    /// 적을 소환하는 코루틴 함수를 실행하는 함수 
    /// </summary>
    public void EnemySpawn()
    {

        StartCoroutine(EnemySpawnCoroutin());               //적 스폰하는 코루틴 실행
    }

    /// <summary>
    /// 적을 소환하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemySpawnCoroutin()
    {
        WaitUntil waitHPPlayer = new WaitUntil(() => player.HP > 0);
        yield return new WaitForSeconds(1.0f);                                              //기다리기
        for(int i = 0; i < count; i++)                                                      //지정된 수만큼 반복 적소환
        {
            yield return waitHPPlayer;
            Instantiate(upEnemy, upPos.position, Quaternion.identity);                      //적소환
            Instantiate(downEnemy, downPos.position, Quaternion.identity);                  //적소환
            yield return new WaitForSeconds(spawnDelay*2.0f);                                    //스폰 딜레이 기다리기
            
            if (i == count-1)                                                               //만약 적이 모두 생성되면
            {
                for (int j = 0; j < explosionPos.Length; j++)                               //폭발 위치 만큼
                {
                    Instantiate(explosion, explosionPos[j].position, Quaternion.identity);  //폭발 이펙트 생성
                }
                Instantiate(supplyNPC, supplyPos.position, Quaternion.identity);            //npc생성
                yield return waitHPPlayer;
                playerCamera.ResetCameraPos();
                gameObject.SetActive(false);                                                //게임오브젝트 비활성화
            }
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    /// <summary>
    /// 다음에 다시 생성할 적의 딜레이
    /// </summary>
    [SerializeField] float nextSpawnDelay;
    /// <summary>
    /// 소환할 적을 저장할 스크립트
    /// </summary>
    Vine_Morden morden;
    /// <summary>
    /// 해당 객체의 애니메이터
    /// </summary>
    Animator anim;
    /// <summary>
    /// 덩굴을 타고 가는것을 구현할 스프링조인트 2d
    /// </summary>
    SpringJoint2D springJoint;
    /// <summary>
    /// 현제 적이 스폰되어있는지 나타내는 변수 true면 적을 생성한 상태
    /// </summary>
    bool nowSpawn = false;
    
    private void Awake()
    {
        anim= GetComponent<Animator>();
        springJoint= GetComponent<SpringJoint2D>();
    }
    private void Start()
    {
        SpawnEnemyCheck();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&!nowSpawn)          //플레이어가 트리거에 닿으면
        {
            SpawnEnemy();                                       //적 생성
            morden.first = true;
            nowSpawn = true;                                    //중복실행 방지 변수 변경
        }
    }
    /// <summary>
    /// 소환할 적이 있는지 확인하는 함수
    /// </summary>
    void SpawnEnemyCheck()
    {
        morden=transform.GetChild(0).GetComponent<Vine_Morden>();       //자식 오브젝트에 스크립트가 있는지 확인 밑 저장
        if(morden != null )                                             //있으면
        {
            if (nowSpawn)                                               //적을 한 번이라도 소환한 상태면 적 소환
                SpawnEnemy();
        }
    }
    /// <summary>
    /// 적을 소환하는 함수
    /// </summary>
    void SpawnEnemy()
    {
        morden.gameObject.SetActive(true);                              //적 활성화
        Rigidbody2D rigid=morden.GetComponent<Rigidbody2D>();           //리지드 바디 받기
        springJoint.connectedBody = rigid;                              //스프링 조인트에 리지드 바디 연결
        anim.SetTrigger("Ride");                                        //애니메이션 재생
        StartCoroutine(Delay());                                        //다음 적을 대비해 딜레이 재생

    }
    /// <summary>
    /// 적 소환을 연속으로 할 딜레이 를 하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(nextSpawnDelay);
        SpawnEnemyCheck();          //적이 있는지 확인하는 변수 실행
    }

}

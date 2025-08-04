using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_3 : EnemyHP
{
    /// <summary>
    /// 생성할 미사일
    /// </summary>
    [SerializeField] GameObject missile;
    /// <summary>
    /// 미사일이 떨어질 위치 배열
    /// </summary>
    [SerializeField]
    Transform[] missilPos;
    /// <summary>
    /// 공격 이펙트
    /// </summary>
    [SerializeField] GameObject fireEffect;
    /// <summary>
    /// 발사 이펙트가 나갈 위치 배열
    /// </summary>
    [SerializeField] Transform[] fireEffectPos;
    
    /// <summary>
    /// 파괴후 같이 부숴질 다리 오브젝트
    /// </summary>
    [SerializeField] GameObject bridge;
    /// <summary>
    /// 부숴진 다리 오브젝트
    /// </summary>
    [SerializeField] GameObject brokenBridge;
    /// <summary>
    /// 폭발 이펙트
    /// </summary>
    [SerializeField] GameObject hugeExplosion;
    /// <summary>
    /// 소환할 적
    /// </summary>
    [SerializeField] GameObject morden;
    /// <summary>
    /// 다리가 부숴질때 생성될 파편
    /// </summary>
    [SerializeField] GameObject woodPartA;
    /// <summary>
    /// 다리가 부숴질때 상성될 파편
    /// </summary>
    [SerializeField] GameObject woodPartB;
    /// <summary>
    /// 폭발 위치
    /// </summary>
    [SerializeField] Transform explosionPos;
    /// <summary>
    /// 배의 움직임을 제어할 스크립트
    /// </summary>
    MovePlatform movePlatform;
    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator anim;
    [SerializeField] Ship ship;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        movePlatform=ship.GetComponent<MovePlatform>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            movePlatform.MoveStop();                                    //배 정지
            StartCoroutine(MordenSpawn());                              //적 소환
            ship.backGroundMove.StopAllCoroutines();
            //StartCoroutine(OnFireCouroutine(2.0f));
        }
    }

    protected override void OnDie()
    {
        gameObject.layer = 10;
        //특정 수 만큼 나무파편 소환
        for(int i = 0; i < 3; i++)
        {
            Instantiate(woodPartB,transform.position,Quaternion.identity);
            Instantiate(woodPartA,transform.position,Quaternion.identity);
        }
        
        bridge.SetActive(false);                                                    //다리 비활성화
        brokenBridge.SetActive(true);                                               //부숴진 다리 활성화
        Instantiate(hugeExplosion, explosionPos.position, Quaternion.identity);     //폭발 이펙트 생성
        movePlatform.PlatformStart(movePlatform.moveSpeed);                         //배가 다시 움직이게 하기
        ship.backGroundMove.OnMoveAuto();
        Destroy(gameObject);                                                        //오브젝트 삭제
    }
    /// <summary>
    /// 미사일을 생성하는 코루틴 함수
    /// </summary>
    /// <param name="Delay">발사 딜레이</param>
    /// <returns></returns>
    IEnumerator OnFireCouroutine(float Delay)
    {
        anim.SetTrigger("Fire");                                                    //애니메이션 재생
        StartCoroutine(FireEffect());                                               //애펙트 재생 함수 실행
        yield return new WaitForSeconds(Delay);                                     //발사 딜레이 기다리기
        for(int j =0;  j<3; j++)                                                    //발사 4번을 3번 반복
        {
            for (int i = 0; i < missilPos.Length; i++)                              //발사 위치를 랜덤으로 받아 발사 
            {
                int rand=Random.Range(0, missilPos.Length);
                Instantiate(missile, missilPos[rand].position,Quaternion.identity);     //미사일 생성
                yield return new WaitForSeconds(0.4f);
            }
        }
        yield return new WaitForSeconds(3.0f);                                      //다음 공격 딜레이
        StartCoroutine(MordenSpawn());                                              //적 소환 코루틴 시작
    }
    /// <summary>
    /// 발사 이펙트를 생성하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator FireEffect()
    {
        int sign = 0;                                                                       //범위를 벗어나지 않게 해줄 변수
        for (int i = 0;i < missilPos.Length*3;i++)                                            //미사일의 발사 만큼 반복
        {
            Instantiate(fireEffect, fireEffectPos[sign].position, Quaternion.identity);         //적소환
            sign++;                                                                            //변수 증가
            if (sign == 2)                                                                      //만약 변수가 이펙트 위치보다 많으면
                sign = 0;                                                                       //변수 변경
            yield return new WaitForSeconds(0.2f);                                              //딜레이
        }
    }
    /// <summary>
    /// 적을 소환하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator MordenSpawn()
    {
        for(int i = 0; i < 5; i++)
        {
            anim.SetTrigger("Morden");                                          //애니메이션 재생
            int rand = Random.Range(0, missilPos.Length);                       //랜덤값 받기
            yield return new WaitForSeconds(1.0f);                              //딜레이
            Instantiate(morden, missilPos[rand].position, Quaternion.identity); //랜덤위치에 적생성
        }
        yield return new WaitForSeconds(2.0f);                                  //다음 공격 딜레이
        StartCoroutine(OnFireCouroutine(2.0f));                                 //미사일 발사 코루틴 실행
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingFish : EnemyHP
{
    /// <summary>
    /// 이동속도에 곱할 애니메이션 커브
    /// </summary>
    [SerializeField] AnimationCurve curve;
    /// <summary>
    /// 움직임 속도
    /// </summary>
    [SerializeField] float moveSpeed;
    /// <summary>
    /// 처음 등장할 때 위로 튀어 오를 힘
    /// </summary>
    [SerializeField] float upForce;
    /// <summary>
    /// 등장할때 생길 이펙트
    /// </summary>
    [SerializeField] GameObject smallWaterEffect;
    /// <summary>
    /// 이펙트가 생길위치 배열
    /// </summary>
    [SerializeField]
    Transform[] effectPos;
    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator anim;
    /// <summary>
    /// 리지드 바디
    /// </summary>
    Rigidbody2D rigid;
    /// <summary>
    /// 애니메이션 커브 값을 저장핣 변수
    /// </summary>
    float addSpeed=0.0f;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(LifeTime());
        rigid.AddForce(new Vector2(0, upForce), ForceMode2D.Impulse);   //위로 튀어 오르게하기
        StartCoroutine(Move());                                         //움직임을 수행하는 코루틴 시작
        for (int i = 0; i < effectPos.Length; i++)
        {
            Instantiate(smallWaterEffect, effectPos[i].position, Quaternion.identity); //이펙트 생성하기
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth=collision.GetComponent<PlayerHealth>();   //플레이어에게 닿으면 공격
            playerHealth.HP = 1;
        }
    }
    /// <summary>
    /// 애니메이션 커브에 값을 전달 및 저장하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveAnimCurv()
    {
        float curvTime=0.0f;    //전달할 값 생성
        
        while (true)
        {
            yield return null;
            curvTime += Time.deltaTime;     //시간 저장
            addSpeed=curve.Evaluate(curvTime);  //애니메이션 커브값 저장
        }
    }
    /// <summary>
    /// 움직임을 수행하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Move()
    {
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            rigid.drag += 1;                            //위로 부드럽게 멈추기
        }
        StartCoroutine(MoveAnimCurv());     //애니메이션커브를 재생하는 코루틴 시작
        while (true)
        {
            yield return new WaitForFixedUpdate();
            transform.Translate(moveSpeed *addSpeed* Time.fixedDeltaTime, 0, 0);        //왼쪽으로 움직이기
        }
    }
    protected override void OnDie()
    {
        StopAllCoroutines();        //모든 코루틴 정지
        moveSpeed = 0.0f;           //이동속도 0
        gameObject.layer = 10;      //충돌 방지
        rigid.drag = 0;             //저항 제거
        rigid.gravityScale = 1.3f;  //밑으로 떨어지게 하기
        anim.SetTrigger("Dead");    //애니메이션 값 전달

    }
    protected override void Explosioned()
    {
        rigid.bodyType = RigidbodyType2D.Kinematic; //그자리 고정하기
        gameObject.layer = 10;                      //충돌 방지
        moveSpeed = 0.0f;                           //움직임 방지
        StopAllCoroutines();                        //모든 코루틴 정지
        anim.SetTrigger("DeadExplosion");           //애니메이션 값전달
        anim.SetTrigger("Dead");                    //애니메이션 값전달
        
    }
    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(10.0f);
        Destroy(gameObject);
 
    }
}

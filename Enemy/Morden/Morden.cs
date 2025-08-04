using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morden : EnemyBase
{
    [SerializeField] Transform waterPos;
    /// <summary>
    /// 물에 떨어질때 생성될 이펙트
    /// </summary>
    [SerializeField] GameObject waterEffect;
    /// <summary>
    /// 애니메이터
    /// </summary>
    protected Animator anim;
    /// <summary>
    /// 트랜스폼의 로컬 x값을 저장하는 변수
    /// </summary>
    float localX;
    /// <summary>
    /// 땅에 닿는지 확인하는 스크립트
    /// </summary>
    GroundSencer groundSencer;
    readonly int onGround_To_Hash = Animator.StringToHash("OnGround");
    readonly int trun_ToHash = Animator.StringToHash("Trun");
    readonly int attack_To_Hash = Animator.StringToHash("Attack");
    readonly int die_To_Hash = Animator.StringToHash("Die");
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        localX= transform.localScale.x;
        groundSencer=GetComponentInChildren<GroundSencer>();
    }
    private void Start()
    {
        secMoveSpeed = 0.0f;            //땅에 닿기 전까지는 이동정지
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")&& groundSencer.onGround)          //땅에 닿고 플레이어가 공격범위 안에 들어와 있으면 
        {
            OnAttack(0.3f);                     //공격함수 실행
        }
        else if (collision.CompareTag("Trun"))  //떨어질 곳에 닿으면
        {       
            TrunAnim();                         //뒤돌아가는 함수 실행
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))        //닿으면 죽는 공간에 닿으면
        {
            DeadZone();                                             //특정 죽음 함수 실행
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(localX*moveSpeed * secMoveSpeed * Time.deltaTime,0,0);
    }
    public override void Fly()
    {
        anim.SetBool(onGround_To_Hash, false);          //애니메이션 값전달
        secMoveSpeed = 0.0f;                            //이동정지
    }
    public override void Landing()
    {
        anim.SetBool(onGround_To_Hash, true);       //애니메이션 값 전달
        secMoveSpeed = 1.0f;                        //이동
        LookPlayer();                               //플레이어를 바라보는 함수 실행
    }

    /// <summary>
    /// 뒤를 돌아보는 함수
    /// </summary>
    protected override void TrunAnim()
    {
        anim.SetTrigger(trun_ToHash);           //애니메이션 값전달
        localX = transform.localScale.x*-1;     //변수 다시 저장
    }

    protected override void OnAttack(float Delay)
    {
        base.OnAttack(Delay);                               
        anim.SetTrigger(attack_To_Hash);                            //애니메이션 값전달
        float delay = anim.GetCurrentAnimatorStateInfo(0).length;       //애니메이션 시간 저장
        StopMove(delay+0.5f, 1);                                        //공격이 끝날때까지 정지
    }
    protected override void LookPlayer()
    {
        float distance = player.transform.position.x - transform.position.x;        //플레이어 위치와 해당 오브젝트의 위치 확인
        Vector3 local = transform.localScale;                                       //변경할 변수 생성
        if (distance < 0)           //플레이어가 왼쪽
        {
            if (local.x > 0)        //뒤를 돌아 봐야 한다면
            {
                TrunAnim();         //뒤돌아보기
            }
        }
        else
        {
            if (local.x < 0)
            {
                TrunAnim();
            }
        }
    }
    protected override void OnDie()
    {
        gameObject.layer = 10;          //사망레이어오 변경
        moveSpeed = 0;                  //이동정지
        attackRange = Vector2.zero;     //버그 대첵으로 공격범위 없애기
        if (groundSencer.onGround)      //땅에 닿은 상태면
        {   
            float distanc=player.transform.position.x - transform.position.x;   //플레이어와의 위치 확인
            if(distanc < 0)
            {
                anim.SetTrigger("DieToBack");               //플레이어가 뒤에 있다면 애니메이션 값 전달
            }
            
        }
        anim.SetTrigger(die_To_Hash);                  //애니메이션 값전달 
    }
    /// <summary>
    /// 객체가 특정 위치에 닿으면 죽음을 구현하는 함수
    /// </summary>
    protected virtual void DeadZone()
    {

        Instantiate(waterEffect,waterPos.position,Quaternion.identity); //수정된 위치에 잎펙트 생성
        Destroy(gameObject);                //오브젝트 삭제
    }
}

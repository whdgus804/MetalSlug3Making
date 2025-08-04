using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Item_Crap : ItemBase
{
    [SerializeField] float disableTime;
    /// <summary>
    /// 이동속도
    /// </summary>
    [SerializeField] float moveSpeed;
    /// <summary>
    /// 멈출 시간
    /// </summary>
    [SerializeField] float stopMoveTime;
    /// <summary>
    /// 최소 이동 시간
    /// </summary>
    [SerializeField] float minStopMoveTime;
    /// <summary>
    /// 옆으로 튈 힘
    /// </summary>
    [SerializeField] float sideForce;
    /// <summary>
    /// 위로 튈 힘
    /// </summary>
    [SerializeField] float upforce;
    /// <summary>
    /// 리지드바디
    /// </summary>
    Rigidbody2D rigid;
    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator anim;
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();            //애니메이터 받기
        rigid=GetComponent<Rigidbody2D>();          //리지드 바디 받기
    }

    private void OnEnable()
    {
        float randflot=Random.Range(-sideForce,sideForce);      //옆으로 튈힘을 랜덤값으로 받기
        float randUp=Random.Range(upforce*0.8f,upforce);        //위로 튈힘 랜덤값으로 받기
        rigid.AddForce(new Vector2(randflot,randUp));           //저장된 랜덤값으로 튀기
        StartCoroutine(StopMove());                             //움직임 멈춤 코루틴 실행
        DisableTime(disableTime);
    }
    private void FixedUpdate()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    /// <summary>
    /// 움직임을 멈춘 후 다시 이동할 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator StopMove()
    {
        float radnomFlot=Random.Range(minStopMoveTime, stopMoveTime);           //랜덤 값 받기
        float setmoveSpeed = moveSpeed;                                         //이동속도 저장
        moveSpeed = 0.0f;                                                       //이동 정지
        anim.SetFloat("Move", moveSpeed);                                       //애니메이터에 값 전달
        yield return new WaitForSeconds(1.0f);                                  //1초 기다리기
        moveSpeed = setmoveSpeed;                                               //원래 이동속도로 변경
        anim.SetFloat("Move", moveSpeed);                                       //애니메이터에 값 전달
        yield return new WaitForSeconds(radnomFlot);                            //랜덤 값 만큼 멈추기
        StartCoroutine(StopMove());                                             //다시 해당 코루틴 시작
    }
}

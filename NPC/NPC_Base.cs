using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Base : MonoBehaviour
{
    /// <summary>
    /// Npc가 떨어뜨릴 아이템
    /// </summary>
    [SerializeField]protected GameObject dropItem;
    ScoreManager scoreManager;
    /// <summary>
    /// 이동속도
    /// </summary>
    [SerializeField]protected float moveSpeed;

    /// <summary>
    /// 아이템을 떨어뜨릴 변수
    /// </summary>
    [SerializeField]protected Transform itemPos;

    /// <summary>
    /// npc가 현재 묶여있는지 나타내는 변수true면 묶여있는중
    /// </summary>
    [SerializeField]protected bool tide = false;

    /// <summary>
    /// NPC 애니메이터
    /// </summary>
    protected Animator anim;
    /// <summary>
    /// NPC의 리지디 바디
    /// </summary>
    protected Rigidbody2D rigid;

    private void Awake()
    {
        anim = GetComponent<Animator>();        //애니메이터 지정
        rigid = GetComponent<Rigidbody2D>();    //리지드바디 지정
        scoreManager=GameManager.Instance.ScoreManager;
    }

    protected virtual void OnEnable()
    {
        if (!tide)
        {
            StartCoroutine(Move());             //묶여 있지않은 방식의 NPC 이동
        }
        else
        {
            StartCoroutine(TideNPC());      //묶여있는 타입이면 풀려 날때까지 대기
        }
    }
    /// <summary>
    /// 플레이어의 공격을 맞을 경우 실행
    /// </summary>
    public virtual void Hit()
    {

    }
    /// <summary>
    /// 잠시 멈출때 실행 함수
    /// </summary>
    /// <param name="time">멈출 시간</param>
    protected void WaitMove(float time)
    {

        StopAllCoroutines();                //모든 코루틴 정지
        StartCoroutine(Wait(time));         //대기 코루틴에 받은 플롯값전달 
    }
    
    /// <summary>
    /// 움직임을 정지하는 코루틴 함수
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator Wait(float time)
    {
        float setmoveSpeed = moveSpeed;
        moveSpeed = 0.0f;
        Debug.Log(setmoveSpeed);
        yield return new WaitForSeconds(time);      //시간 기다리기
        moveSpeed = setmoveSpeed;
        if (tide)                                   //묶여있는 상태면
        {
            StartCoroutine(TideNPC());              //마저 묶여있기
        }
        else
        {
            StartCoroutine(Move());                //풀려있는 상태였으면 이동
        }

    }
    /// <summary>
    ///  움직이지 못할때 움직일 수 있는지 확인하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator TideNPC()
    {
        yield return new WaitUntil(()=> !tide);
        scoreManager.prisonerCount++;
        WaitMove(0.5f);
    }
    /// <summary>
    /// 움직일 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Move()
    {
        while (true)
        {
            transform.Translate(moveSpeed*transform.localScale.x*Time.fixedDeltaTime, 0, 0);
            yield return new WaitForFixedUpdate();
            //yield return null;
        }
    }
    /// <summary>
    /// 플레이어에게 아이템을 주는 함수
    /// </summary>
    protected virtual void DropItem()
    {

    }
    /// <summary>
    /// npc가 공중에서 떨어질때 실행함수
    /// </summary>
    public virtual void Fly()
    {
        if (!tide)
        {
            StopAllCoroutines();
        }
    }
    /// <summary>
    /// 땅에 착지할때 실행함수
    /// </summary>
    public virtual void Landing()
    {
        if (!tide)
        {
            StartCoroutine(Move());
        }
    }
}

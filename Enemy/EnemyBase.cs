using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : EnemyHP
{
    /// <summary>
    /// 적의 이동속도
    /// </summary>
    [SerializeField] protected float moveSpeed;

    /// <summary>
    /// 적의 근접공격 지점
    /// </summary>
    [SerializeField] protected Transform attackPos;
    /// <summary>
    /// 적의 근접 공격 범위
    /// </summary>
    [SerializeField] protected Vector2 attackRange;
    /// <summary>
    /// 적의 공격 데미지
    /// </summary>
    [SerializeField] int attackDamage;
    /// <summary>
    /// 객체가 공격가능한 레이어
    /// </summary>
    [SerializeField] protected LayerMask attackLayer;
    /// <summary>
    /// 플레이어 가져오기
    /// </summary>
    protected PlayerHealth player;

    /// <summary>
    /// 적의 이동의 정지, 감속처리를 할때 변경할 변수
    /// </summary>
    protected float secMoveSpeed=1.0f;
    protected virtual void Awake()
    {
        player = GameManager.Instance.PlayerHealth;
    }


    /// <summary>
    /// 적이 움직임을 멈출때 실행할 함수
    /// </summary>
    /// <param name="waitTime">기다릴 시간</param>
    /// <param name="setSpeed">기다린 후 이동할 방향 +혹은 - 부호로 1을 받는다</param>
    public void StopMove(float waitTime,float setSpeed)
    {
        StartCoroutine(MoveStopper(waitTime,setSpeed));        //전달바든 시간만큼 기다리는 코루틴 실행
    }
    /// <summary>
    /// 움직임을 정지할 코루틴 함수
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="setSpeed"></param>
    /// <returns></returns>
    IEnumerator MoveStopper(float waitTime,float setSpeed)
    {
        secMoveSpeed=0.0f;                                  //이동정지
        //StopMovedAnim();
        yield return new WaitForSeconds(waitTime);          //전달받은 값만큼 기다리기
        secMoveSpeed = setSpeed;                            //전달 받은 방향으로 이동하게끔 변수 변경
        //StartMoveAgain();

    }
    ///// <summary>
    ///// 정지애니메이션 실행을 위한 함수
    ///// </summary>
    //protected virtual void StopMovedAnim()
    //{

    //}
    ///// <summary>
    ///// 움직임을 멈춘 후 다시 움직임이 시작될때 실행되는 함수
    ///// </summary>
    //protected virtual void StartMoveAgain()
    //{

    //}
    /// <summary>
    /// 공격하기전 딜레이를 실행 하는 함수 
    /// </summary>
    /// <param name="Delay"></param>
    protected virtual void OnAttack(float Delay)
    {

        StartCoroutine(AttackDelay(Delay));           //시간을 기다릴 코루틴 힘수 실행
    }
    /// <summary>
    /// 일정 시간 뒤 공격을 실행할 코루틴 함수
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator AttackDelay(float waitTime)
    {
        //StopMove(waitTime, 1.0f);                        //공격할때는 이동정지
        //Debug.Log($"Attack_Delay:{waitTime}");              
        yield return new WaitForSeconds(waitTime);          //전달받은 값만큼 기다리기
        OnPlayer();                                         //플레이어 공격
        //Debug.Log("Attack");
    }
    /// <summary>
    /// 근접공격할 때 플레이어가 있는지 확인하는 함수 있으면 데미지를 준다
    /// </summary>
    /// <returns></returns>
     void OnPlayer()
     {
        Collider2D collider;                                                                 //충돌체 생성
        collider = Physics2D.OverlapBox(attackPos.position, attackRange, 0, attackLayer);  //플레이어가있는지 공격 위치 기준 범위 만큼 플레이어 찾기
        if (collider != null)                                                                //플레이어가 있으면
        {
            PlayerHealth player=collider.GetComponent<PlayerHealth>();          //모든 타입의 플레이어가 가지고 있는 HP스크립트 저장
            if(player==null)
                player=GameManager.Instance.PlayerHealth;
            player.HP = attackDamage;                                           //공격 데미지 만큼 데미지 주기
            //Debug.Log($"{collider.name}_attack({gameObject.name})");              //플레이어를 공격 
        }
        StartCoroutine(WaitPlayerRespawn());

     }
    IEnumerator WaitPlayerRespawn()
    {
        yield return new WaitUntil(() => player.HP > 0);
        LookPlayer();
    }
    /// <summary>
    /// 적이 플레이어를 바라보는 함수
    /// </summary>
    protected virtual void LookPlayer()
    {
        float sight = player.transform.position.x - transform.position.x;               //플레이어가 해당 객체 기준 왼쪽 혹은 오른쪽에 있는지 계산
        Vector2 vector2 = Vector2.one;                                                  //로컬스케일에 대입할 벡터값
        if(sight < 0)                                                                   //플레이어가 왼쪽에있으면
        {
            //플레이어가 해당 객체 기준 왼쪽
            vector2.x = -1;                                                             //백터값 변경
            
        }
        int vectorx = (int)vector2.x;
        int scaleX= (int)transform.localScale.x;
        if (vectorx!=scaleX)
        {
            TrunAnim();
        }
        StartCoroutine(WaitTrunAnim(vector2));
    }
    IEnumerator WaitTrunAnim(Vector2 vector)
    {
        yield return new WaitForSeconds(0.15f);
        transform.localScale = vector;                                                 //백터값을 로컬스케일에 대입
    }
    /// <summary>
    /// 다른 곳을 바라보다 플레이어를 바라볼때 실행되는 빈함수
    /// </summary>
    protected virtual void TrunAnim()
    {

    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        if(attackPos != null)
        {
            Gizmos.color = Color.yellow;
            Vector2 leftUp = new Vector2(attackPos.position.x - attackRange.x * 0.5f, attackPos.position.y + attackRange.y * 0.5f);      //값지정
            Vector2 rightUp = new Vector2(leftUp.x + attackRange.x, leftUp.y);
            Vector2 leftDown = new Vector2(leftUp.x, leftUp.y - attackRange.y);
            Vector2 rightDown = new Vector2(rightUp.x, rightUp.y - attackRange.y);

            Gizmos.DrawLine(leftUp, rightUp);                                                                                           //기즈모 그리기
            Gizmos.DrawLine(leftUp, leftDown);
            Gizmos.DrawLine(rightUp, rightDown);
            Gizmos.DrawLine(leftDown, rightDown);
        }
    }
#endif
}

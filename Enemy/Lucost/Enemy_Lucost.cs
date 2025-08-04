using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Lucost : EnemyBase
{
    /// <summary>
    /// x축의 애니메이션 커브
    /// </summary>
    [SerializeField] AnimationCurve xAnimCurv;
    /// <summary>
    /// y축의 애니메이션 커브
    /// </summary>
    [SerializeField] AnimationCurve yAnimCurv;
    /// <summary>
    /// x축의 이동속도
    /// </summary>
    [SerializeField] float xMoveSpeed;
    /// <summary>
    /// y축의 이동속도
    /// </summary>
    [SerializeField] float yMoveSpeed;
    /// <summary>
    /// 전체적인 이동속도
    /// </summary>
    [SerializeField] float flyMoveSpeed;
    /// <summary>
    /// 공격준비시작 딜레이 
    /// </summary>
    [SerializeField] float readyAttackTime;
    /// <summary>
    /// 공격 준비 딜레이
    /// </summary>
    [SerializeField] float beforeAttackDelay;
    /// <summary>
    /// 공격할때 플레이어위치로 이동하게될 변수
    /// </summary>
    [SerializeField] Transform attackTarget;
    /// <summary>
    /// 플레이어를 잡게될 위치
    /// </summary>
    [SerializeField] Transform hangPos;
    /// <summary>
    /// 공격 애니메이션이 끝난 뒤 플레이어를 떨어드릴 시간을 저장할 변수
    /// </summary>
    [SerializeField] float afterAttackDelay;
    /// <summary>
    /// 죽을 때 생성될 파츠1
    /// </summary>
    [Space(20.0f)]
    [SerializeField] GameObject wingA;
    /// <summary>
    /// 파츠2
    /// </summary>
    [SerializeField] GameObject wingB;
    /// <summary>
    /// 파츠3
    /// </summary>
    [SerializeField] GameObject leg;
    [SerializeField] GameObject parts;
    /// <summary>
    /// 적의 중심이 될 위치
    /// </summary>
    Transform center;
    /// <summary>
    /// 중심으로 이동하게하는 스크립트
    /// </summary>
    LucostCenter lucostCenter;

    Lucost_Animation anim;

    /// <summary>
    /// 애니메이션 커브에 넣을 변수
    /// </summary>
    float xcurv = 0.0f;
    /// <summary>
    /// 애니메이션 커브에 넣을 변수
    /// </summary>
    float ycurv = 0.0f;  
    /// <summary>
    /// 애니메이션 커브에 넣을 변수
    /// </summary>
    float curvTime = 0.0f;  
    protected override void Awake()
    {
        base.Awake();               
        center = transform.parent;                              //변수 초기화
        lucostCenter=GetComponentInParent<LucostCenter>();      //변수 초기화
        anim=GetComponent<Lucost_Animation>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Target"))                     //공격지점에 다다름을 나타냄
        {
            Attack();                                           //공격함수 실행
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(hp<1 && collision.gameObject.CompareTag("Ground"))
        {
            anim.OnGround();
        }
    }

    private void OnEnable()
    {
        StartFly();
    }
    


    /// <summary>
    /// 비행하는 함수
    /// </summary>
    void StartFly()
    {
        StopAllCoroutines();                    //모든 코루틴 정지
        StartCoroutine(FlyingCoroutin());       //비행코루틴 시작
        StartCoroutine(PatrolCro());            //일정 시간 후 공격 준비를 하게하는 코루틴 함수 실행
    }

    /// <summary>
    /// 지속적으로 중심 위치 기준으로 맴도는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator FlyingCoroutin()
    {
        while (true)
        {
            curvTime += flyMoveSpeed * Time.deltaTime;                                                                      //시간 받기
            xcurv = xAnimCurv.Evaluate(curvTime);                                                                           //애니메이션 커브에 따른 값 받기
            ycurv = yAnimCurv.Evaluate(curvTime);                                                                           //애니메이션 커브에 따른 값 받기
            transform.Translate(xcurv * xMoveSpeed * Time.fixedDeltaTime, ycurv * yMoveSpeed * Time.fixedDeltaTime, 0);     //위치 이동
            yield return new WaitForFixedUpdate();
        }
    }
    /// <summary>
    /// 일정 시간뒤 공격준비를 시작하게 하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator PatrolCro()
    {
        yield return new WaitForSeconds(readyAttackTime);
        AttackReady();
        //float patrolTime = 0.0f;
        //while (true)
        //{
        //    patrolTime += Time.deltaTime;
        //    if (patrolTime > readyAttackTime)
        //    {
        //        AttackReady();
        //    }
        //    yield return null;
        //}
    }
    /// <summary>
    /// 공격을 준비하는 코루틴 함수
    /// </summary>
    void AttackReady()
    {
        StopAllCoroutines();                                                //모든 코루틴 정지
        lucostCenter.Attacking(true);
        StartCoroutine(MoveToWards(center,moveSpeed*0.2f));                 //위치 중심으로 이동
        anim.AttackAnim();
        StartCoroutine(StartAttack());                                      //공격 시작하는 코루틴 함수 실행
    }
    /// <summary>
    /// 비행을 정지하고 목표 위치까지 나아가는 코루틴 함수
    /// </summary>
    /// <param name="target">목표위치</param>
    /// <param name="speed">속도</param>
    /// <returns></returns>
    IEnumerator MoveToWards(Transform target, float speed)
    {
        while (true)
        {
            transform.position=Vector2.MoveTowards(transform.position, target.position,speed*Time.fixedDeltaTime);      //목표위치에 지정된 속도로 이동
            yield return new FixedUpdate();
        }
    }
    /// <summary>
    /// 공격을 시작하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(beforeAttackDelay);                     //공격 준비 시간동안 대기
        AttackMove();                                                           //공격이동함수 실행
    }
    /// <summary>
    /// 공격을 위해 플레이어에게 다가가는 함수
    /// </summary>
    void AttackMove()
    {
        StopAllCoroutines();                                                    //모든 코루틴 정지
        lucostCenter.StopMoveCenter(100.0f);                                    //중심 이동 정지
        attackTarget.parent = null;                                             //목표위치 부모해제
        attackTarget.position = player.transform.position;                      //플레이어 위치에 목표위치 지정
        attackTarget.gameObject.SetActive(true);                                //목표위치 활성화
        StartCoroutine(MoveToWards(attackTarget, moveSpeed*0.7f));                    //목표위치로 이동하게 이동 코루틴에 변수 전달
    }
    /// <summary>
    /// 공격을 실행하는 함수
    /// </summary>
    void Attack()
    {
        StopAllCoroutines();                                                    //모든 코루틴 정지
        attackTarget.gameObject.SetActive(false);                               //목표 위치 비활성화
        Collider2D collider;                                                    //충돌체 생성
        collider = Physics2D.OverlapBox(attackPos.position, attackRange, 0, attackLayer);           //플레이어가있는지 공격 위치 기준 범위 만큼 플레이어 찾기
        if (collider != null)                                                                       //플레이어가 있으면
        {
            PlayerHealth player = collider.GetComponent<PlayerHealth>();                            //모든 타입의 플레이어가 가지고 있는 HP스크립트 저장
            if (player == null)
                player = GameManager.Instance.PlayerHealth;
            GroundSencer playergrounSencer=player.gameObject.GetComponentInChildren<GroundSencer>();    //플레이어를 공중에서 죽이기 때문에 변수를 받기 위해 스크립트 변수 생성
            playergrounSencer.onGround = false;                                                         //플레이어가 땅에서 떨어지게 하기
            PlayerAnimation anim=collider.GetComponent<PlayerAnimation>();
            anim.OnDie_Lucost();                                                                        //플레이어 물려죽는 애니메이션
            Vector3 vec3 = transform.localScale;
            vec3.x *= -1;
            player.transform.localScale = vec3;
            player.HP = 100;                                                                            //공격 데미지 만큼 데미지 주기   
            StartCoroutine(HangPlayer(collider.gameObject));                                            //플레이어를 매다는 코루틴 함수 실행
            StartCoroutine(MoveToWards(center, moveSpeed*0.5f));                                        //중심 지점으로 나아가게 끔 이동 함수에 변수 전달
            //Debug.Log($"{collider.name}_attack({gameObject.name})");              //플레이어를 공격 
        }
        else                                                                                            //플레이어가 없으면
        {
            lucostCenter.StopMoveCenter(0.0f);                                                          //중심으로 이동스크립트에 이동 다시 활성화
            //anim.Trun();
            StartCoroutine(MoveToWards(center, moveSpeed*0.5f));                                        //중심으로 이동
            StartCoroutine(WaitCoroutine(2.0f));                                                        //되돌아 갈때까지 기다리기
        }
    }
    /// <summary>
    /// 일정 시간을 기다리는 코루틴 함수
    /// </summary>
    /// <param name="time">기다릴 시간</param>
    /// <returns></returns>
    IEnumerator WaitCoroutine(float time)
    {
        yield return new WaitForSeconds(time);                          //시간 기다리기
        lucostCenter.Attacking(false);
        StartFly();                                                     //비행 함수 실행
    }
    /// <summary>
    /// 플레이어를 매달고 공중으로 끌고 올라가 죽이는 코루틴 함수
    /// </summary>
    /// <param name="player">플레이어</param>
    /// <returns></returns>
    IEnumerator HangPlayer(GameObject player)
    {
        anim.ExcutionAnim();
        Rigidbody2D playerrigid=player.GetComponent<Rigidbody2D>();         //플레이어의 리지드 바디 받기
        playerrigid.bodyType = RigidbodyType2D.Kinematic;                   //리지드 바디 타입을 키네마틱으로 변경 (않하면 바닥에 던질때 너무 빠르게 떨어짐)
        float time = 0.0f;                                                  //시간을 저장할 변수 생성
        while (true)
        {
            player.transform.position = hangPos.position;                   //플레이어 위치 고정
            time += Time.deltaTime;                                         //시간 저장
            if (time > afterAttackDelay)                                    //시간이 공격 후 딜레이보다 지났으면
            {
                playerrigid.bodyType= RigidbodyType2D.Dynamic;              //플레이어 떨구기
                StartFly();                                                 //비행시작
                lucostCenter.StopMoveCenter(0.0f);                          //중심이동 스크립트의 중심이동 식작
            }
            yield return new WaitForFixedUpdate();  
        }
    }

    /// <summary>
    /// 도망갈때 실행되는 함수
    /// </summary>
    /// <param name="run">참이면 도망중 거짓이면 도망끝</param>
    public void Runaway(bool run)
    {
        StopAllCoroutines();            //모든 코루틴 정지
        if (run)
        {
            StartCoroutine(FlyingCoroutin());           //비행 코루틴 실행
            StartCoroutine(WaitCoroutine(3.0f));        //이동 후 다시 공격 준비 시간 시작 및 비행 시작하게 위해 코루틴 실행

        }
        else
        {
            StartFly();                 //비행시작
        }

    }
    /// <summary>
    /// 몸을 돌리는 애니메이션
    /// </summary>
    public void LookPlayerStart()
    {
        anim.Trun();
    }

    protected override void OnDie()
    {
        StopAllCoroutines();
        lucostCenter.OnDead();
        gameObject.layer = 10;
        anim.DieAnim();
        Rigidbody2D rigid=GetComponent<Rigidbody2D>();
        rigid.bodyType = RigidbodyType2D.Dynamic;
        Instantiate(wingA, transform.position, Quaternion.identity);        //죽을때 파편 생성
        Instantiate(wingB, transform.position, Quaternion.identity);        //죽을때 파편 생성
        Instantiate(leg, transform.position, Quaternion.identity);        //죽을때 파편 생성
        Instantiate(leg, transform.position, Quaternion.identity);        //죽을때 파편 생성
        Instantiate(parts, transform.position, Quaternion.identity);        //죽을때 파편 생성
    }
}

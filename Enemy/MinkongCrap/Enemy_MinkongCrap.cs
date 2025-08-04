using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MinkongCrap : EnemyBase
{

    

    /// <summary>
    /// 밍콩게의 처음 있을 타입
    /// </summary>
    public enum IdleType
    {
        Combat,     //전투 전투를 제외한 다른 타입은 공격 받으면 해당 타입으로 변경 
        Eatting,    //생선 먹는 중
        Idle        //가만히 서있는 중 
    }
    /// <summary>
    /// 밍콩게의 처음 있을 타입
    /// </summary>
    public IdleType idleType;
    /// <summary>
    /// 밍콩게의 공격 타입 근접, 원거리
    /// </summary>
    public enum AttackType
    {
        Random,
        Close,      //근접
        Long        //원거리
    }
    /// <summary>
    /// 적의 총알
    /// </summary>
    [SerializeField] GameObject bullet;
    /// <summary>
    /// 밍콩게의 공격 타입 근접, 원거리
    /// </summary>
    public AttackType attackType;
    /// <summary>
    /// 근접공겨 선딜레이
    /// </summary>
    [SerializeField] float attackDelay;
    /// <summary>
    /// 근점공격을 시작하는 거리
    /// </summary>
    [SerializeField] float attackSencerRange;
    /// <summary>
    /// 원거리 공격 범위
    /// </summary>
    [SerializeField] float shootSencerRange;

    /// <summary>
    /// 공격 후 앞뒤로 돌아다닐때 한 번 이돌할때 이동할 최대시간
    /// </summary>
    [Space(20.0f)]
    [SerializeField] float patrolTime;
    /// <summary>
    /// 땅에 닿는지 확인하는 스크립트
    /// </summary>
    GroundSencer groundSencer;
    /// <summary>
    /// 원거리타입의 발사가능을 나타내는 bool변수 true면 발사가능
    /// </summary>
    bool readyToShoot = true;


    Vector2 moveValue = Vector2.zero;

    /// <summary>
    /// 밍콩게의 애니메이션스크립트
    /// </summary>
    MinkongCrap_Animation anim;

    /// <summary>
    /// 이동 관련 변수들을 곱할 벡터(moveValue * moveSpeed * secMoveSpeed)
    /// </summary>
    Vector2 moveValueAdd = Vector2.zero;

    /// <summary>
    /// 머리 파츠
    /// </summary>
    [Space(20.0f)]
    [SerializeField] GameObject parts_Head;
    /// <summary>
    /// 집게 파츠
    /// </summary>
    [SerializeField] GameObject parts_Hand;
    /// <summary>
    /// 다리 파츠
    /// </summary>
    [SerializeField] GameObject parts_Leg;

    /// <summary>
    /// 작은 피 이펙트
    /// </summary>
    [SerializeField] GameObject smallBlood;
    /// <summary>
    /// 큰 피 이펙트
    /// </summary>
    [SerializeField] GameObject bigBlood;

    [SerializeField] GameObject head;
    /// <summary>
    /// 다리파츠가 생성될 위치 배열
    /// </summary>
    Transform[] legPos;
    /// <summary>
    /// 집게파츠가 생성될 위치 배열 
    /// </summary>
    Transform[] handPos;
    /// <summary>
    /// 머리 파츠가 생성될 위치 
    /// </summary>
    Transform headPos;

    
    protected override void Awake()
    {
        base.Awake();
        anim=GetComponent<MinkongCrap_Animation>();         //애니메이션 받기
        legPos= new Transform[3];
        handPos= new Transform[2];
        Transform trans = transform.GetChild(4);
        for(int i = 0; i < legPos.Length; i++)
        {
            legPos[i] = trans.GetChild(i);
        }
        for(int i = 0;i < handPos.Length; i++)
        {
            handPos[i] = trans.GetChild(i+4);
        }
        headPos = trans.GetChild(3);

    }
    private void Start()
    {
        if (attackType == AttackType.Random)        //랜덤이면
        {
            int rand = Random.Range(0, 2);          //0에서1랜덤 저장
            if(rand == 0)                           //0이면
            {
                attackType = AttackType.Close;      //근접
            }
            else                                    //1이면
            {
                attackType=AttackType.Long;         //원거리
            }
        }
        LookPlayer();                               //플레이어 바라보기
        moveValue.x = transform.localScale.x;
    }
    private void OnEnable()
    {
        
        //LookPlayer();
        gameObject.layer = 6;
        //boxCollider.offset = attackPos.position + transform.position;
        //boxCollider.size = attackRange;
        switch (idleType)                           //밍콩게의 타입에 따른 행동
        {
            case IdleType.Combat:                   //전투면 
                //StopMove(0.0f, 1.0f);
                if (attackType == AttackType.Long) //원거리 타입이면 
                {
                    StartCoroutine(PlayerDistance());           //플레이어와의 거리를 계산하는 코루틴 실행
                }

                //anim.MoveAnim(1.0f);
                break;
            case IdleType.Eatting:                  //먹고있는 상태면 
                StopMove(0.0f, 0.0f);                 //가만히 서있기
                anim.Eat(true);
                //애니메이션
                break;
            case IdleType.Idle:                     //가만히 있는 상태면 
                StopMove(0.0f, 0.0f);
                break;
        }
        if (attackType == AttackType.Long)
            patrolTime *= 0.5f;
        

    }

    private void FixedUpdate()
    {
        moveValueAdd = moveValue * moveSpeed * secMoveSpeed;
        transform.Translate(moveValueAdd * Time.deltaTime);
        anim.MoveAnim(moveValueAdd.x);                          //애니메이터에 값전달
        

    }

    public void OnAttackTrigger()
    {
        if (idleType == IdleType.Combat)
            OnAttack(attackDelay);              //근접공격함수 실행
    }

    public override void Fly()
    {
        anim.Fly();             //애니메이션 재생
        secMoveSpeed = 0.0f;    //이동정지
        StopAllCoroutines();    //모든 코루틴 정지
    }
    public override void Landing()
    {
        anim.Landing();
        if(idleType== IdleType.Combat && hp>0)
        {
            LookPlayer();
            moveValue.x = transform.localScale.x;
            secMoveSpeed = 1.0f;
        }
        //StartCoroutine(PatrolCoroutin(1.0f));
    }
    /// <summary>
    /// 플레이어와의 거리를 측정하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerDistance()
    {
        float distance = 0.0f;          //플레이어와의 거리를 계산할 변수 생성
        while (true)
        {
            distance = (player.transform.position - transform.position).sqrMagnitude;         //플레이어와 거리측정

            if (distance < 3 * 3)                   //너무가까우면 
            {
                RunAway();                          //뒤로 도망가는 함수 실행
            }
            else if(distance<5*5)                   //원거리 공격 범위 안이면
            {
                if (readyToShoot)                   //발사가능한 상태일때
                {
                    ShootBefor();                   //발사 함수 실행
                }
            }
            yield return null;
            //    if(attackType == AttackType.Long && !attacking)                                                  //만약 원거리 타입이면
            //    {
            //        if (distance < shootSencerRange * shootSencerRange+shootSencerRange)            //플레이어가 공격 사거리에 공격 사거리를 더한 값만큼 가까우면
            //        {
            //            if (readyToShoot)                                                           //공격 가능상태면
            //            {
            //                if (distance < shootSencerRange * shootSencerRange)                     //플레이어가 공격 사거리보다 가까우면 
            //                {
            //                    Debug.Log("Run");
            //                    RunAway();                                                          //객체 도망
            //                }
            //                else                                                                    //공격 사거리를 더한 값 이랑 공격 사거리 사이에 있으면
            //                {
            //                    //발사 
            //                    ShootBefor();

            //                    //Shoot();                                                            //공격
            //                }
            //            }
            //            else                                                                        //공격 가능한 상태가 아니면
            //            {
            //                Debug.Log("Run");
            //                RunAway();                                                              //무조건 도망
            //            }
            //        }
            //    }
            //    //if (distance < attackSencerRange * attackSencerRange)
            //    //{
            //    //    attacking = true;
            //    //    OnAttack(attackDelay);
            //    //}
            //    yield return null;
            //}
       }
        //float range = attackPos.position.x + attackRange.x * 0.5f*transform.localScale.x;
        //Debug.Log(range);
        //if (player == null)
        //{
        //    Debug.Log("Null"); 
        //    player=GameManager.Instance.Player;
        //}
    }
    protected override void OnAttack(float Delay)
    {
        StopAllCoroutines();                        //코루틴 정지
        if (player.HP > 0)
        {
            LookPlayer();                               //플레이어 바라보기
            StopMove(Delay+1.0f, 1.0f);                 //이동정지
            if(hp > 0)
            {
                base.OnAttack(Delay);                       //플레이어 공격실행
                anim.AttackAnim();
                //Debug.Log(moveValueAdd.x);
                StartCoroutine(PatrolCoroutin(Delay));
            }
        }
        else
        {
            StartCoroutine(PatrolCoroutin(Delay));
        }

    }

    /// <summary>
    /// 총알을 발사하기 전 다른 코루틴을 멈추는 함수
    /// </summary>
    void ShootBefor()
    {
        if (HP > 1)
        {

        StopAllCoroutines();                    //모든 코루틴 정지 
        StartCoroutine(Shooting());             //발사시작
        }
    }
    /// <summary>
    /// 원거리 공격할때 선딜레 후 총알을 생성하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Shooting()
    {
        anim.Shoot();                                           //애니메이션 재생
        StopMove(1.5f, 1);                                      //이동 정지
        readyToShoot = false;                                   //발사 불가로 변수 변경
        yield return new WaitForSeconds(0.5f);                  //선딜레이
        GameObject obj = bullet;                                //총알과 같은 오브젝트 생성
        obj.transform.localScale = transform.localScale;        //총알의 로컬스케일값변경 (해당 스케일값에 따라 총알이 나가는 방향이 다름)
        Instantiate(obj,attackPos.position,Quaternion.identity);//총알 생성
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(PatrolCoroutin(0.0f));
    }

    /// <summary>
    /// 원거리 공격을 하기 위해 플레이어로부터 멀어지는 함수
    /// </summary>
    void RunAway()
    {
        StopAllCoroutines();                //모든 코루틴 정지
        
        LookPlayer();                               //플레이어를 바라보기
        moveValue.x =transform.localScale.x;        //이동 방향 초기화                          
        moveValue *= -1.0f;                          //플레이어방향으로 부터 멀어지게끔 변수 조정
        //secMoveSpeed = 1.0f;                        //적 이동
                                                    //Debug.Log($"{moveValue.x}X{secMoveSpeed}X{moveSpeed}={moveValueAdd.x}");
                                                    //StopMove(0.0f, 1.0f);

        StartCoroutine(RunCouroutin());     //코루틴 실행 
    }
    /// <summary>
    /// 플레이어로부터 멀어지는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator RunCouroutin()
    {
        //secMoveSpeed = 1.0f;
        
        yield return new WaitForSeconds(1.0f);
        LookPlayer();
        moveValue.x=transform.localScale.x;         //적이 앞으로 이동하게 끔 변환
        
        //secMoveSpeed = 1.0f;
        //StartCoroutine(PatrolCoroutin(1.0f));           //플레이어와의 거리 계산
        //StartCoroutine(PlayerDistance());
        StartCoroutine(PatrolCoroutin(1.0f));
    }


    /// <summary>
    /// 플레이어를 공격하고 잠깐동안 공격을 중지하고 왔다갔다를 반복하는 코루틴함수 이하 정찰함수 호명
    /// 하지만 플레이어가 근접 공격범위 안으로 들어오면 바로 다시 공격
    /// </summary>
    IEnumerator PatrolCoroutin(float attackAfterDelay)
    {
        yield return new WaitForSeconds(attackAfterDelay);                                      //공격 후딜레이까지 기다리기
        LookPlayer();
        
        int randInt = Random.Range(3, 6);                                                      //정찰할 횟수
        if (attackType == AttackType.Long)                                                      //원거리 타입이면 변수 조정
        {
            randInt = 3;
        }

        for (int i = 0; i < randInt; i++)                                                        //정찰반복
        {
            LookPlayer();                                                                       //매 정찰마다 플레이어 바라보기
            int moveArrow = Random.Range(-1, 2);                                                // 앞, 제자리, 뒤 중 랜덤 선택
            if (attackType == AttackType.Long)                                                  //원거리 타입이면
            {
                moveArrow = Random.Range(-1, 1);                                                //플레이어 방향으로 가지않게 조정
                moveArrow *= (int)transform.localScale.x;                                       //플레이어 방향으로 가지않게 조정
            }

            moveValue.x = moveArrow;                                                            //앞, 제자리, 뒤로 이동
            float randfloat = Random.Range(0.3f, patrolTime);                                   //한 번의 정할할 시간
            anim.MoveAnim(moveValueAdd.x);                                                      //애니메이션에 값전달
            yield return new WaitForSeconds(randfloat);
        }
        LookPlayer();                                                                           //플레이어 바라보기
        readyToShoot = true;                                                                    //원거리 공격이 발사가능하도록 변수 조정
        
        moveValue.x = transform.localScale.x;                                                   //정찰이 끝났으니 플레이어 쪽으로 걸어가기
        if(attackType == AttackType.Long)
        {
            StartCoroutine(PlayerDistance());
        }
    }
    protected override void TrunAnim()
    {
        anim.Trun();
    }
    protected override void OnHit()
    {
        if(idleType != IdleType.Combat)                                 //만약 공격을 당했는데 비전투 상태면
        {
            idleType=IdleType.Combat;                                   // 공격타입으로 변경
            StopMove(1.0f, 1);                                          //잠깐 정지
            moveValue.x = transform.localScale.x;                                         //움직임 초기화
            OnEnable();                                                 //다시 홣성화 함수 실행
            anim.Eat(false);
        }
        smallBlood.SetActive(true);
    }

    protected override void OnDie()
    {
        StopAllCoroutines();
        gameObject.layer = 10;
        head.layer = 10;
        //moveValueAdd = Vector3.zero;
        secMoveSpeed = 0.0f;
        anim.DieAnim();
    }
    protected override void Explosioned()
    {
        
        Instantiate(parts_Head,headPos.position,Quaternion.identity);       //머리파츠 생성
        //손을 랜덤방향으로 갯수만큼 생성
        for(int i=0;i<handPos.Length;i++)                                   //팔의 개수 만큼 반복
        {
            GameObject obj = parts_Hand;                                    //게임오브젝트를 생성 후 핸드 오브젝트로 저장
            int rand = Random.Range(-1, 2);                                 //랜덤값 받기
            if (rand == 0)                                                  //로컬스케일 x가 0이면 않보이기에 0이면 1지정
                rand = 1;
            obj.transform.localScale = new Vector3(rand, 1, 1);             //변수 전달 
            Instantiate(obj, handPos[i].position, Quaternion.identity);     //생성

        }
        //다리 개수만큼 랜덤 방향으로 생성 
        for(int i=0; i < legPos.Length; i++)
        {
            GameObject leg = parts_Leg;                                     //다리오브젝트로 새 오브젝트를 저장
            int legrand = Random.Range(-1, 2);                              //랜덤값 받기
            if (legrand == 0)
                legrand = 1;
            leg.transform.localScale = new Vector3(legrand, 1, 1);          //랜덤값 전달
            Instantiate(leg, legPos[i].position , Quaternion.identity);     //생성
        }

        //GameObject blood = bigBlood;
        
        //Instantiate(blood, transform.position,Quaternion .identity);
        //blood.SetActive(true);
        bigBlood.transform.parent = null;
        bigBlood.SetActive(true);
        gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Vector2 shootRange = new Vector2(transform.position.x + shootSencerRange*transform.localScale.x, 0);
        //Gizmos.DrawIcon(shootRange, "_");
        Gizmos.DrawLine(transform.position, shootRange);
    }
#endif
}

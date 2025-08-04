using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : EnemyHP
{
    //뒤로 갔다가 정지후 앞 돌진
    //불구덩이 발사 두 번 발사하지만 하나는 보스쪽으로 하나는 플레이어 쪽으로 떨어진다
    //플레이어쪽으로 캐논 발사
    /// <summary>
    /// 돌진속도
    /// </summary>
    [SerializeField] float rushSpeed;
    /// <summary>
    /// 뒤로 빠지는 속도
    /// </summary>
    [SerializeField] float backSpeed;
    /// <summary>
    /// 돌진 쿨타임
    /// </summary>
    [SerializeField] float rushCoolTime;
    /// <summary>
    /// 대포 탄환
    /// </summary>
    [SerializeField] GameObject cannon;
    /// <summary>
    /// 캐논이 발사될 위치
    /// </summary>
    [SerializeField] Transform cannonPos;
    /// <summary>
    /// 불 덩이
    /// </summary>
    [SerializeField] GameObject fireBullet;
    /// <summary>
    /// 불덩이 왼쪽버전
    /// </summary>
    [SerializeField] GameObject fireBullet_Left;
    [SerializeField] Transform fireBullet_FirePos;
    [SerializeField] Transform fireBullet_Left_FirePos;
    [SerializeField] GameObject fireBallFireEffect;

    /// <summary>
    /// 캐논 발사 이펙트
    /// </summary>
    [SerializeField] GameObject cannonFireEffect;
    /// <summary>
    /// 캐논 전개 이펙트
    /// </summary>
    [SerializeField] GameObject deployingCannonEffect;

    /// <summary>
    /// 플레이어 위치를 알릴 트랜스폼
    /// </summary>
    [SerializeField] Transform target;

    [SerializeField] GameObject[] waterEffect;



    /// <summary>
    /// 한 번 반파된 후 지속적으로 소환될 불꽃
    /// </summary>
    [SerializeField] Transform[] rocketTail;

    [SerializeField] float fallSpeed;
    /// <summary>
    /// 혀내 이동속도
    /// </summary>
    float moveSpeed = 0.0f;
    /// <summary>
    /// 돌진준비를 나타내는 변수 true면 다음 공격 돌진
    /// </summary>
    bool readyRush = false;
    /// <summary>
    /// 일정 체력이 소비되면 true되는 변수
    /// </summary>
    bool phaseTwo = false;
    Animator anim;
    PlayerHealth player;

    BossStageManager stageManager;
    /// <summary>
    /// 현재 돌진중인지 혹은 돌진 준비중인지 나타내는 변수 true면 돌진중
    /// </summary>
    bool rushing = false;

    Vector2 moveVec = new Vector2(1, 0);
    private void Awake()
    {
        player = GameManager.Instance.PlayerHealth;
        anim = GetComponent<Animator>();
        stageManager = FindAnyObjectByType<BossStageManager>();
    }
    private void OnEnable()
    {
        target.parent = null;
        StartCoroutine(ShowStart());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stop"))
        {
            collision.gameObject.SetActive(false);
            moveSpeed = 0.0f;
            StopAllCoroutines();
            StartCoroutine(RushCoolDown());
            OnAttack();
        }
    }
    IEnumerator ShowStart()
    {
        yield return new WaitForSeconds(3.0f);
        moveSpeed = rushSpeed;

    }
    private void FixedUpdate()
    {
        transform.Translate(moveVec*moveSpeed*Time.deltaTime);
    }
    /// <summary>
    /// 공격을 실행하는 함수
    /// </summary>
    void OnAttack()
    {

        if (player.HP > 0)                                  //플레이어가 살아있으면
        {
            if (readyRush)                                  //돌진가능 이면
            {
                StartCoroutine(RushCoroutine());            //돌진코루틴 시작
            }
            else
            {
                //돌진이 쿨타임에 들어간 상태면
                StartCoroutine(OnFireCorountine(0));        //총알발사
            }
        }
        else
        {
            StartCoroutine(WaitPlayerRespawn());        //플레이어가 부활할때까지 기다리기
        }
    }
    /// <summary>
    /// 플레이어가 부활할 때가지 기다리는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitPlayerRespawn()
    {
        yield return new WaitUntil(() => player.HP > 0);  //플레이어 부활 기다라기
        OnAttack(); //공격함수 실행
    }
    /// <summary>
    /// 뒤로 가고 앞으로 돌진하는 코루틴함수
    /// </summary>
    /// <returns></returns>
    IEnumerator RushCoroutine()
    {
        rushing = true;                     //돌진중 변수 변경
        readyRush = false;                  //돌진 변수 변경
        moveSpeed = backSpeed;              //뒤로이동
        //애니메이션 감속
        yield return new WaitForSeconds(2.0f);  //뒤로 갈 시간

        moveSpeed = 0.0f;           //정지
        yield return new WaitForSeconds(2.0f);
        anim.SetBool("Rush", true);     //애니메이션 값전달
        moveSpeed = rushSpeed;          //이동속도 변경
        //애니메이션 배속
        yield return new WaitForSeconds(2.8f);  //돌진할 시간
        anim.SetBool("Rush", false);        //애니메이션값전달
        moveSpeed = backSpeed;              //뒤로 동
        yield return new WaitForSeconds(0.8f);  //다시 자리로 돌아갈 시간
        moveSpeed = 0.0f;       //이동정지
        rushing = false;         //돌진 변수 변경
        StartCoroutine(RushCoolDown());
        OnAttack();
    }
    /// <summary>
    /// 돌진 쿨타임 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator RushCoolDown()
    {
        yield return new WaitForSeconds(rushCoolTime);
        readyRush = true;
    }

    /// <summary>
    /// 캐논 및 불을 발사하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator OnFireCorountine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);      //기다리기
        anim.SetTrigger("Fire");                        //발사 애니메이션 값전달
        target.position = player.transform.position; //플레이어 위치로 이동
        if (phaseTwo)       //만약 페이즈 2장이 면
        {
            //캐논
            Instantiate(cannon, cannonPos.position, Quaternion.identity);   //대포탄 발사
            cannonFireEffect.SetActive(true);                               //대포 이펙트
        }
        else //1페이즈이면
        {
            //불
            fireBallFireEffect.SetActive(true);         //불덩이 이펙트
            Instantiate(fireBullet, fireBullet_FirePos.position, Quaternion.identity);    //불덩이 생상
            yield return new WaitForSeconds(0.2f);      //기다리기
            Instantiate(fireBullet_Left, fireBullet_Left_FirePos.position, Quaternion.identity);  //다음 불덩이 발사
        }
        yield return new WaitForSeconds(3.0f); //공격 쿨타임
        OnAttack();             //공격 함수 실행
    }
    protected override void OnDie()
    {
        if (!phaseTwo)      //만약 페이즈 1이면
        {
            if (rushing)     //돌진중이면
            {
                StartCoroutine(WaitRushEnd()); //돌진이 끝날때까지 기다리는 코루틴 함수 실행
            }
            else //돌진중이 아니면
            {

                StopAllCoroutines();                //모든 코루틴 정지
                StartCoroutine(RocketTail());       //몸에서 터지는 이펙트를 관리하는 코루틴 실행
                phaseTwo = true;                    //변수 변경
                hp = 20;                             //체력값 변경
                anim.SetTrigger("Cannon");          //애니메이션 값전달
                deployingCannonEffect.SetActive(true);  //대포전개 이펙트
                StartCoroutine(OnFireCorountine(3.0f)); //발사 코루틴 실행 
                StartCoroutine(RushCoolDown());     //돌진 쿨타임 마저 실행
            }
        }
        else
        {
            //무력화 애니메이션
            //주기적으로 작은 폭팔
            StopAllCoroutines();            //모든 코루틴 정지
            StartCoroutine(WaterEffect());
            anim.SetTrigger("Defeat");      //애니메이션에 값전달
            //moveSpeed = 0.0f;               //이동정지
            stageManager.GameOver();        //스테이지 매니저에 함수 실행
        }
    }
    /// <summary>
    /// 몸이 부숴저 불꽃이펙트를 관리하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator RocketTail()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);     //기다릴 시간 생성
        for (int i = 0; i < rocketTail.Length; i++)
        {
            int rand = Random.Range(0, rocketTail.Length);      //랜덤값받기
            rocketTail[rand].gameObject.SetActive(true);        //받은 랜덤값을 기반으로 불꽅 활성화
            yield return wait;                                  //기다리기
        }
        StartCoroutine(RocketTail());
    }
    /// <summary>
    /// 돌진 이 끝날때까지 기다리는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitRushEnd()
    {
        yield return new WaitUntil(() => !rushing);     //기다리기
        OnDie();                               //함수 마저 실행

    }
    /// <summary>
    /// 물 이펙트를 활성화 하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator WaterEffect()
    {
        moveVec = Vector2.zero;         //이동정지
        for(int i= 0; i < waterEffect.Length; i++)      //모든 이펙트를 켜기
        {
            waterEffect[i].transform.parent = null;     //같이 밑으로 가는 것을 방지하기위해 부모 제거
            waterEffect[i].SetActive(true);             //이펙트(게임 오브젝트) 활성화
        }
        yield return new WaitForSeconds(2.0f);          //시간 기다리기
        moveVec = new Vector2(0, -1);                   //밑으로 가라앉기
        moveVec *= fallSpeed;                           //속도 추가
        for(int i=0; i < 100; i++)                      //이펙트 랜덤으로 계속 활성화 하기
        {
            yield return new WaitForSeconds(0.1f);      //기다리기
            int randint=Random.Range(0, waterEffect.Length);        //랜덤값 받기
            waterEffect[randint].SetActive(true);                   //해당 값으로 오브젝트 활성화
        }
    }

}

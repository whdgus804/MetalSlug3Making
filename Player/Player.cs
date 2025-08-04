using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.InputSystem;

public class Player : PlayerHealth
{
    /// <summary>
    /// 플레이어의 이동속도
    /// </summary>
    [SerializeField] float moveSpeed;

    /// <summary>
    /// 플레이어가 점프하면 적용할 힘의 수치
    /// </summary>
    [SerializeField] float jumpForce;


    /// <summary>
    /// 0-정면
    /// 1-위
    /// 2-앉기
    /// 3-밑
    /// </summary>
    int sign = 0;
    /// <summary>
    /// 플레이어의 감속, 정지할때 해당 수치를 받을 변수
    /// </summary>
    float secMoveSpeed=1.0f;

    /// <summary>
    /// 플레이어가 위를 바라보고 있는지 나타내는 bool 변수 true면 위를 보고 있다
    /// </summary>
    bool isLookup = false;

    /// <summary>
    /// 플레이어가 바닥을 바라보거나 앉는지를 나타내는 bool 변수 true면 앉거나 밑을 보는중
    /// </summary>
    bool isDown = false;


    /// <summary>
    /// 플레이어의 리지드 바디
    /// </summary>
    Rigidbody2D rigid;

    /// <summary>
    /// 플레이어 이동 값을 저장할 vector2 변수
    /// </summary>
    Vector2 readValue = Vector2.zero;
    /// <summary>
    /// 기본 플레이어의 인풋액션
    /// </summary>
    BasicPlayerInput input;
    ///// <summary>
    ///// 점프가능한지 나타내는 스크립트
    ///// </summary>
    //GroundSencer groundSencer;

    PlayerCameraMove playerCameraMove;
    /// <summary>
    /// 플레이어 공격 스크립트
    /// </summary>
    Weapon_Attack playerWeapon;

    PlayerAnimation anim;

    float sitmoveSpeed = 1.0f;

    
    
    protected override void Awake()
    {
        base.Awake();   
        input=new BasicPlayerInput();                           //인풋액션 지정
        rigid= GetComponent<Rigidbody2D>();                     //플레이어의 리지드바디 지정
        //groundSencer=GetComponentInChildren<GroundSencer>();    //점프가능확인 스크립트 지정
        playerCameraMove=FindAnyObjectByType<PlayerCameraMove>();   
        playerWeapon=GetComponent<Weapon_Attack>();             //플레이어 공격 스크립트 지정
        anim=GetComponent<PlayerAnimation>();

    }
    private void OnEnable()
    {
        InputEnable();
    }



    private void FixedUpdate()
    {
        transform.Translate(readValue*moveSpeed*secMoveSpeed*sitmoveSpeed*Time.deltaTime);        //플레이어 이동
    }

    private void OnDisable()
    {
        InputDisable();                                         //오브젝트가 비활성화 되면 인풋액션 비활성화 
    }
    /// <summary>
    /// 인풋액션을 활성화하는 함수
    /// </summary>
    public void InputEnable()
    {
        input.Enable();
        //input.Move.move.performed += OnMove;        //이동
        //input.Move.move.canceled += OnMove;         //이동 취소

        MoveInputEnble(true);
        input.Jump.jump.performed += OnJump;        //점프

        input.Look.lookUp.performed += OnLookUp;    //위 보는 중
        input.Look.lookUp.canceled += OnLookFront;     //다시 앞보는중

        input.Attack.attack.performed += OnFire;

        input.Look.Down.performed += OnDown;        //밑을 보는중
        input.Look.Down.canceled += OnDown;         //다시 앞을 보는중 

        input.Throw.throwed.performed += OnGrenade;
    }


    /// <summary>
    /// 인풋액션을 끄는 함수
    /// </summary>
    public void InputDisable()
    {
        input.Throw.throwed.performed -= OnGrenade;

        input.Look.Down.canceled -= OnDown;         //다시 앞보는중
        input.Look.Down.performed -= OnDown;        //밑을 보는중 

        input.Attack.attack.performed -= OnFire;

        input.Look.lookUp.canceled -= OnLookUp;     //다시 앞보는중
        input.Look.lookUp.performed -= OnLookFront;    //위 보는 중

        input.Jump.jump.performed -= OnJump;    //점프

        //input.Move.move.canceled -= OnMove;     //이동
        //input.Move.move.performed -= OnMove;    //이동취소

        MoveInputEnble(false);

        input.Disable();
    }

    public void MoveInputEnble(bool enable)
    {
        if (enable)
        {
            input.Move.move.performed += OnMove;        //이동
            input.Move.move.canceled += OnMove;         //이동 취소
        }
        else
        {
            input.Move.move.canceled -= OnMove;     //이동
            input.Move.move.performed -= OnMove;    //이동취소
        }
    }



    /// <summary>
    /// 플레이어의 좌우 이동
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(InputAction.CallbackContext context)
    {
        if (!groundSencer.onGround)
        {
            StopAllCoroutines();                                //중복 공중이동 코루틴실해을 방지하기 위해 모든 코루틴 정지
            StartCoroutine(AirMove());                          //공중에서 이동할때 공중움직임 코루틴 시작
        }
        readValue = context.ReadValue<Vector2>();               //입력 받은 값을 readValue에 저장 
        anim.OnMove(readValue);

        if (readValue.x < 0&& groundSencer.onGround)
        {
            playerCameraMove.CameraStayHere();
        }
        Vector2Int vec = new Vector2Int((int)readValue.x,(int)readValue.y);
        if(vec.x== 0)
        {
            playerCameraMove.StopAllCoroutines();
        }
        Sight();

    }
    /// <summary>
    /// 움직이는 방향으로 플레이어를 바라보게 하는 함수
    /// </summary>
    void Sight()
    {
        if (groundSencer.onGround && readValue.x != 0)
        {
            Vector2 vector2 = Vector2.one;          //로컬스케일 값에 저장할 변수 생성
            vector2.x = readValue.x;                //이동한 방향 저장
            if ((int)transform.localScale.x != (int)vector2.x)
            {
                anim.OnTrun();
                StopAllCoroutines();
                StartCoroutine(TrunDelay());
            }
            //transform.localScale = vector2;         //로컬 스케일값에 전달
        }
    }
    /// <summary>
    /// 돌아보는 딜레이
    /// </summary>
    /// <returns></returns>
    IEnumerator TrunDelay()
    {
        secMoveSpeed = 0.0f;                //이동정지
        yield return new WaitForSeconds(0.2f);  //기다리기
        secMoveSpeed = 1.0f;                //이동 계속
        Sight();                            //시야 재확인 좌우를 연타하여 뒤로 가는 것을 방지
    }
    /// <summary>
    /// 플레이어가 공중에서 이동할때 천천히 이동되게 하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator AirMove()
    {
        secMoveSpeed = 0.0f;                                //이동정지
        WaitForSeconds wait = new WaitForSeconds(0.1f);     //0.1초 대기시간 저장
        for (int i = 0; i < 10; i++)                        //0.1초마다 이동속도 0.1증가
        {
            secMoveSpeed += 0.1f;
            yield return wait;
        }
        secMoveSpeed = 1.0f;                                //이동증감 변수 초기화

    }


    /// <summary>
    /// 플레이어가 점프 실행 함수
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnJump(InputAction.CallbackContext _)
    {
        if (groundSencer.onGround)                                      //플레이어가 땅에 발을 디디고 있으면
        {
            rigid.AddForce(Vector2.up*jumpForce,ForceMode2D.Impulse);   //위로 jumpForce만큼 힘주기
            anim.OnFall(true);
            anim.OnJump();
        }
    }
    public override void Fly()
    {

        base.Fly();
        anim.OnFall(true);
        if (isDown)
        {
            sign = 3;  //하단
            anim.ChangeLookPoint(sign);
        }
    }
    public override void Landing()
    {
        base.Landing();
        anim.OnFall(false);
        if(hp>0)                                                    //죽은 상태가 아니면
            MoveStop(0.2f);                                         //착지 할때는 잠깐 경직
        Sight();
        if (isDown)
        {
            sign = 2;   //앉은 상태 정면 조준
            anim.ChangeLookPoint(sign);
        }
    }

    /// <summary>
    /// 플레이어가 위를 바라볼때 실행되는 함수
    /// </summary>
    /// <param name="_"></param>
    private void OnLookUp(InputAction.CallbackContext _)
    {
        if (!isDown)
        {
            isLookup = true;
            sign = 1;
            
            playerWeapon.AimMoved(sign);
            anim.ChangeLookPoint(sign);
        }
    }
    private void OnLookFront(InputAction.CallbackContext _)
    {
        if (!isDown)
        {
            isLookup = false;
            sign = 0;

            playerWeapon.AimMoved(sign);
            anim.ChangeLookPoint(sign);
        }
    }
    /// <summary>
    /// 아래 혹은 앉는 함수
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnDown(InputAction.CallbackContext _)
    {
        if (!isLookup)                      //위를 보고 있지않은 사태에서
        {
            isDown= !isDown;
            if (isDown)
            {
                if (groundSencer.onGround)      //땅에 닿고 있으면
                {
                    sign = 2;   //앉은 상태 정면 조준
                    MoveStop(0.3f);
                    sitmoveSpeed = 0.4f;
                   
                    
                }
                else
                {
                    sign = 3;  //하단
                }
            }
            else
            {
                sign= 0;
                sitmoveSpeed = 1.0f;
            }
            playerWeapon.AimMoved(sign);
            anim.ChangeLookPoint(sign);
        }
    }

    /// <summary>
    /// 무기 발사함수 실행
    /// </summary>
    /// <param name="_"></param>
    private void OnFire(InputAction.CallbackContext _)
    {
        if (sign == 2)
        {
            MoveStop(0.3f);
        }
        playerWeapon.OnAttack(sign,transform.localScale);
    }

    /// <summary>
    /// 수류탄 투척 함수 실행
    /// </summary>
    /// <param name="_"></param>
    private void OnGrenade(InputAction.CallbackContext _)
    {
        playerWeapon.OnGrenade();
    }



    /// <summary>
    /// 움직임을 잠깐 멈출때 실행할 함수
    /// </summary>
    /// <param name="waitTime">멈출 시간</param>
    private void MoveStop(float waitTime)
    {
        //IEnumerator moveStopper = MoveStopper(waitTime);
        //StopCoroutine(moveStopper);
        StopAllCoroutines();
        StartCoroutine(MoveStopper(waitTime));                  //코루틴 실행
    }
    /// <summary>
    /// 이동 정지 후 지정된 시간만큼 기다린 후 이동속도 증감 변수를 1로 변경하는 코루틴 변수
    /// </summary>
    /// <param name="waitTime">멈출 시간 </param>
    /// <returns></returns>
    IEnumerator MoveStopper(float waitTime)                 
    {
        secMoveSpeed = 0.0f;                        //이동정지
        yield return new WaitForSeconds(waitTime);  //지정된 만큼 기다리기
        secMoveSpeed = 1.0f;                        //이동개시
    }
    protected override void ReSpawn()
    {
        anim.OnReapwn();                    //애니메이션 재상
        //playerWeapon.weaponType=Weapon_Attack.WeaponType.Pistol;
        playerWeapon.OnRespawn();
        sprite.color = Vector4.one;         //스프라이트 컬러 초기화
        gameObject.layer = 3;
        hp = 1;                             //hp초기화
        playerCameraMove.CameraReset();
        InputEnable();
        base.ReSpawn();

    }
    protected override void FallingDead()
    {
        gameObject.layer = 10;
        playerCameraMove.CameraColOff();  //죽으면 카메라 고정
        playerCameraMove.CameraStayHere();
        anim.OnDeadResetAnim();
        InputDisable();                     //죽을 때 움직이는것을 방지
        readValue = Vector2.zero;           //이동중에 죽을때 죽고나서 한 방향으로 이동하는 것을 방지
        anim.OnDie();
    }
    protected override void OnDie()
    {
        gameObject.layer = 10;
        playerCameraMove.CameraColOff();
        playerCameraMove.CameraStayHere();
        anim.OnDeadResetAnim();
        base.OnDie();                       //죽으면 카메라 고정
        InputDisable();                     //죽을 때 움직이는것을 방지
        isLookup = false;
        isDown = false;
        sitmoveSpeed = 1.0f;
        readValue = Vector2.zero;           //이동중에 죽을때 죽고나서 한 방향으로 이동하는 것을 방지
        anim.OnDie();
    }

}

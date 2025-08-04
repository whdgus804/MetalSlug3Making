using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.InputSystem;

public class Player : PlayerHealth
{
    /// <summary>
    /// �÷��̾��� �̵��ӵ�
    /// </summary>
    [SerializeField] float moveSpeed;

    /// <summary>
    /// �÷��̾ �����ϸ� ������ ���� ��ġ
    /// </summary>
    [SerializeField] float jumpForce;


    /// <summary>
    /// 0-����
    /// 1-��
    /// 2-�ɱ�
    /// 3-��
    /// </summary>
    int sign = 0;
    /// <summary>
    /// �÷��̾��� ����, �����Ҷ� �ش� ��ġ�� ���� ����
    /// </summary>
    float secMoveSpeed=1.0f;

    /// <summary>
    /// �÷��̾ ���� �ٶ󺸰� �ִ��� ��Ÿ���� bool ���� true�� ���� ���� �ִ�
    /// </summary>
    bool isLookup = false;

    /// <summary>
    /// �÷��̾ �ٴ��� �ٶ󺸰ų� �ɴ����� ��Ÿ���� bool ���� true�� �ɰų� ���� ������
    /// </summary>
    bool isDown = false;


    /// <summary>
    /// �÷��̾��� ������ �ٵ�
    /// </summary>
    Rigidbody2D rigid;

    /// <summary>
    /// �÷��̾� �̵� ���� ������ vector2 ����
    /// </summary>
    Vector2 readValue = Vector2.zero;
    /// <summary>
    /// �⺻ �÷��̾��� ��ǲ�׼�
    /// </summary>
    BasicPlayerInput input;
    ///// <summary>
    ///// ������������ ��Ÿ���� ��ũ��Ʈ
    ///// </summary>
    //GroundSencer groundSencer;

    PlayerCameraMove playerCameraMove;
    /// <summary>
    /// �÷��̾� ���� ��ũ��Ʈ
    /// </summary>
    Weapon_Attack playerWeapon;

    PlayerAnimation anim;

    float sitmoveSpeed = 1.0f;

    
    
    protected override void Awake()
    {
        base.Awake();   
        input=new BasicPlayerInput();                           //��ǲ�׼� ����
        rigid= GetComponent<Rigidbody2D>();                     //�÷��̾��� ������ٵ� ����
        //groundSencer=GetComponentInChildren<GroundSencer>();    //��������Ȯ�� ��ũ��Ʈ ����
        playerCameraMove=FindAnyObjectByType<PlayerCameraMove>();   
        playerWeapon=GetComponent<Weapon_Attack>();             //�÷��̾� ���� ��ũ��Ʈ ����
        anim=GetComponent<PlayerAnimation>();

    }
    private void OnEnable()
    {
        InputEnable();
    }



    private void FixedUpdate()
    {
        transform.Translate(readValue*moveSpeed*secMoveSpeed*sitmoveSpeed*Time.deltaTime);        //�÷��̾� �̵�
    }

    private void OnDisable()
    {
        InputDisable();                                         //������Ʈ�� ��Ȱ��ȭ �Ǹ� ��ǲ�׼� ��Ȱ��ȭ 
    }
    /// <summary>
    /// ��ǲ�׼��� Ȱ��ȭ�ϴ� �Լ�
    /// </summary>
    public void InputEnable()
    {
        input.Enable();
        //input.Move.move.performed += OnMove;        //�̵�
        //input.Move.move.canceled += OnMove;         //�̵� ���

        MoveInputEnble(true);
        input.Jump.jump.performed += OnJump;        //����

        input.Look.lookUp.performed += OnLookUp;    //�� ���� ��
        input.Look.lookUp.canceled += OnLookFront;     //�ٽ� �պ�����

        input.Attack.attack.performed += OnFire;

        input.Look.Down.performed += OnDown;        //���� ������
        input.Look.Down.canceled += OnDown;         //�ٽ� ���� ������ 

        input.Throw.throwed.performed += OnGrenade;
    }


    /// <summary>
    /// ��ǲ�׼��� ���� �Լ�
    /// </summary>
    public void InputDisable()
    {
        input.Throw.throwed.performed -= OnGrenade;

        input.Look.Down.canceled -= OnDown;         //�ٽ� �պ�����
        input.Look.Down.performed -= OnDown;        //���� ������ 

        input.Attack.attack.performed -= OnFire;

        input.Look.lookUp.canceled -= OnLookUp;     //�ٽ� �պ�����
        input.Look.lookUp.performed -= OnLookFront;    //�� ���� ��

        input.Jump.jump.performed -= OnJump;    //����

        //input.Move.move.canceled -= OnMove;     //�̵�
        //input.Move.move.performed -= OnMove;    //�̵����

        MoveInputEnble(false);

        input.Disable();
    }

    public void MoveInputEnble(bool enable)
    {
        if (enable)
        {
            input.Move.move.performed += OnMove;        //�̵�
            input.Move.move.canceled += OnMove;         //�̵� ���
        }
        else
        {
            input.Move.move.canceled -= OnMove;     //�̵�
            input.Move.move.performed -= OnMove;    //�̵����
        }
    }



    /// <summary>
    /// �÷��̾��� �¿� �̵�
    /// </summary>
    /// <param name="context"></param>
    private void OnMove(InputAction.CallbackContext context)
    {
        if (!groundSencer.onGround)
        {
            StopAllCoroutines();                                //�ߺ� �����̵� �ڷ�ƾ������ �����ϱ� ���� ��� �ڷ�ƾ ����
            StartCoroutine(AirMove());                          //���߿��� �̵��Ҷ� ���߿����� �ڷ�ƾ ����
        }
        readValue = context.ReadValue<Vector2>();               //�Է� ���� ���� readValue�� ���� 
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
    /// �����̴� �������� �÷��̾ �ٶ󺸰� �ϴ� �Լ�
    /// </summary>
    void Sight()
    {
        if (groundSencer.onGround && readValue.x != 0)
        {
            Vector2 vector2 = Vector2.one;          //���ý����� ���� ������ ���� ����
            vector2.x = readValue.x;                //�̵��� ���� ����
            if ((int)transform.localScale.x != (int)vector2.x)
            {
                anim.OnTrun();
                StopAllCoroutines();
                StartCoroutine(TrunDelay());
            }
            //transform.localScale = vector2;         //���� �����ϰ��� ����
        }
    }
    /// <summary>
    /// ���ƺ��� ������
    /// </summary>
    /// <returns></returns>
    IEnumerator TrunDelay()
    {
        secMoveSpeed = 0.0f;                //�̵�����
        yield return new WaitForSeconds(0.2f);  //��ٸ���
        secMoveSpeed = 1.0f;                //�̵� ���
        Sight();                            //�þ� ��Ȯ�� �¿츦 ��Ÿ�Ͽ� �ڷ� ���� ���� ����
    }
    /// <summary>
    /// �÷��̾ ���߿��� �̵��Ҷ� õõ�� �̵��ǰ� �ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator AirMove()
    {
        secMoveSpeed = 0.0f;                                //�̵�����
        WaitForSeconds wait = new WaitForSeconds(0.1f);     //0.1�� ���ð� ����
        for (int i = 0; i < 10; i++)                        //0.1�ʸ��� �̵��ӵ� 0.1����
        {
            secMoveSpeed += 0.1f;
            yield return wait;
        }
        secMoveSpeed = 1.0f;                                //�̵����� ���� �ʱ�ȭ

    }


    /// <summary>
    /// �÷��̾ ���� ���� �Լ�
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnJump(InputAction.CallbackContext _)
    {
        if (groundSencer.onGround)                                      //�÷��̾ ���� ���� ���� ������
        {
            rigid.AddForce(Vector2.up*jumpForce,ForceMode2D.Impulse);   //���� jumpForce��ŭ ���ֱ�
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
            sign = 3;  //�ϴ�
            anim.ChangeLookPoint(sign);
        }
    }
    public override void Landing()
    {
        base.Landing();
        anim.OnFall(false);
        if(hp>0)                                                    //���� ���°� �ƴϸ�
            MoveStop(0.2f);                                         //���� �Ҷ��� ��� ����
        Sight();
        if (isDown)
        {
            sign = 2;   //���� ���� ���� ����
            anim.ChangeLookPoint(sign);
        }
    }

    /// <summary>
    /// �÷��̾ ���� �ٶ󺼶� ����Ǵ� �Լ�
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
    /// �Ʒ� Ȥ�� �ɴ� �Լ�
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnDown(InputAction.CallbackContext _)
    {
        if (!isLookup)                      //���� ���� �������� ���¿���
        {
            isDown= !isDown;
            if (isDown)
            {
                if (groundSencer.onGround)      //���� ��� ������
                {
                    sign = 2;   //���� ���� ���� ����
                    MoveStop(0.3f);
                    sitmoveSpeed = 0.4f;
                   
                    
                }
                else
                {
                    sign = 3;  //�ϴ�
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
    /// ���� �߻��Լ� ����
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
    /// ����ź ��ô �Լ� ����
    /// </summary>
    /// <param name="_"></param>
    private void OnGrenade(InputAction.CallbackContext _)
    {
        playerWeapon.OnGrenade();
    }



    /// <summary>
    /// �������� ��� ���⶧ ������ �Լ�
    /// </summary>
    /// <param name="waitTime">���� �ð�</param>
    private void MoveStop(float waitTime)
    {
        //IEnumerator moveStopper = MoveStopper(waitTime);
        //StopCoroutine(moveStopper);
        StopAllCoroutines();
        StartCoroutine(MoveStopper(waitTime));                  //�ڷ�ƾ ����
    }
    /// <summary>
    /// �̵� ���� �� ������ �ð���ŭ ��ٸ� �� �̵��ӵ� ���� ������ 1�� �����ϴ� �ڷ�ƾ ����
    /// </summary>
    /// <param name="waitTime">���� �ð� </param>
    /// <returns></returns>
    IEnumerator MoveStopper(float waitTime)                 
    {
        secMoveSpeed = 0.0f;                        //�̵�����
        yield return new WaitForSeconds(waitTime);  //������ ��ŭ ��ٸ���
        secMoveSpeed = 1.0f;                        //�̵�����
    }
    protected override void ReSpawn()
    {
        anim.OnReapwn();                    //�ִϸ��̼� ���
        //playerWeapon.weaponType=Weapon_Attack.WeaponType.Pistol;
        playerWeapon.OnRespawn();
        sprite.color = Vector4.one;         //��������Ʈ �÷� �ʱ�ȭ
        gameObject.layer = 3;
        hp = 1;                             //hp�ʱ�ȭ
        playerCameraMove.CameraReset();
        InputEnable();
        base.ReSpawn();

    }
    protected override void FallingDead()
    {
        gameObject.layer = 10;
        playerCameraMove.CameraColOff();  //������ ī�޶� ����
        playerCameraMove.CameraStayHere();
        anim.OnDeadResetAnim();
        InputDisable();                     //���� �� �����̴°��� ����
        readValue = Vector2.zero;           //�̵��߿� ������ �װ��� �� �������� �̵��ϴ� ���� ����
        anim.OnDie();
    }
    protected override void OnDie()
    {
        gameObject.layer = 10;
        playerCameraMove.CameraColOff();
        playerCameraMove.CameraStayHere();
        anim.OnDeadResetAnim();
        base.OnDie();                       //������ ī�޶� ����
        InputDisable();                     //���� �� �����̴°��� ����
        isLookup = false;
        isDown = false;
        sitmoveSpeed = 1.0f;
        readValue = Vector2.zero;           //�̵��߿� ������ �װ��� �� �������� �̵��ϴ� ���� ����
        anim.OnDie();
    }

}

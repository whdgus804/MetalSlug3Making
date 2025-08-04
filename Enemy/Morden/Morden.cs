using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morden : EnemyBase
{
    [SerializeField] Transform waterPos;
    /// <summary>
    /// ���� �������� ������ ����Ʈ
    /// </summary>
    [SerializeField] GameObject waterEffect;
    /// <summary>
    /// �ִϸ�����
    /// </summary>
    protected Animator anim;
    /// <summary>
    /// Ʈ�������� ���� x���� �����ϴ� ����
    /// </summary>
    float localX;
    /// <summary>
    /// ���� ����� Ȯ���ϴ� ��ũ��Ʈ
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
        secMoveSpeed = 0.0f;            //���� ��� �������� �̵�����
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")&& groundSencer.onGround)          //���� ��� �÷��̾ ���ݹ��� �ȿ� ���� ������ 
        {
            OnAttack(0.3f);                     //�����Լ� ����
        }
        else if (collision.CompareTag("Trun"))  //������ ���� ������
        {       
            TrunAnim();                         //�ڵ��ư��� �Լ� ����
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DeadZone"))        //������ �״� ������ ������
        {
            DeadZone();                                             //Ư�� ���� �Լ� ����
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(localX*moveSpeed * secMoveSpeed * Time.deltaTime,0,0);
    }
    public override void Fly()
    {
        anim.SetBool(onGround_To_Hash, false);          //�ִϸ��̼� ������
        secMoveSpeed = 0.0f;                            //�̵�����
    }
    public override void Landing()
    {
        anim.SetBool(onGround_To_Hash, true);       //�ִϸ��̼� �� ����
        secMoveSpeed = 1.0f;                        //�̵�
        LookPlayer();                               //�÷��̾ �ٶ󺸴� �Լ� ����
    }

    /// <summary>
    /// �ڸ� ���ƺ��� �Լ�
    /// </summary>
    protected override void TrunAnim()
    {
        anim.SetTrigger(trun_ToHash);           //�ִϸ��̼� ������
        localX = transform.localScale.x*-1;     //���� �ٽ� ����
    }

    protected override void OnAttack(float Delay)
    {
        base.OnAttack(Delay);                               
        anim.SetTrigger(attack_To_Hash);                            //�ִϸ��̼� ������
        float delay = anim.GetCurrentAnimatorStateInfo(0).length;       //�ִϸ��̼� �ð� ����
        StopMove(delay+0.5f, 1);                                        //������ ���������� ����
    }
    protected override void LookPlayer()
    {
        float distance = player.transform.position.x - transform.position.x;        //�÷��̾� ��ġ�� �ش� ������Ʈ�� ��ġ Ȯ��
        Vector3 local = transform.localScale;                                       //������ ���� ����
        if (distance < 0)           //�÷��̾ ����
        {
            if (local.x > 0)        //�ڸ� ���� ���� �Ѵٸ�
            {
                TrunAnim();         //�ڵ��ƺ���
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
        gameObject.layer = 10;          //������̾�� ����
        moveSpeed = 0;                  //�̵�����
        attackRange = Vector2.zero;     //���� ��ý���� ���ݹ��� ���ֱ�
        if (groundSencer.onGround)      //���� ���� ���¸�
        {   
            float distanc=player.transform.position.x - transform.position.x;   //�÷��̾���� ��ġ Ȯ��
            if(distanc < 0)
            {
                anim.SetTrigger("DieToBack");               //�÷��̾ �ڿ� �ִٸ� �ִϸ��̼� �� ����
            }
            
        }
        anim.SetTrigger(die_To_Hash);                  //�ִϸ��̼� ������ 
    }
    /// <summary>
    /// ��ü�� Ư�� ��ġ�� ������ ������ �����ϴ� �Լ�
    /// </summary>
    protected virtual void DeadZone()
    {

        Instantiate(waterEffect,waterPos.position,Quaternion.identity); //������ ��ġ�� ����Ʈ ����
        Destroy(gameObject);                //������Ʈ ����
    }
}

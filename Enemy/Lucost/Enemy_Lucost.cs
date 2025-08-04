using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Lucost : EnemyBase
{
    /// <summary>
    /// x���� �ִϸ��̼� Ŀ��
    /// </summary>
    [SerializeField] AnimationCurve xAnimCurv;
    /// <summary>
    /// y���� �ִϸ��̼� Ŀ��
    /// </summary>
    [SerializeField] AnimationCurve yAnimCurv;
    /// <summary>
    /// x���� �̵��ӵ�
    /// </summary>
    [SerializeField] float xMoveSpeed;
    /// <summary>
    /// y���� �̵��ӵ�
    /// </summary>
    [SerializeField] float yMoveSpeed;
    /// <summary>
    /// ��ü���� �̵��ӵ�
    /// </summary>
    [SerializeField] float flyMoveSpeed;
    /// <summary>
    /// �����غ���� ������ 
    /// </summary>
    [SerializeField] float readyAttackTime;
    /// <summary>
    /// ���� �غ� ������
    /// </summary>
    [SerializeField] float beforeAttackDelay;
    /// <summary>
    /// �����Ҷ� �÷��̾���ġ�� �̵��ϰԵ� ����
    /// </summary>
    [SerializeField] Transform attackTarget;
    /// <summary>
    /// �÷��̾ ��Ե� ��ġ
    /// </summary>
    [SerializeField] Transform hangPos;
    /// <summary>
    /// ���� �ִϸ��̼��� ���� �� �÷��̾ ����帱 �ð��� ������ ����
    /// </summary>
    [SerializeField] float afterAttackDelay;
    /// <summary>
    /// ���� �� ������ ����1
    /// </summary>
    [Space(20.0f)]
    [SerializeField] GameObject wingA;
    /// <summary>
    /// ����2
    /// </summary>
    [SerializeField] GameObject wingB;
    /// <summary>
    /// ����3
    /// </summary>
    [SerializeField] GameObject leg;
    [SerializeField] GameObject parts;
    /// <summary>
    /// ���� �߽��� �� ��ġ
    /// </summary>
    Transform center;
    /// <summary>
    /// �߽����� �̵��ϰ��ϴ� ��ũ��Ʈ
    /// </summary>
    LucostCenter lucostCenter;

    Lucost_Animation anim;

    /// <summary>
    /// �ִϸ��̼� Ŀ�꿡 ���� ����
    /// </summary>
    float xcurv = 0.0f;
    /// <summary>
    /// �ִϸ��̼� Ŀ�꿡 ���� ����
    /// </summary>
    float ycurv = 0.0f;  
    /// <summary>
    /// �ִϸ��̼� Ŀ�꿡 ���� ����
    /// </summary>
    float curvTime = 0.0f;  
    protected override void Awake()
    {
        base.Awake();               
        center = transform.parent;                              //���� �ʱ�ȭ
        lucostCenter=GetComponentInParent<LucostCenter>();      //���� �ʱ�ȭ
        anim=GetComponent<Lucost_Animation>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Target"))                     //���������� �ٴٸ��� ��Ÿ��
        {
            Attack();                                           //�����Լ� ����
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
    /// �����ϴ� �Լ�
    /// </summary>
    void StartFly()
    {
        StopAllCoroutines();                    //��� �ڷ�ƾ ����
        StartCoroutine(FlyingCoroutin());       //�����ڷ�ƾ ����
        StartCoroutine(PatrolCro());            //���� �ð� �� ���� �غ� �ϰ��ϴ� �ڷ�ƾ �Լ� ����
    }

    /// <summary>
    /// ���������� �߽� ��ġ �������� �ɵ��� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator FlyingCoroutin()
    {
        while (true)
        {
            curvTime += flyMoveSpeed * Time.deltaTime;                                                                      //�ð� �ޱ�
            xcurv = xAnimCurv.Evaluate(curvTime);                                                                           //�ִϸ��̼� Ŀ�꿡 ���� �� �ޱ�
            ycurv = yAnimCurv.Evaluate(curvTime);                                                                           //�ִϸ��̼� Ŀ�꿡 ���� �� �ޱ�
            transform.Translate(xcurv * xMoveSpeed * Time.fixedDeltaTime, ycurv * yMoveSpeed * Time.fixedDeltaTime, 0);     //��ġ �̵�
            yield return new WaitForFixedUpdate();
        }
    }
    /// <summary>
    /// ���� �ð��� �����غ� �����ϰ� �ϴ� �ڷ�ƾ �Լ�
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
    /// ������ �غ��ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    void AttackReady()
    {
        StopAllCoroutines();                                                //��� �ڷ�ƾ ����
        lucostCenter.Attacking(true);
        StartCoroutine(MoveToWards(center,moveSpeed*0.2f));                 //��ġ �߽����� �̵�
        anim.AttackAnim();
        StartCoroutine(StartAttack());                                      //���� �����ϴ� �ڷ�ƾ �Լ� ����
    }
    /// <summary>
    /// ������ �����ϰ� ��ǥ ��ġ���� ���ư��� �ڷ�ƾ �Լ�
    /// </summary>
    /// <param name="target">��ǥ��ġ</param>
    /// <param name="speed">�ӵ�</param>
    /// <returns></returns>
    IEnumerator MoveToWards(Transform target, float speed)
    {
        while (true)
        {
            transform.position=Vector2.MoveTowards(transform.position, target.position,speed*Time.fixedDeltaTime);      //��ǥ��ġ�� ������ �ӵ��� �̵�
            yield return new FixedUpdate();
        }
    }
    /// <summary>
    /// ������ �����ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(beforeAttackDelay);                     //���� �غ� �ð����� ���
        AttackMove();                                                           //�����̵��Լ� ����
    }
    /// <summary>
    /// ������ ���� �÷��̾�� �ٰ����� �Լ�
    /// </summary>
    void AttackMove()
    {
        StopAllCoroutines();                                                    //��� �ڷ�ƾ ����
        lucostCenter.StopMoveCenter(100.0f);                                    //�߽� �̵� ����
        attackTarget.parent = null;                                             //��ǥ��ġ �θ�����
        attackTarget.position = player.transform.position;                      //�÷��̾� ��ġ�� ��ǥ��ġ ����
        attackTarget.gameObject.SetActive(true);                                //��ǥ��ġ Ȱ��ȭ
        StartCoroutine(MoveToWards(attackTarget, moveSpeed*0.7f));                    //��ǥ��ġ�� �̵��ϰ� �̵� �ڷ�ƾ�� ���� ����
    }
    /// <summary>
    /// ������ �����ϴ� �Լ�
    /// </summary>
    void Attack()
    {
        StopAllCoroutines();                                                    //��� �ڷ�ƾ ����
        attackTarget.gameObject.SetActive(false);                               //��ǥ ��ġ ��Ȱ��ȭ
        Collider2D collider;                                                    //�浹ü ����
        collider = Physics2D.OverlapBox(attackPos.position, attackRange, 0, attackLayer);           //�÷��̾�ִ��� ���� ��ġ ���� ���� ��ŭ �÷��̾� ã��
        if (collider != null)                                                                       //�÷��̾ ������
        {
            PlayerHealth player = collider.GetComponent<PlayerHealth>();                            //��� Ÿ���� �÷��̾ ������ �ִ� HP��ũ��Ʈ ����
            if (player == null)
                player = GameManager.Instance.PlayerHealth;
            GroundSencer playergrounSencer=player.gameObject.GetComponentInChildren<GroundSencer>();    //�÷��̾ ���߿��� ���̱� ������ ������ �ޱ� ���� ��ũ��Ʈ ���� ����
            playergrounSencer.onGround = false;                                                         //�÷��̾ ������ �������� �ϱ�
            PlayerAnimation anim=collider.GetComponent<PlayerAnimation>();
            anim.OnDie_Lucost();                                                                        //�÷��̾� �����״� �ִϸ��̼�
            Vector3 vec3 = transform.localScale;
            vec3.x *= -1;
            player.transform.localScale = vec3;
            player.HP = 100;                                                                            //���� ������ ��ŭ ������ �ֱ�   
            StartCoroutine(HangPlayer(collider.gameObject));                                            //�÷��̾ �Ŵٴ� �ڷ�ƾ �Լ� ����
            StartCoroutine(MoveToWards(center, moveSpeed*0.5f));                                        //�߽� �������� ���ư��� �� �̵� �Լ��� ���� ����
            //Debug.Log($"{collider.name}_attack({gameObject.name})");              //�÷��̾ ���� 
        }
        else                                                                                            //�÷��̾ ������
        {
            lucostCenter.StopMoveCenter(0.0f);                                                          //�߽����� �̵���ũ��Ʈ�� �̵� �ٽ� Ȱ��ȭ
            //anim.Trun();
            StartCoroutine(MoveToWards(center, moveSpeed*0.5f));                                        //�߽����� �̵�
            StartCoroutine(WaitCoroutine(2.0f));                                                        //�ǵ��� �������� ��ٸ���
        }
    }
    /// <summary>
    /// ���� �ð��� ��ٸ��� �ڷ�ƾ �Լ�
    /// </summary>
    /// <param name="time">��ٸ� �ð�</param>
    /// <returns></returns>
    IEnumerator WaitCoroutine(float time)
    {
        yield return new WaitForSeconds(time);                          //�ð� ��ٸ���
        lucostCenter.Attacking(false);
        StartFly();                                                     //���� �Լ� ����
    }
    /// <summary>
    /// �÷��̾ �Ŵް� �������� ���� �ö� ���̴� �ڷ�ƾ �Լ�
    /// </summary>
    /// <param name="player">�÷��̾�</param>
    /// <returns></returns>
    IEnumerator HangPlayer(GameObject player)
    {
        anim.ExcutionAnim();
        Rigidbody2D playerrigid=player.GetComponent<Rigidbody2D>();         //�÷��̾��� ������ �ٵ� �ޱ�
        playerrigid.bodyType = RigidbodyType2D.Kinematic;                   //������ �ٵ� Ÿ���� Ű�׸�ƽ���� ���� (���ϸ� �ٴڿ� ������ �ʹ� ������ ������)
        float time = 0.0f;                                                  //�ð��� ������ ���� ����
        while (true)
        {
            player.transform.position = hangPos.position;                   //�÷��̾� ��ġ ����
            time += Time.deltaTime;                                         //�ð� ����
            if (time > afterAttackDelay)                                    //�ð��� ���� �� �����̺��� ��������
            {
                playerrigid.bodyType= RigidbodyType2D.Dynamic;              //�÷��̾� ������
                StartFly();                                                 //�������
                lucostCenter.StopMoveCenter(0.0f);                          //�߽��̵� ��ũ��Ʈ�� �߽��̵� ����
            }
            yield return new WaitForFixedUpdate();  
        }
    }

    /// <summary>
    /// �������� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="run">���̸� ������ �����̸� ������</param>
    public void Runaway(bool run)
    {
        StopAllCoroutines();            //��� �ڷ�ƾ ����
        if (run)
        {
            StartCoroutine(FlyingCoroutin());           //���� �ڷ�ƾ ����
            StartCoroutine(WaitCoroutine(3.0f));        //�̵� �� �ٽ� ���� �غ� �ð� ���� �� ���� �����ϰ� ���� �ڷ�ƾ ����

        }
        else
        {
            StartFly();                 //�������
        }

    }
    /// <summary>
    /// ���� ������ �ִϸ��̼�
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
        Instantiate(wingA, transform.position, Quaternion.identity);        //������ ���� ����
        Instantiate(wingB, transform.position, Quaternion.identity);        //������ ���� ����
        Instantiate(leg, transform.position, Quaternion.identity);        //������ ���� ����
        Instantiate(leg, transform.position, Quaternion.identity);        //������ ���� ����
        Instantiate(parts, transform.position, Quaternion.identity);        //������ ���� ����
    }
}

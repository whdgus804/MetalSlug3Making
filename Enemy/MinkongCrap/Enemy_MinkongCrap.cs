using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MinkongCrap : EnemyBase
{

    

    /// <summary>
    /// ������� ó�� ���� Ÿ��
    /// </summary>
    public enum IdleType
    {
        Combat,     //���� ������ ������ �ٸ� Ÿ���� ���� ������ �ش� Ÿ������ ���� 
        Eatting,    //���� �Դ� ��
        Idle        //������ ���ִ� �� 
    }
    /// <summary>
    /// ������� ó�� ���� Ÿ��
    /// </summary>
    public IdleType idleType;
    /// <summary>
    /// ������� ���� Ÿ�� ����, ���Ÿ�
    /// </summary>
    public enum AttackType
    {
        Random,
        Close,      //����
        Long        //���Ÿ�
    }
    /// <summary>
    /// ���� �Ѿ�
    /// </summary>
    [SerializeField] GameObject bullet;
    /// <summary>
    /// ������� ���� Ÿ�� ����, ���Ÿ�
    /// </summary>
    public AttackType attackType;
    /// <summary>
    /// �������� ��������
    /// </summary>
    [SerializeField] float attackDelay;
    /// <summary>
    /// ���������� �����ϴ� �Ÿ�
    /// </summary>
    [SerializeField] float attackSencerRange;
    /// <summary>
    /// ���Ÿ� ���� ����
    /// </summary>
    [SerializeField] float shootSencerRange;

    /// <summary>
    /// ���� �� �յڷ� ���ƴٴҶ� �� �� �̵��Ҷ� �̵��� �ִ�ð�
    /// </summary>
    [Space(20.0f)]
    [SerializeField] float patrolTime;
    /// <summary>
    /// ���� ����� Ȯ���ϴ� ��ũ��Ʈ
    /// </summary>
    GroundSencer groundSencer;
    /// <summary>
    /// ���Ÿ�Ÿ���� �߻簡���� ��Ÿ���� bool���� true�� �߻簡��
    /// </summary>
    bool readyToShoot = true;


    Vector2 moveValue = Vector2.zero;

    /// <summary>
    /// ������� �ִϸ��̼ǽ�ũ��Ʈ
    /// </summary>
    MinkongCrap_Animation anim;

    /// <summary>
    /// �̵� ���� �������� ���� ����(moveValue * moveSpeed * secMoveSpeed)
    /// </summary>
    Vector2 moveValueAdd = Vector2.zero;

    /// <summary>
    /// �Ӹ� ����
    /// </summary>
    [Space(20.0f)]
    [SerializeField] GameObject parts_Head;
    /// <summary>
    /// ���� ����
    /// </summary>
    [SerializeField] GameObject parts_Hand;
    /// <summary>
    /// �ٸ� ����
    /// </summary>
    [SerializeField] GameObject parts_Leg;

    /// <summary>
    /// ���� �� ����Ʈ
    /// </summary>
    [SerializeField] GameObject smallBlood;
    /// <summary>
    /// ū �� ����Ʈ
    /// </summary>
    [SerializeField] GameObject bigBlood;

    [SerializeField] GameObject head;
    /// <summary>
    /// �ٸ������� ������ ��ġ �迭
    /// </summary>
    Transform[] legPos;
    /// <summary>
    /// ���������� ������ ��ġ �迭 
    /// </summary>
    Transform[] handPos;
    /// <summary>
    /// �Ӹ� ������ ������ ��ġ 
    /// </summary>
    Transform headPos;

    
    protected override void Awake()
    {
        base.Awake();
        anim=GetComponent<MinkongCrap_Animation>();         //�ִϸ��̼� �ޱ�
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
        if (attackType == AttackType.Random)        //�����̸�
        {
            int rand = Random.Range(0, 2);          //0����1���� ����
            if(rand == 0)                           //0�̸�
            {
                attackType = AttackType.Close;      //����
            }
            else                                    //1�̸�
            {
                attackType=AttackType.Long;         //���Ÿ�
            }
        }
        LookPlayer();                               //�÷��̾� �ٶ󺸱�
        moveValue.x = transform.localScale.x;
    }
    private void OnEnable()
    {
        
        //LookPlayer();
        gameObject.layer = 6;
        //boxCollider.offset = attackPos.position + transform.position;
        //boxCollider.size = attackRange;
        switch (idleType)                           //������� Ÿ�Կ� ���� �ൿ
        {
            case IdleType.Combat:                   //������ 
                //StopMove(0.0f, 1.0f);
                if (attackType == AttackType.Long) //���Ÿ� Ÿ���̸� 
                {
                    StartCoroutine(PlayerDistance());           //�÷��̾���� �Ÿ��� ����ϴ� �ڷ�ƾ ����
                }

                //anim.MoveAnim(1.0f);
                break;
            case IdleType.Eatting:                  //�԰��ִ� ���¸� 
                StopMove(0.0f, 0.0f);                 //������ ���ֱ�
                anim.Eat(true);
                //�ִϸ��̼�
                break;
            case IdleType.Idle:                     //������ �ִ� ���¸� 
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
        anim.MoveAnim(moveValueAdd.x);                          //�ִϸ����Ϳ� ������
        

    }

    public void OnAttackTrigger()
    {
        if (idleType == IdleType.Combat)
            OnAttack(attackDelay);              //���������Լ� ����
    }

    public override void Fly()
    {
        anim.Fly();             //�ִϸ��̼� ���
        secMoveSpeed = 0.0f;    //�̵�����
        StopAllCoroutines();    //��� �ڷ�ƾ ����
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
    /// �÷��̾���� �Ÿ��� �����ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerDistance()
    {
        float distance = 0.0f;          //�÷��̾���� �Ÿ��� ����� ���� ����
        while (true)
        {
            distance = (player.transform.position - transform.position).sqrMagnitude;         //�÷��̾�� �Ÿ�����

            if (distance < 3 * 3)                   //�ʹ������� 
            {
                RunAway();                          //�ڷ� �������� �Լ� ����
            }
            else if(distance<5*5)                   //���Ÿ� ���� ���� ���̸�
            {
                if (readyToShoot)                   //�߻簡���� �����϶�
                {
                    ShootBefor();                   //�߻� �Լ� ����
                }
            }
            yield return null;
            //    if(attackType == AttackType.Long && !attacking)                                                  //���� ���Ÿ� Ÿ���̸�
            //    {
            //        if (distance < shootSencerRange * shootSencerRange+shootSencerRange)            //�÷��̾ ���� ��Ÿ��� ���� ��Ÿ��� ���� ����ŭ ������
            //        {
            //            if (readyToShoot)                                                           //���� ���ɻ��¸�
            //            {
            //                if (distance < shootSencerRange * shootSencerRange)                     //�÷��̾ ���� ��Ÿ����� ������ 
            //                {
            //                    Debug.Log("Run");
            //                    RunAway();                                                          //��ü ����
            //                }
            //                else                                                                    //���� ��Ÿ��� ���� �� �̶� ���� ��Ÿ� ���̿� ������
            //                {
            //                    //�߻� 
            //                    ShootBefor();

            //                    //Shoot();                                                            //����
            //                }
            //            }
            //            else                                                                        //���� ������ ���°� �ƴϸ�
            //            {
            //                Debug.Log("Run");
            //                RunAway();                                                              //������ ����
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
        StopAllCoroutines();                        //�ڷ�ƾ ����
        if (player.HP > 0)
        {
            LookPlayer();                               //�÷��̾� �ٶ󺸱�
            StopMove(Delay+1.0f, 1.0f);                 //�̵�����
            if(hp > 0)
            {
                base.OnAttack(Delay);                       //�÷��̾� ���ݽ���
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
    /// �Ѿ��� �߻��ϱ� �� �ٸ� �ڷ�ƾ�� ���ߴ� �Լ�
    /// </summary>
    void ShootBefor()
    {
        if (HP > 1)
        {

        StopAllCoroutines();                    //��� �ڷ�ƾ ���� 
        StartCoroutine(Shooting());             //�߻����
        }
    }
    /// <summary>
    /// ���Ÿ� �����Ҷ� ������ �� �Ѿ��� �����ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator Shooting()
    {
        anim.Shoot();                                           //�ִϸ��̼� ���
        StopMove(1.5f, 1);                                      //�̵� ����
        readyToShoot = false;                                   //�߻� �Ұ��� ���� ����
        yield return new WaitForSeconds(0.5f);                  //��������
        GameObject obj = bullet;                                //�Ѿ˰� ���� ������Ʈ ����
        obj.transform.localScale = transform.localScale;        //�Ѿ��� ���ý����ϰ����� (�ش� �����ϰ��� ���� �Ѿ��� ������ ������ �ٸ�)
        Instantiate(obj,attackPos.position,Quaternion.identity);//�Ѿ� ����
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(PatrolCoroutin(0.0f));
    }

    /// <summary>
    /// ���Ÿ� ������ �ϱ� ���� �÷��̾�κ��� �־����� �Լ�
    /// </summary>
    void RunAway()
    {
        StopAllCoroutines();                //��� �ڷ�ƾ ����
        
        LookPlayer();                               //�÷��̾ �ٶ󺸱�
        moveValue.x =transform.localScale.x;        //�̵� ���� �ʱ�ȭ                          
        moveValue *= -1.0f;                          //�÷��̾�������� ���� �־����Բ� ���� ����
        //secMoveSpeed = 1.0f;                        //�� �̵�
                                                    //Debug.Log($"{moveValue.x}X{secMoveSpeed}X{moveSpeed}={moveValueAdd.x}");
                                                    //StopMove(0.0f, 1.0f);

        StartCoroutine(RunCouroutin());     //�ڷ�ƾ ���� 
    }
    /// <summary>
    /// �÷��̾�κ��� �־����� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator RunCouroutin()
    {
        //secMoveSpeed = 1.0f;
        
        yield return new WaitForSeconds(1.0f);
        LookPlayer();
        moveValue.x=transform.localScale.x;         //���� ������ �̵��ϰ� �� ��ȯ
        
        //secMoveSpeed = 1.0f;
        //StartCoroutine(PatrolCoroutin(1.0f));           //�÷��̾���� �Ÿ� ���
        //StartCoroutine(PlayerDistance());
        StartCoroutine(PatrolCoroutin(1.0f));
    }


    /// <summary>
    /// �÷��̾ �����ϰ� ��񵿾� ������ �����ϰ� �Դٰ��ٸ� �ݺ��ϴ� �ڷ�ƾ�Լ� ���� �����Լ� ȣ��
    /// ������ �÷��̾ ���� ���ݹ��� ������ ������ �ٷ� �ٽ� ����
    /// </summary>
    IEnumerator PatrolCoroutin(float attackAfterDelay)
    {
        yield return new WaitForSeconds(attackAfterDelay);                                      //���� �ĵ����̱��� ��ٸ���
        LookPlayer();
        
        int randInt = Random.Range(3, 6);                                                      //������ Ƚ��
        if (attackType == AttackType.Long)                                                      //���Ÿ� Ÿ���̸� ���� ����
        {
            randInt = 3;
        }

        for (int i = 0; i < randInt; i++)                                                        //�����ݺ�
        {
            LookPlayer();                                                                       //�� �������� �÷��̾� �ٶ󺸱�
            int moveArrow = Random.Range(-1, 2);                                                // ��, ���ڸ�, �� �� ���� ����
            if (attackType == AttackType.Long)                                                  //���Ÿ� Ÿ���̸�
            {
                moveArrow = Random.Range(-1, 1);                                                //�÷��̾� �������� �����ʰ� ����
                moveArrow *= (int)transform.localScale.x;                                       //�÷��̾� �������� �����ʰ� ����
            }

            moveValue.x = moveArrow;                                                            //��, ���ڸ�, �ڷ� �̵�
            float randfloat = Random.Range(0.3f, patrolTime);                                   //�� ���� ������ �ð�
            anim.MoveAnim(moveValueAdd.x);                                                      //�ִϸ��̼ǿ� ������
            yield return new WaitForSeconds(randfloat);
        }
        LookPlayer();                                                                           //�÷��̾� �ٶ󺸱�
        readyToShoot = true;                                                                    //���Ÿ� ������ �߻簡���ϵ��� ���� ����
        
        moveValue.x = transform.localScale.x;                                                   //������ �������� �÷��̾� ������ �ɾ��
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
        if(idleType != IdleType.Combat)                                 //���� ������ ���ߴµ� ������ ���¸�
        {
            idleType=IdleType.Combat;                                   // ����Ÿ������ ����
            StopMove(1.0f, 1);                                          //��� ����
            moveValue.x = transform.localScale.x;                                         //������ �ʱ�ȭ
            OnEnable();                                                 //�ٽ� �b��ȭ �Լ� ����
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
        
        Instantiate(parts_Head,headPos.position,Quaternion.identity);       //�Ӹ����� ����
        //���� ������������ ������ŭ ����
        for(int i=0;i<handPos.Length;i++)                                   //���� ���� ��ŭ �ݺ�
        {
            GameObject obj = parts_Hand;                                    //���ӿ�����Ʈ�� ���� �� �ڵ� ������Ʈ�� ����
            int rand = Random.Range(-1, 2);                                 //������ �ޱ�
            if (rand == 0)                                                  //���ý����� x�� 0�̸� �ʺ��̱⿡ 0�̸� 1����
                rand = 1;
            obj.transform.localScale = new Vector3(rand, 1, 1);             //���� ���� 
            Instantiate(obj, handPos[i].position, Quaternion.identity);     //����

        }
        //�ٸ� ������ŭ ���� �������� ���� 
        for(int i=0; i < legPos.Length; i++)
        {
            GameObject leg = parts_Leg;                                     //�ٸ�������Ʈ�� �� ������Ʈ�� ����
            int legrand = Random.Range(-1, 2);                              //������ �ޱ�
            if (legrand == 0)
                legrand = 1;
            leg.transform.localScale = new Vector3(legrand, 1, 1);          //������ ����
            Instantiate(leg, legPos[i].position , Quaternion.identity);     //����
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

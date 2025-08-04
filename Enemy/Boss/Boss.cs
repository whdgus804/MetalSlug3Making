using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : EnemyHP
{
    //�ڷ� ���ٰ� ������ �� ����
    //�ұ����� �߻� �� �� �߻������� �ϳ��� ���������� �ϳ��� �÷��̾� ������ ��������
    //�÷��̾������� ĳ�� �߻�
    /// <summary>
    /// �����ӵ�
    /// </summary>
    [SerializeField] float rushSpeed;
    /// <summary>
    /// �ڷ� ������ �ӵ�
    /// </summary>
    [SerializeField] float backSpeed;
    /// <summary>
    /// ���� ��Ÿ��
    /// </summary>
    [SerializeField] float rushCoolTime;
    /// <summary>
    /// ���� źȯ
    /// </summary>
    [SerializeField] GameObject cannon;
    /// <summary>
    /// ĳ���� �߻�� ��ġ
    /// </summary>
    [SerializeField] Transform cannonPos;
    /// <summary>
    /// �� ����
    /// </summary>
    [SerializeField] GameObject fireBullet;
    /// <summary>
    /// �ҵ��� ���ʹ���
    /// </summary>
    [SerializeField] GameObject fireBullet_Left;
    [SerializeField] Transform fireBullet_FirePos;
    [SerializeField] Transform fireBullet_Left_FirePos;
    [SerializeField] GameObject fireBallFireEffect;

    /// <summary>
    /// ĳ�� �߻� ����Ʈ
    /// </summary>
    [SerializeField] GameObject cannonFireEffect;
    /// <summary>
    /// ĳ�� ���� ����Ʈ
    /// </summary>
    [SerializeField] GameObject deployingCannonEffect;

    /// <summary>
    /// �÷��̾� ��ġ�� �˸� Ʈ������
    /// </summary>
    [SerializeField] Transform target;

    [SerializeField] GameObject[] waterEffect;



    /// <summary>
    /// �� �� ���ĵ� �� ���������� ��ȯ�� �Ҳ�
    /// </summary>
    [SerializeField] Transform[] rocketTail;

    [SerializeField] float fallSpeed;
    /// <summary>
    /// ���� �̵��ӵ�
    /// </summary>
    float moveSpeed = 0.0f;
    /// <summary>
    /// �����غ� ��Ÿ���� ���� true�� ���� ���� ����
    /// </summary>
    bool readyRush = false;
    /// <summary>
    /// ���� ü���� �Һ�Ǹ� true�Ǵ� ����
    /// </summary>
    bool phaseTwo = false;
    Animator anim;
    PlayerHealth player;

    BossStageManager stageManager;
    /// <summary>
    /// ���� ���������� Ȥ�� ���� �غ������� ��Ÿ���� ���� true�� ������
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
    /// ������ �����ϴ� �Լ�
    /// </summary>
    void OnAttack()
    {

        if (player.HP > 0)                                  //�÷��̾ ���������
        {
            if (readyRush)                                  //�������� �̸�
            {
                StartCoroutine(RushCoroutine());            //�����ڷ�ƾ ����
            }
            else
            {
                //������ ��Ÿ�ӿ� �� ���¸�
                StartCoroutine(OnFireCorountine(0));        //�Ѿ˹߻�
            }
        }
        else
        {
            StartCoroutine(WaitPlayerRespawn());        //�÷��̾ ��Ȱ�Ҷ����� ��ٸ���
        }
    }
    /// <summary>
    /// �÷��̾ ��Ȱ�� ������ ��ٸ��� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitPlayerRespawn()
    {
        yield return new WaitUntil(() => player.HP > 0);  //�÷��̾� ��Ȱ ��ٶ��
        OnAttack(); //�����Լ� ����
    }
    /// <summary>
    /// �ڷ� ���� ������ �����ϴ� �ڷ�ƾ�Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator RushCoroutine()
    {
        rushing = true;                     //������ ���� ����
        readyRush = false;                  //���� ���� ����
        moveSpeed = backSpeed;              //�ڷ��̵�
        //�ִϸ��̼� ����
        yield return new WaitForSeconds(2.0f);  //�ڷ� �� �ð�

        moveSpeed = 0.0f;           //����
        yield return new WaitForSeconds(2.0f);
        anim.SetBool("Rush", true);     //�ִϸ��̼� ������
        moveSpeed = rushSpeed;          //�̵��ӵ� ����
        //�ִϸ��̼� ���
        yield return new WaitForSeconds(2.8f);  //������ �ð�
        anim.SetBool("Rush", false);        //�ִϸ��̼ǰ�����
        moveSpeed = backSpeed;              //�ڷ� ��
        yield return new WaitForSeconds(0.8f);  //�ٽ� �ڸ��� ���ư� �ð�
        moveSpeed = 0.0f;       //�̵�����
        rushing = false;         //���� ���� ����
        StartCoroutine(RushCoolDown());
        OnAttack();
    }
    /// <summary>
    /// ���� ��Ÿ�� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator RushCoolDown()
    {
        yield return new WaitForSeconds(rushCoolTime);
        readyRush = true;
    }

    /// <summary>
    /// ĳ�� �� ���� �߻��ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator OnFireCorountine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);      //��ٸ���
        anim.SetTrigger("Fire");                        //�߻� �ִϸ��̼� ������
        target.position = player.transform.position; //�÷��̾� ��ġ�� �̵�
        if (phaseTwo)       //���� ������ 2���� ��
        {
            //ĳ��
            Instantiate(cannon, cannonPos.position, Quaternion.identity);   //����ź �߻�
            cannonFireEffect.SetActive(true);                               //���� ����Ʈ
        }
        else //1�������̸�
        {
            //��
            fireBallFireEffect.SetActive(true);         //�ҵ��� ����Ʈ
            Instantiate(fireBullet, fireBullet_FirePos.position, Quaternion.identity);    //�ҵ��� ����
            yield return new WaitForSeconds(0.2f);      //��ٸ���
            Instantiate(fireBullet_Left, fireBullet_Left_FirePos.position, Quaternion.identity);  //���� �ҵ��� �߻�
        }
        yield return new WaitForSeconds(3.0f); //���� ��Ÿ��
        OnAttack();             //���� �Լ� ����
    }
    protected override void OnDie()
    {
        if (!phaseTwo)      //���� ������ 1�̸�
        {
            if (rushing)     //�������̸�
            {
                StartCoroutine(WaitRushEnd()); //������ ���������� ��ٸ��� �ڷ�ƾ �Լ� ����
            }
            else //�������� �ƴϸ�
            {

                StopAllCoroutines();                //��� �ڷ�ƾ ����
                StartCoroutine(RocketTail());       //������ ������ ����Ʈ�� �����ϴ� �ڷ�ƾ ����
                phaseTwo = true;                    //���� ����
                hp = 20;                             //ü�°� ����
                anim.SetTrigger("Cannon");          //�ִϸ��̼� ������
                deployingCannonEffect.SetActive(true);  //�������� ����Ʈ
                StartCoroutine(OnFireCorountine(3.0f)); //�߻� �ڷ�ƾ ���� 
                StartCoroutine(RushCoolDown());     //���� ��Ÿ�� ���� ����
            }
        }
        else
        {
            //����ȭ �ִϸ��̼�
            //�ֱ������� ���� ����
            StopAllCoroutines();            //��� �ڷ�ƾ ����
            StartCoroutine(WaterEffect());
            anim.SetTrigger("Defeat");      //�ִϸ��̼ǿ� ������
            //moveSpeed = 0.0f;               //�̵�����
            stageManager.GameOver();        //�������� �Ŵ����� �Լ� ����
        }
    }
    /// <summary>
    /// ���� �ν��� �Ҳ�����Ʈ�� �����ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator RocketTail()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);     //��ٸ� �ð� ����
        for (int i = 0; i < rocketTail.Length; i++)
        {
            int rand = Random.Range(0, rocketTail.Length);      //�������ޱ�
            rocketTail[rand].gameObject.SetActive(true);        //���� �������� ������� �҄� Ȱ��ȭ
            yield return wait;                                  //��ٸ���
        }
        StartCoroutine(RocketTail());
    }
    /// <summary>
    /// ���� �� ���������� ��ٸ��� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitRushEnd()
    {
        yield return new WaitUntil(() => !rushing);     //��ٸ���
        OnDie();                               //�Լ� ���� ����

    }
    /// <summary>
    /// �� ����Ʈ�� Ȱ��ȭ �ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator WaterEffect()
    {
        moveVec = Vector2.zero;         //�̵�����
        for(int i= 0; i < waterEffect.Length; i++)      //��� ����Ʈ�� �ѱ�
        {
            waterEffect[i].transform.parent = null;     //���� ������ ���� ���� �����ϱ����� �θ� ����
            waterEffect[i].SetActive(true);             //����Ʈ(���� ������Ʈ) Ȱ��ȭ
        }
        yield return new WaitForSeconds(2.0f);          //�ð� ��ٸ���
        moveVec = new Vector2(0, -1);                   //������ ����ɱ�
        moveVec *= fallSpeed;                           //�ӵ� �߰�
        for(int i=0; i < 100; i++)                      //����Ʈ �������� ��� Ȱ��ȭ �ϱ�
        {
            yield return new WaitForSeconds(0.1f);      //��ٸ���
            int randint=Random.Range(0, waterEffect.Length);        //������ �ޱ�
            waterEffect[randint].SetActive(true);                   //�ش� ������ ������Ʈ Ȱ��ȭ
        }
    }

}

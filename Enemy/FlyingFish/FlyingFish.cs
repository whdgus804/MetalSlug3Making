using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingFish : EnemyHP
{
    /// <summary>
    /// �̵��ӵ��� ���� �ִϸ��̼� Ŀ��
    /// </summary>
    [SerializeField] AnimationCurve curve;
    /// <summary>
    /// ������ �ӵ�
    /// </summary>
    [SerializeField] float moveSpeed;
    /// <summary>
    /// ó�� ������ �� ���� Ƣ�� ���� ��
    /// </summary>
    [SerializeField] float upForce;
    /// <summary>
    /// �����Ҷ� ���� ����Ʈ
    /// </summary>
    [SerializeField] GameObject smallWaterEffect;
    /// <summary>
    /// ����Ʈ�� ������ġ �迭
    /// </summary>
    [SerializeField]
    Transform[] effectPos;
    /// <summary>
    /// �ִϸ�����
    /// </summary>
    Animator anim;
    /// <summary>
    /// ������ �ٵ�
    /// </summary>
    Rigidbody2D rigid;
    /// <summary>
    /// �ִϸ��̼� Ŀ�� ���� ������ ����
    /// </summary>
    float addSpeed=0.0f;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(LifeTime());
        rigid.AddForce(new Vector2(0, upForce), ForceMode2D.Impulse);   //���� Ƣ�� �������ϱ�
        StartCoroutine(Move());                                         //�������� �����ϴ� �ڷ�ƾ ����
        for (int i = 0; i < effectPos.Length; i++)
        {
            Instantiate(smallWaterEffect, effectPos[i].position, Quaternion.identity); //����Ʈ �����ϱ�
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth=collision.GetComponent<PlayerHealth>();   //�÷��̾�� ������ ����
            playerHealth.HP = 1;
        }
    }
    /// <summary>
    /// �ִϸ��̼� Ŀ�꿡 ���� ���� �� �����ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveAnimCurv()
    {
        float curvTime=0.0f;    //������ �� ����
        
        while (true)
        {
            yield return null;
            curvTime += Time.deltaTime;     //�ð� ����
            addSpeed=curve.Evaluate(curvTime);  //�ִϸ��̼� Ŀ�갪 ����
        }
    }
    /// <summary>
    /// �������� �����ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator Move()
    {
        for(int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            rigid.drag += 1;                            //���� �ε巴�� ���߱�
        }
        StartCoroutine(MoveAnimCurv());     //�ִϸ��̼�Ŀ�긦 ����ϴ� �ڷ�ƾ ����
        while (true)
        {
            yield return new WaitForFixedUpdate();
            transform.Translate(moveSpeed *addSpeed* Time.fixedDeltaTime, 0, 0);        //�������� �����̱�
        }
    }
    protected override void OnDie()
    {
        StopAllCoroutines();        //��� �ڷ�ƾ ����
        moveSpeed = 0.0f;           //�̵��ӵ� 0
        gameObject.layer = 10;      //�浹 ����
        rigid.drag = 0;             //���� ����
        rigid.gravityScale = 1.3f;  //������ �������� �ϱ�
        anim.SetTrigger("Dead");    //�ִϸ��̼� �� ����

    }
    protected override void Explosioned()
    {
        rigid.bodyType = RigidbodyType2D.Kinematic; //���ڸ� �����ϱ�
        gameObject.layer = 10;                      //�浹 ����
        moveSpeed = 0.0f;                           //������ ����
        StopAllCoroutines();                        //��� �ڷ�ƾ ����
        anim.SetTrigger("DeadExplosion");           //�ִϸ��̼� ������
        anim.SetTrigger("Dead");                    //�ִϸ��̼� ������
        
    }
    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(10.0f);
        Destroy(gameObject);
 
    }
}

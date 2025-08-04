using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Minkong : EnemyBulletBase
{
    /// <summary>
    /// y������ ������ �ӵ� 
    /// </summary>
    [SerializeField] float ySpeed;   
    /// <summary>
    /// �ִϸ��̼� Ŀ��
    /// </summary>
    [SerializeField] AnimationCurve curve;
    /// <summary>
    /// �Ѿ��� �����ð�
    /// </summary>
    [SerializeField] float lifeTime;
    /// <summary>
    /// Ȱ��ȭ �Ǿ��� �� �ۿ���� ���� ��
    /// </summary>
    [SerializeField] float force;
    /// <summary>
    /// ��ü�� ������ٵ� 
    /// </summary>
    Rigidbody2D rigid;

    CircleCollider2D circleCollider;

    float curTime=0.0f;
    protected override void Awake()
    {
        base.Awake();
        rigid= GetComponent<Rigidbody2D>();             //������ٵ� �ޱ�
        circleCollider=GetComponent<CircleCollider2D>();
    }
    private void OnEnable()
    {
        circleCollider.enabled = true;
        rigid.AddForce(new Vector2(transform.localScale.x*force, 0));       //���ý����� ���� ���� ���� Ȥ�� ���������� �߻� 
        StartCoroutine(ExplosionCoroutin());
    }

    private void FixedUpdate()
    {
        curTime += Time.deltaTime;                              //�ð� �߰�
        float y=curve.Evaluate(curTime);                        //�ִϸ��̼� Ŀ�꿡 ������ �� ����
        transform.Translate(0, ySpeed *y* Time.deltaTime, 0);   //ySpeed�� �ִϸ��̼� Ŀ�� ���� ������ y������ �̵�
    }
    protected override void HitEffect()
    {
        Explosion();
    }
    /// <summary>
    /// ���� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator ExplosionCoroutin()
    {
        yield return new WaitForSeconds(lifeTime);  //��ٸ���
        Explosion();
    }

    protected override void OnDie()
    {
        StopAllCoroutines();
        Explosion();
    }
    void Explosion()
    {
        ySpeed = 0.0f;                              //y�� �̵� ����
        circleCollider.enabled = false;
        rigid.drag = 1000;                          //x�� �̵� ����
        anim.SetTrigger("Explosion");               //���� �ִϸ��̼� ����
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Item_Crap : ItemBase
{
    [SerializeField] float disableTime;
    /// <summary>
    /// �̵��ӵ�
    /// </summary>
    [SerializeField] float moveSpeed;
    /// <summary>
    /// ���� �ð�
    /// </summary>
    [SerializeField] float stopMoveTime;
    /// <summary>
    /// �ּ� �̵� �ð�
    /// </summary>
    [SerializeField] float minStopMoveTime;
    /// <summary>
    /// ������ ƥ ��
    /// </summary>
    [SerializeField] float sideForce;
    /// <summary>
    /// ���� ƥ ��
    /// </summary>
    [SerializeField] float upforce;
    /// <summary>
    /// ������ٵ�
    /// </summary>
    Rigidbody2D rigid;
    /// <summary>
    /// �ִϸ�����
    /// </summary>
    Animator anim;
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();            //�ִϸ����� �ޱ�
        rigid=GetComponent<Rigidbody2D>();          //������ �ٵ� �ޱ�
    }

    private void OnEnable()
    {
        float randflot=Random.Range(-sideForce,sideForce);      //������ ƥ���� ���������� �ޱ�
        float randUp=Random.Range(upforce*0.8f,upforce);        //���� ƥ�� ���������� �ޱ�
        rigid.AddForce(new Vector2(randflot,randUp));           //����� ���������� Ƣ��
        StartCoroutine(StopMove());                             //������ ���� �ڷ�ƾ ����
        DisableTime(disableTime);
    }
    private void FixedUpdate()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    /// <summary>
    /// �������� ���� �� �ٽ� �̵��� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator StopMove()
    {
        float radnomFlot=Random.Range(minStopMoveTime, stopMoveTime);           //���� �� �ޱ�
        float setmoveSpeed = moveSpeed;                                         //�̵��ӵ� ����
        moveSpeed = 0.0f;                                                       //�̵� ����
        anim.SetFloat("Move", moveSpeed);                                       //�ִϸ����Ϳ� �� ����
        yield return new WaitForSeconds(1.0f);                                  //1�� ��ٸ���
        moveSpeed = setmoveSpeed;                                               //���� �̵��ӵ��� ����
        anim.SetFloat("Move", moveSpeed);                                       //�ִϸ����Ϳ� �� ����
        yield return new WaitForSeconds(radnomFlot);                            //���� �� ��ŭ ���߱�
        StartCoroutine(StopMove());                                             //�ٽ� �ش� �ڷ�ƾ ����
    }
}

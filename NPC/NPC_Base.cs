using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Base : MonoBehaviour
{
    /// <summary>
    /// Npc�� ����߸� ������
    /// </summary>
    [SerializeField]protected GameObject dropItem;
    ScoreManager scoreManager;
    /// <summary>
    /// �̵��ӵ�
    /// </summary>
    [SerializeField]protected float moveSpeed;

    /// <summary>
    /// �������� ����߸� ����
    /// </summary>
    [SerializeField]protected Transform itemPos;

    /// <summary>
    /// npc�� ���� �����ִ��� ��Ÿ���� ����true�� �����ִ���
    /// </summary>
    [SerializeField]protected bool tide = false;

    /// <summary>
    /// NPC �ִϸ�����
    /// </summary>
    protected Animator anim;
    /// <summary>
    /// NPC�� ������ �ٵ�
    /// </summary>
    protected Rigidbody2D rigid;

    private void Awake()
    {
        anim = GetComponent<Animator>();        //�ִϸ����� ����
        rigid = GetComponent<Rigidbody2D>();    //������ٵ� ����
        scoreManager=GameManager.Instance.ScoreManager;
    }

    protected virtual void OnEnable()
    {
        if (!tide)
        {
            StartCoroutine(Move());             //���� �������� ����� NPC �̵�
        }
        else
        {
            StartCoroutine(TideNPC());      //�����ִ� Ÿ���̸� Ǯ�� �������� ���
        }
    }
    /// <summary>
    /// �÷��̾��� ������ ���� ��� ����
    /// </summary>
    public virtual void Hit()
    {

    }
    /// <summary>
    /// ��� ���⶧ ���� �Լ�
    /// </summary>
    /// <param name="time">���� �ð�</param>
    protected void WaitMove(float time)
    {

        StopAllCoroutines();                //��� �ڷ�ƾ ����
        StartCoroutine(Wait(time));         //��� �ڷ�ƾ�� ���� �÷԰����� 
    }
    
    /// <summary>
    /// �������� �����ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator Wait(float time)
    {
        float setmoveSpeed = moveSpeed;
        moveSpeed = 0.0f;
        Debug.Log(setmoveSpeed);
        yield return new WaitForSeconds(time);      //�ð� ��ٸ���
        moveSpeed = setmoveSpeed;
        if (tide)                                   //�����ִ� ���¸�
        {
            StartCoroutine(TideNPC());              //���� �����ֱ�
        }
        else
        {
            StartCoroutine(Move());                //Ǯ���ִ� ���¿����� �̵�
        }

    }
    /// <summary>
    ///  �������� ���Ҷ� ������ �� �ִ��� Ȯ���ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator TideNPC()
    {
        yield return new WaitUntil(()=> !tide);
        scoreManager.prisonerCount++;
        WaitMove(0.5f);
    }
    /// <summary>
    /// ������ �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator Move()
    {
        while (true)
        {
            transform.Translate(moveSpeed*transform.localScale.x*Time.fixedDeltaTime, 0, 0);
            yield return new WaitForFixedUpdate();
            //yield return null;
        }
    }
    /// <summary>
    /// �÷��̾�� �������� �ִ� �Լ�
    /// </summary>
    protected virtual void DropItem()
    {

    }
    /// <summary>
    /// npc�� ���߿��� �������� �����Լ�
    /// </summary>
    public virtual void Fly()
    {
        if (!tide)
        {
            StopAllCoroutines();
        }
    }
    /// <summary>
    /// ���� �����Ҷ� �����Լ�
    /// </summary>
    public virtual void Landing()
    {
        if (!tide)
        {
            StartCoroutine(Move());
        }
    }
}

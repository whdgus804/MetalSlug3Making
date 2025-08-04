using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TideNPC : NPC_Base
{
    /// <summary>
    /// ������ �����ִ� Ÿ���̸� Ż���Ҷ� �ν��� ����������Ʈ
    /// </summary>
    [SerializeField] GameObject wood;
    /// <summary>
    /// �Ŵ޷��ִ� Ÿ���̸� Ǯ���� ���� ������Ʈ
    /// </summary>
    [SerializeField] GameObject rope;
    /// <summary>
    /// �÷��̾�� �������� ����� ��Ÿ���� ���� true�� ���� �ֱ���
    /// </summary>
    bool hasItem = true;
    /// <summary>
    /// NPC�� �����ִ� Ÿ��
    /// </summary>
    public enum Tide
    {
        Ground, 
        Wood,
        Hang
    }
    /// <summary>
    /// NPC�� �����ִ� Ÿ��
    /// </summary>
    public Tide tideTpye;

    /// <summary>
    /// �Ѿˤ��� ���� �ݶ��̴�
    /// </summary>
    [SerializeField] BoxCollider2D tideRopeCollider;



    protected override void OnEnable()
    {

        base.OnEnable();
        switch(tideTpye)                    //Ÿ�Կ����� �ִϸ��̼� ����
        {
            case Tide.Wood:
                anim.SetTrigger("Wood");
                break;
            case Tide.Hang:
                anim.SetTrigger("Hang");
                rigid.gravityScale = 0.0f;      //�����ִ� Ÿ���� �������� �ʰ� �߷°� ����
                rope.SetActive(true);           //�������� ���� Ȱ��ȭ
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!tide)              //Ǯ���� ���¿��� �÷��̾�� �������� �ش� 
        {
            if (collision.CompareTag("Player"))
            {
                if(hasItem)
                {
                    hasItem = false;
                    DropItem();             //�÷��̾ ������ ������ ���� �Լ� ����
                }
            }
            else if(collision.CompareTag("Trun"))       //�ڸ� ���� Ʈ���ſ� ������ �ڸ� ���ư���
            {
                //���� ������ ���� -1�� �����ش�
                if (hasItem)
                {

                    Vector3 vec = transform.localScale;
                    vec.x *= -1;
                    transform.localScale = vec;
                }
            }
        }
    }
    public override void Hit()
    {
        anim.SetTrigger("UnTide");                                          //Ǯ������ �ִϸ��̼� ���
        tide = false;                                                       //�����ִ� ���� ����
        switch (tideTpye)                                                   //Ÿ�Կ� ���� Ǯ������ ��Ȳ ����
        {
            case Tide.Wood:
                wood.transform.parent = null;                               //������ ���ڸ����� �������Բ� ����
                wood.SetActive(true);                                       //�ν����� ���� ����
                break;
            case Tide.Hang:                                                 //�Ŵ޷��ִ� ���¸�
                rope.transform.parent = null;                               //���� Ʈ������ ����
                GameObject hand = rope.transform.GetChild(0).gameObject;    //���� �ڽ����� �ִ� ���� Ǯ������ ������Ʈ �ޱ�
                hand.SetActive(true);                                       //���� Ǯ������ ������Ʈ Ȱ��ȭ
                Animator ropeAnim=rope.GetComponent<Animator>();            //���� �ִϸ����� �ޱ�
                ropeAnim.SetTrigger("Break");                               //���� �ִϸ��̼� ���
                Fly();                                                      //�Ŵ޷��ִ� ���¿��� Ǯ�������μ� �������� �Լ� ���
                break;
        }
        gameObject.layer = 11;                                              //�÷��̾�� �浹�����ϰԲ� ����
        tideRopeCollider.isTrigger = true;                                  //Ʈ���� �ѱ�

    }
    protected override void DropItem()
    {
        anim.SetBool("Item", true);
        gameObject.layer = 10;                                      //���� ���� �����ϸ� �ε����� �ʰ� ����
        moveSpeed *= 1.5f;                                          //�̵��ӵ� 2��
        WaitMove(2.7f);                                             //������ ���� ������
        StartCoroutine(ItemDropCoroutin());                         //������ ��� ������
    }
    public override void Fly()
    {
        base.Fly();
        anim.SetBool("OnGround", false);                            //���� ���� �ִϸ��̼� ���
        if (!tide)                                                  //Ǯ���� ���¸� 
            rigid.gravityScale = 0.5f;                              //õõ�� ��������
    }
    public override void Landing()
    {
        base.Landing();
        anim.SetBool("OnGround", true);                             //�ִϸ��̼� ����
    }
    /// <summary>
    /// �������� �����ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator ItemDropCoroutin()
    {
        yield return new WaitForSeconds(1.25f);
        Instantiate(dropItem, itemPos.position, Quaternion.identity);//������ ����
        transform.localScale = new Vector3(-1, 1, 1);
    }
}

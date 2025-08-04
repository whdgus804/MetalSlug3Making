using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    /// <summary>
    /// ������ �ٽ� ������ ���� ������
    /// </summary>
    [SerializeField] float nextSpawnDelay;
    /// <summary>
    /// ��ȯ�� ���� ������ ��ũ��Ʈ
    /// </summary>
    Vine_Morden morden;
    /// <summary>
    /// �ش� ��ü�� �ִϸ�����
    /// </summary>
    Animator anim;
    /// <summary>
    /// ������ Ÿ�� ���°��� ������ ����������Ʈ 2d
    /// </summary>
    SpringJoint2D springJoint;
    /// <summary>
    /// ���� ���� �����Ǿ��ִ��� ��Ÿ���� ���� true�� ���� ������ ����
    /// </summary>
    bool nowSpawn = false;
    
    private void Awake()
    {
        anim= GetComponent<Animator>();
        springJoint= GetComponent<SpringJoint2D>();
    }
    private void Start()
    {
        SpawnEnemyCheck();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&!nowSpawn)          //�÷��̾ Ʈ���ſ� ������
        {
            SpawnEnemy();                                       //�� ����
            morden.first = true;
            nowSpawn = true;                                    //�ߺ����� ���� ���� ����
        }
    }
    /// <summary>
    /// ��ȯ�� ���� �ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    void SpawnEnemyCheck()
    {
        morden=transform.GetChild(0).GetComponent<Vine_Morden>();       //�ڽ� ������Ʈ�� ��ũ��Ʈ�� �ִ��� Ȯ�� �� ����
        if(morden != null )                                             //������
        {
            if (nowSpawn)                                               //���� �� ���̶� ��ȯ�� ���¸� �� ��ȯ
                SpawnEnemy();
        }
    }
    /// <summary>
    /// ���� ��ȯ�ϴ� �Լ�
    /// </summary>
    void SpawnEnemy()
    {
        morden.gameObject.SetActive(true);                              //�� Ȱ��ȭ
        Rigidbody2D rigid=morden.GetComponent<Rigidbody2D>();           //������ �ٵ� �ޱ�
        springJoint.connectedBody = rigid;                              //������ ����Ʈ�� ������ �ٵ� ����
        anim.SetTrigger("Ride");                                        //�ִϸ��̼� ���
        StartCoroutine(Delay());                                        //���� ���� ����� ������ ���

    }
    /// <summary>
    /// �� ��ȯ�� �������� �� ������ �� �ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(nextSpawnDelay);
        SpawnEnemyCheck();          //���� �ִ��� Ȯ���ϴ� ���� ����
    }

}

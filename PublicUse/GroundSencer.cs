using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSencer : MonoBehaviour
{
    //�� Ȥ�� �÷��̾ ���� ���� ���� �ִ��� �ƴ� �������� �ִ��� Ȯ���� ��ũ��Ʈ (���� ������ ȣ��

    /// <summary>
    /// �÷��̾ �������� ���� ��ũ��Ʈ ��������
    /// </summary>
    PlayerHealth player;
    EnemyHP enemy;
    NPC_Base npc;
    /// <summary>
    /// �h�� ���� ���� ���� �ִ����� ��Ÿ���� ����true�� ���� ���� ����
    /// </summary>
    public bool onGround = false;

    /// <summary>
    /// ���� �ߺ����� Ʈ���ſ� ������ �� ���� ���� �ִµ� �����ϴ� ���� ����
    /// ���� ������ ���� �������� ����
    /// </summary>
    int groundCount = 0;

    public enum Type
    {
        player,
        enemy,
        NPC
    }
    public Type type;

    private void Awake()
    {
        switch (type)
        {
            case Type.player:
                player=GetComponentInParent<PlayerHealth>();
                break;
            case Type.enemy:
                enemy= GetComponentInParent<EnemyHP>();
                break;

            case Type.NPC:
                npc= GetComponentInParent<NPC_Base>();
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))                     //������ ���� ������
        {
            onGround = true;                                    //���� ����
            if (groundCount < 1)                                //���� ���߿� �־�����
            {
                Landing();                                      //���� �Լ� ����
            }
            groundCount++;                                      //ī��Ʈ ����
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))                     //������ ���� ������
        {
            groundCount--;                                      //���� ����
            if(groundCount < 1)                                 //���� ��� ������ ��������(�����̸�)
            {
                onGround = false;                                   //���� ����
                groundCount = 0;                                //Ȥ�� �� ���� ���� 
                Fly();                                          //���� �Լ� ����
            }
        }
    }
    /// <summary>
    /// ��ü�� ���߿��� ���� ó�� �������� ����Ǵ� �Լ�
    /// </summary>
    private void Landing()
    {
        switch (type)
        {
            case Type.player:
                player.Landing();
                break;
            case Type.enemy:
                enemy.Landing();
                break;

            case Type.NPC:
                npc.Landing();
                break;
        }
    }
    /// <summary>
    /// ��ü�� ������ �������� ����Ǵ� �Լ�
    /// </summary>
    private void Fly()
    {
        switch (type)
        {
            case Type.player:
                player.Fly();
                break;
            case Type.enemy:
                enemy.Fly();
                break;

            case Type.NPC:
                npc.Fly();
                break;
        }
    }
}

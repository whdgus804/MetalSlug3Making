using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    /// <summary>
    /// �ش� �÷����� ���ư� ���� 
    /// </summary>
    public Vector2 movingForce;
    /// <summary>
    /// �÷����� ������ �ӵ�
    /// </summary>
    public float moveSpeed;
    /// <summary>
    /// �ش� �÷����� ���� �������� �������� ��ũ��Ʈ ����Ʈ
    /// </summary>
    List<MovePlatformUse> movePlatformUses;
    /// <summary>
    /// ���� �÷��� ������ �����´� ���� true �� ���� �÷��� ��
    /// </summary>
    bool onplay = true;
    bool nowMoving=false;
    PlayerHealth player;
    private void Awake()
    {
        movePlatformUses = new List<MovePlatformUse>();
        player=GameManager.Instance.PlayerHealth;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GroundSencer"))                                               //GroundSencer�� ������ �ش� �θ� ������Ʈ�� MovePlatformUse��ũ��Ʈ�� ����Ʈ�� �߰�
        {
            MovePlatformUse movePlatformUse=collision.GetComponentInParent<MovePlatformUse>();      //��ũ��Ʈ �ޱ�
            movePlatformUses.Add(movePlatformUse);
            if (nowMoving)
            {
                movePlatformUse.MovePlatform(moveSpeed, movingForce);
            }
            //Debug.Log(collision.transform.parent.name);


            //����Ʈ�� �߰�
            //for(int i = 0; i < movePlatformUses.Count; i++)
            //{
            //    Debug.Log(movePlatformUses[i].gameObject.name);
            //}
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GroundSencer"))                                    //GroundSencer�� ������ ����Ʈ���� ���� �� ������ ����
        {
            if (onplay)                                                                                 //�÷��� ���̸� ���׹���
            {

                MovePlatformUse movePlatformUse = collision.GetComponentInParent<MovePlatformUse>();    //��ũ��Ʈ �ޱ�
                GroundSencer groundSencer = collision.GetComponent<GroundSencer>();
                if (groundSencer.type == GroundSencer.Type.player)
                {
                    movePlatformUse.StopAllCoroutines();

                }else if(groundSencer.type== GroundSencer.Type.enemy)
                {
                    EnemyHP enemyHP = collision.GetComponentInParent<EnemyHP>();
                    if(enemyHP!=null&&enemyHP.HP>1)
                    {
                        movePlatformUse.StopAllCoroutines();                                                    //������ ����

                    }
                }
                movePlatformUses.Remove(movePlatformUse);                                               //����Ʈ���� ����
                
            }
            //for (int i = 0; i < movePlatformUses.Count; i++)
            //{
            //    Debug.Log(movePlatformUses[i].gameObject.name);
            //}
        }
        
    }

    /// <summary>
    /// �÷����� ������ �Լ�
    /// </summary>
    /// <param name="moveSpeed"></param>
    void Move(float moveSpeed)
    {
        nowMoving = true;
        for(int i = 0; i < movePlatformUses.Count; i++)
        {
            movePlatformUses[i].MovePlatform(moveSpeed, movingForce);       //���� ����Ʈ�� �ִ� (�ش� �÷����� ��� �ִ�)������Ʈ�� �������� �����ϴ� �Լ� ����
        }
    }
    public void PlatformStart(float move)
    {
        
        StartCoroutine(PlatformStartCoroutine(move));
    }
    IEnumerator PlatformStartCoroutine(float move)
    {
        Move(move);
        while (true)
        {
            yield return new WaitForFixedUpdate();
            transform.Translate(movingForce*move*Time.deltaTime);
        }
    }
    /// <summary>
    /// �������� �����ϴ� �Լ�
    /// </summary>
    public void MoveStop()
    {
        nowMoving = false;
        for (int i = 0;i < movePlatformUses.Count; i++)
        {
            movePlatformUses[i].StopAllCoroutines();                      //���� ����Ʈ�� �ִ� (�ش� �÷����� ��� �ִ�)������Ʈ�� �������� �����ڷ�ƾ ��� ����
        }
        StopAllCoroutines();
    }

    private void OnApplicationQuit()
    {
        onplay = false;                                             //���� �÷��������� �ƴ����� ���� ����
    }
}

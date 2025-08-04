using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{

    //�÷��̾ ���� �Ӹ��� ������ Ƣ�������


    /// <summary>
    /// ���� �Ӹ� �浹�ݶ��̴�
    /// </summary>
    BoxCollider2D col;

    EnemyHP enemyHP;
    private void Awake()
    {
        col= GetComponent<BoxCollider2D>();     //�ݶ��̴� �ޱ�
        enemyHP=GetComponentInParent<EnemyHP>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GroundSencer"))                                                 //�÷��̾��
        {
            PlayerHealth player=collision.GetComponentInParent<PlayerHealth>();
            if(player != null && enemyHP.HP >0)
            {

                //GroundSencer groundSencer= collision.GetComponent<GroundSencer>();    //�÷��̾ ���������� �� �� �ְ� �׶��� ���� �ޱ�
                col.enabled = false;                                                        //���� �浹 ������ ���� �ݶ��̴� ��Ȱ��ȭ
                Rigidbody2D rigid=player.GetComponent<Rigidbody2D>();                    //addforce�� ���� �÷��̾��� ������ �ٵ� �ޱ�
                rigid.constraints |= RigidbodyConstraints2D.FreezePositionY;
                float distance = transform.position.x - collision.transform.position.x;     //�÷��̾ �����ʿ� �ִ��� ���ʿ� �ִ��� �Ǵ�
                float xForce = 2;                                                           //Ƣ����� ��


                rigid.constraints &=~RigidbodyConstraints2D.FreezePositionY;
                if (distance > 0)                                                           //�÷��̾ ���ʿ� �� ������
                {
                    rigid.AddForce(new Vector2(-xForce, 4), ForceMode2D.Impulse);           //�������� Ƣ��������ϱ�
                }
                else
                {
                    rigid.AddForce(new Vector2(xForce, 4), ForceMode2D.Impulse);            //���������� Ƣ������� �ϱ�
                }
                StartCoroutine(TriggerDelay());                                             //�ٽ� �ݶ��̴��� Ȱ��ȭ�ϴ� �ڷ�ƾ �Լ� 

            }
        }
    }
    /// <summary>
    /// �ݶ��̴��� �ٽ� Ȱ��ȭ��Ű�� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator TriggerDelay()
    {
        yield return new WaitForSeconds(0.1f);  //0.1�� ��ٸ���
        col.enabled = true;                     //�ݶ��̴� Ȱ��ȭ
    }
}

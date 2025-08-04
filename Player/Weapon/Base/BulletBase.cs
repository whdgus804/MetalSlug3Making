using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : RecycleObject
{
    /// <summary>
    /// �Ѿ��� �̵��ӵ�
    /// </summary>
    [SerializeField]protected float bulletMoveSpeed;
    /// <summary>
    /// �ǰݴ�󿡰� ���� ������
    /// </summary>
    [SerializeField] protected int damage;
    /// <summary>
    /// �浹���� ���̾�
    /// </summary>
    [SerializeField]LayerMask hitLayer;
    /// <summary>
    /// �Ѿ��� ���� �ð�
    /// </summary>
    [SerializeField] float lifeTime;
    /// <summary>
    /// �ε��� ������Ʈ�� ������ ����
    /// </summary>
    protected GameObject hitObj=null;
    protected virtual void OnEnable()
    {
        DisableTime(lifeTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((hitLayer &(1<<collision.gameObject.layer)) !=0)                    //�浹���� ���̾ �ε����ٸ�
        {
            
            //if (collision.gameObject.CompareTag("Ground"))                      // ���� �ε�����
            //{
            //    GrounHhit();
            //}
            //else if(collision.gameObject.CompareTag("NPC"))
            //{
            //    hitObj = collision.gameObject;
            //    NPCHit();
            //}
            //else
            //{
            //    hitObj = collision.gameObject;                                      //������Ʈ ����
            //    EnemyHP enemyHP=collision.gameObject.GetComponent<EnemyHP>();       //���� ü���� ���� ������Ʈ��(��, ���� �Ѿ�)
            //    if(enemyHP!=null)
            //    {
            //        EnemyHit();                                                     //�� ��Ʈ �Լ� ����

            //    }
            //}
            EnemyHP enemyHP = collision.gameObject.GetComponent<EnemyHP>();       //���� ü���� ���� ������Ʈ��(��, ���� �Ѿ�)
            if (enemyHP != null)
            {
                 hitObj = collision.gameObject;                                      //������Ʈ ����
                EnemyHit();

            }else if (collision.gameObject.CompareTag("Ground"))
            {
                GrounHhit();
            }else if (collision.gameObject.CompareTag("NPC"))
            {
                hitObj = collision.gameObject;
                NPCHit();
            }
        }
    }
    /// <summary>
    /// ���� �Ѿ��� ������ ����Ǵ� �Լ�
    /// </summary>
    protected virtual void GrounHhit()
    {
        //���� ��Ʈ
        Vector2 vector2 = Vector2.zero;                       //��ġ�� ���� ���Ͱ� ���� �� �ʱ�ȭ
        if (transform.eulerAngles.z > 260.0f)               //�ٴ��� ���� �߻��� �Ѿ��̸�
        {
            vector2 = EffectPos(false, true);               //x�ุ ���������� ����

            vector2 += (Vector2)transform.position;         //�������� �Ѿ˰� ������ �ε��� ��ġ ���ϱ�
            PoolFactory.Instance.GetGroundHitDown(vector2); //���� �� ��ġ�� ����Ʈ ����

        }
        else                                                //���� �����Ÿ�
        {
            vector2 = EffectPos(true, false);               //y�ุ �������� �ޱ�
            vector2.x += 0.5f * transform.localScale.x;     //���� ��ġ���� ���� �������� ����
            vector2 += (Vector2)transform.position;         //���� ��ġ ���ϱ�
            PoolFactory.Instance.GetGroundHitSide(vector2, Vector3.zero, transform.localScale); //���� ��ġ�� ���ý����ϰ��� ������ ����Ʈ ���� 
        }
        gameObject.SetActive(false);                        //������Ʈ ��Ȱ��ȭ
    }
    /// <summary>
    /// ���� �Ѿ˿� ������ ����Ǵ� �Լ�
    /// </summary>
    protected virtual void EnemyHit()
    {
        EnemyHP enemyHP = hitObj.GetComponent<EnemyHP>();     //�ǰݵ� ���� hp��ũ��Ʈ ��������
        enemyHP.HP = damage;                                //������ �ֱ�
        Vector2 vector2 = EffectPos(true, true);            //���� ��ġ�ޱ�
        vector2 += (Vector2)transform.position;             //������ġ�� ���� ��ġ ���ϱ�
        PoolFactory.Instance.GetBulletHitEffect(vector2);   //��Ʈ ����Ʈ ����
        gameObject.SetActive(false);                        //������Ʈ ��Ȱ��ȭ
    }
    protected virtual void NPCHit()
    {
        NPC_Base npc = hitObj.GetComponent<NPC_Base>();
        npc.Hit();
        Vector2 vector2 = EffectPos(true, true);            //���� ��ġ�ޱ�
        vector2 += (Vector2)transform.position;             //������ġ�� ���� ��ġ ���ϱ�
        PoolFactory.Instance.GetBulletHitEffect(vector2);   //��Ʈ ����Ʈ ����
        gameObject.SetActive(false);                        //������Ʈ ��Ȱ��ȭ
    }
    /// <summary>
    /// �Ѿ��� �浹 �� ����Ʈ�� ���涧 ���� ������������ �翷 Ȥ�� �� �Ʒ��� ���ݾ� �̵��ϴ� Vector2��ȯ �Լ�
    /// </summary>
    /// <param name="upDown">y��</param>
    /// <param name="side">x��</param>
    /// <returns></returns>
    protected virtual Vector2 EffectPos(bool upDown,bool side)
    {
        Vector2 vector2= Vector2.zero;                      //��ȯ�� ����
        float randnum = Random.Range(-0.5f, 0.6f);          //���� ��ġ ����
        if (upDown)                                         //upDown�� ���̸�
        {
            vector2.y += randnum;                           //y�� �� ������ ���ϱ�
        }
        randnum = Random.Range(-0.5f, 0.6f);          //���� ��ġ ����
        if (side)                                           //�翷�� ���̸�
        {
            vector2.x += randnum;                           //x�� ������ ���ϱ�
        }

        return vector2;         //�� ��ȯ
    }
}

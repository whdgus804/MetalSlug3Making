using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_3Mssile : EnemyBulletBase
{
    /// <summary>
    /// �ε��� ���̾�
    /// </summary>
    [SerializeField] LayerMask hitLayer;
    /// <summary>
    /// ���� ��ġ
    /// </summary>
    [SerializeField] Transform explosionPos;
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("Ground"))
        {
            GroundHit();                                    //��/�迡 �ε����� �����ϴ� �Լ�����
        }
    }
    protected override void HitEffect()
    {
        PoolFactory.Instance.Get_N_Explosion_S(transform.position);         // ���� ����
        Destroy(gameObject);                                                //������Ʈ ����
    }
    /// <summary>
    /// �̻����� �ٴڿ� ������ ������ ��ó�� �÷��̾�� ������� �ִ� �Լ�
    /// </summary>
    void GroundHit()
    {
        Collider2D collider2=Physics2D.OverlapBox(explosionPos.position,new Vector2(2,1),0,hitLayer);       //��ó�� �÷��̾ �ִ��� Ȯ��
        if(collider2!=null)             //������
        {       
            hitobj=collider2.gameObject;        //���� ����
            PlayerHit();                        //�÷��̾�� �������� �ִ� �Լ� ����
        }       
        
        HitEffect();                            //���� �Լ� ����
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector2 vec = new Vector2(2, 1);
        if(explosionPos != null)
            Gizmos.DrawWireCube(explosionPos.position,vec);
    }
    protected override void OnDie()
    {
        HitEffect();
    }
}

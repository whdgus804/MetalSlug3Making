using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBase : RecycleObject
{
    /// <summary>
    /// ����� �����
    /// </summary>
    [SerializeField] int damage;
    /// <summary>
    /// ������ ������ �� �� �ִ�  ���̾� ��NPc����
    /// </summary>
    [SerializeField] LayerMask canDamageLayer;

    /// <summary>
    /// ���߹����� �߾�
    /// </summary>
    [Space(20.0f)]
    [SerializeField] Transform explosionCenter;
    /// <summary>
    /// ���߹���
    /// </summary>
    [SerializeField] Vector2 ExplosionRang;

    AmmoCounter ammoCounter;
    protected virtual void Awake()
    {
        ammoCounter = GameManager.Instance.AmmoCounter;
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))                      //�翡 �ε�����
        {
            GroundHit();                                                    //���� �ε����� �Լ� ����
        }
        else if((canDamageLayer & (1 << collision.gameObject.layer)) != 0)  //����� ���̾ ���� �ݶ��̴��� �浹�ϸ�
        {
            Explosion();                                                    //�ٷ� ����
        }
    }

    /// <summary>
    /// ���� ����� �� ����Ǵ� �Լ�
    /// </summary>
    protected virtual void GroundHit()
    {

    }

    /// <summary>
    /// �����Լ�
    /// </summary>
    protected virtual void Explosion()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(explosionCenter.position, ExplosionRang, 0, canDamageLayer);
        for(int i = 0; i < hitColliders.Length; i++)
        {
            //Debug.Log(hitColliders[i].name);
            if (!hitColliders[i].CompareTag("NPC"))
            {
                EnemyHP enemyHP= hitColliders[i].GetComponent<EnemyHP>();
                if (enemyHP != null)
                {
                    enemyHP.GrenadeHit(damage);
                }
            }
            else
            {
                NPC_Base npc=hitColliders[i].GetComponent<NPC_Base>();
                npc.Hit();
            }
        }
        
        
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        if (ammoCounter.grenadeCount < 2)
        {
            ammoCounter.grenadeCount++;
        }
    }

    private void OnDrawGizmos()
    {
        if (explosionCenter == null)
        {
            explosionCenter = transform;
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(explosionCenter.position, ExplosionRang);
    }
}

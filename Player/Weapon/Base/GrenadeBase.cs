using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBase : RecycleObject
{
    /// <summary>
    /// 적용될 대미지
    /// </summary>
    [SerializeField] int damage;
    /// <summary>
    /// 데미지 적용을 할 수 있는  레이어 만NPc포함
    /// </summary>
    [SerializeField] LayerMask canDamageLayer;

    /// <summary>
    /// 폭발범위의 중앙
    /// </summary>
    [Space(20.0f)]
    [SerializeField] Transform explosionCenter;
    /// <summary>
    /// 폭발범위
    /// </summary>
    [SerializeField] Vector2 ExplosionRang;

    AmmoCounter ammoCounter;
    protected virtual void Awake()
    {
        ammoCounter = GameManager.Instance.AmmoCounter;
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))                      //당에 부딪히면
        {
            GroundHit();                                                    //땅에 부딪히는 함수 실행
        }
        else if((canDamageLayer & (1 << collision.gameObject.layer)) != 0)  //저장된 레이어가 가진 콜라이더와 충돌하면
        {
            Explosion();                                                    //바로 폭발
        }
    }

    /// <summary>
    /// 땅에 닿았을 때 실행되는 함수
    /// </summary>
    protected virtual void GroundHit()
    {

    }

    /// <summary>
    /// 폭발함수
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

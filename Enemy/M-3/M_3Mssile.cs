using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_3Mssile : EnemyBulletBase
{
    /// <summary>
    /// 부딪힐 레이어
    /// </summary>
    [SerializeField] LayerMask hitLayer;
    /// <summary>
    /// 폭발 위치
    /// </summary>
    [SerializeField] Transform explosionPos;
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("Ground"))
        {
            GroundHit();                                    //땅/배에 부딪히면 폭발하는 함수실행
        }
    }
    protected override void HitEffect()
    {
        PoolFactory.Instance.Get_N_Explosion_S(transform.position);         // 폭발 생성
        Destroy(gameObject);                                                //오브젝트 삭제
    }
    /// <summary>
    /// 미사일이 바닥에 닿을때 폭발해 근처에 플레이어에게 대미지를 주는 함수
    /// </summary>
    void GroundHit()
    {
        Collider2D collider2=Physics2D.OverlapBox(explosionPos.position,new Vector2(2,1),0,hitLayer);       //근처에 플레이어가 있는지 확인
        if(collider2!=null)             //있으면
        {       
            hitobj=collider2.gameObject;        //변수 저장
            PlayerHit();                        //플레이어에게 데미지를 주는 함수 실행
        }       
        
        HitEffect();                            //폭발 함수 실행
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

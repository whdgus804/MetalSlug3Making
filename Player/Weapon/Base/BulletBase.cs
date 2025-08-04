using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : RecycleObject
{
    /// <summary>
    /// 총알의 이동속도
    /// </summary>
    [SerializeField]protected float bulletMoveSpeed;
    /// <summary>
    /// 피격대상에게 입힐 데미지
    /// </summary>
    [SerializeField] protected int damage;
    /// <summary>
    /// 충돌가능 레이어
    /// </summary>
    [SerializeField]LayerMask hitLayer;
    /// <summary>
    /// 총알의 생존 시간
    /// </summary>
    [SerializeField] float lifeTime;
    /// <summary>
    /// 부딪힌 오브젝트를 저장할 변수
    /// </summary>
    protected GameObject hitObj=null;
    protected virtual void OnEnable()
    {
        DisableTime(lifeTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((hitLayer &(1<<collision.gameObject.layer)) !=0)                    //충돌가능 레이어에 부딪혔다면
        {
            
            //if (collision.gameObject.CompareTag("Ground"))                      // 땅에 부딪히면
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
            //    hitObj = collision.gameObject;                                      //오브젝트 저장
            //    EnemyHP enemyHP=collision.gameObject.GetComponent<EnemyHP>();       //적의 체력을 갖는 오브젝트면(적, 적의 총알)
            //    if(enemyHP!=null)
            //    {
            //        EnemyHit();                                                     //적 히트 함수 실행

            //    }
            //}
            EnemyHP enemyHP = collision.gameObject.GetComponent<EnemyHP>();       //적의 체력을 갖는 오브젝트면(적, 적의 총알)
            if (enemyHP != null)
            {
                 hitObj = collision.gameObject;                                      //오브젝트 저장
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
    /// 땅에 총알이 닿으면 실행되는 함수
    /// </summary>
    protected virtual void GrounHhit()
    {
        //땅에 히트
        Vector2 vector2 = Vector2.zero;                       //위치를 더할 백터값 생성 및 초기화
        if (transform.eulerAngles.z > 260.0f)               //바닥을 향해 발사한 총알이면
        {
            vector2 = EffectPos(false, true);               //x축만 랜덤값으로 받음

            vector2 += (Vector2)transform.position;         //랜덤값에 총알과 땅과의 부딪힌 위치 더하기
            PoolFactory.Instance.GetGroundHitDown(vector2); //더한 값 위치에 이펙트 생성

        }
        else                                                //벽에 맞은거면
        {
            vector2 = EffectPos(true, false);               //y축만 랜덤으로 받기
            vector2.x += 0.5f * transform.localScale.x;     //맞은 위치보다 좀더 벽쪽으로 들어가기
            vector2 += (Vector2)transform.position;         //현재 위치 더하기
            PoolFactory.Instance.GetGroundHitSide(vector2, Vector3.zero, transform.localScale); //현재 위치에 로컬스케일값을 참조로 이펙트 생성 
        }
        gameObject.SetActive(false);                        //오브젝트 비활성화
    }
    /// <summary>
    /// 적이 총알에 맞으면 실행되는 함수
    /// </summary>
    protected virtual void EnemyHit()
    {
        EnemyHP enemyHP = hitObj.GetComponent<EnemyHP>();     //피격된 적의 hp스크립트 가져오기
        enemyHP.HP = damage;                                //데미지 주기
        Vector2 vector2 = EffectPos(true, true);            //랜덤 위치받기
        vector2 += (Vector2)transform.position;             //랜덤위치에 맞은 위치 더하기
        PoolFactory.Instance.GetBulletHitEffect(vector2);   //히트 이펙트 생성
        gameObject.SetActive(false);                        //오브젝트 비활성화
    }
    protected virtual void NPCHit()
    {
        NPC_Base npc = hitObj.GetComponent<NPC_Base>();
        npc.Hit();
        Vector2 vector2 = EffectPos(true, true);            //랜덤 위치받기
        vector2 += (Vector2)transform.position;             //랜덤위치에 맞은 위치 더하기
        PoolFactory.Instance.GetBulletHitEffect(vector2);   //히트 이펙트 생성
        gameObject.SetActive(false);                        //오브젝트 비활성화
    }
    /// <summary>
    /// 총알이 충돌 후 이펙트를 남길때 맞은 지점기준으로 양옆 혹은 위 아레로 조금씩 이동하는 Vector2반환 함수
    /// </summary>
    /// <param name="upDown">y축</param>
    /// <param name="side">x축</param>
    /// <returns></returns>
    protected virtual Vector2 EffectPos(bool upDown,bool side)
    {
        Vector2 vector2= Vector2.zero;                      //반환갑 생성
        float randnum = Random.Range(-0.5f, 0.6f);          //랜덤 위치 지정
        if (upDown)                                         //upDown이 참이면
        {
            vector2.y += randnum;                           //y축 에 랜덤값 더하기
        }
        randnum = Random.Range(-0.5f, 0.6f);          //랜덤 위치 지정
        if (side)                                           //양옆이 참이면
        {
            vector2.x += randnum;                           //x에 랜덤값 더하기
        }

        return vector2;         //값 반환
    }
}

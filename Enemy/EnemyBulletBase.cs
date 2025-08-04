using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBase : EnemyHP
{
    /// <summary>
    /// 총알의 애니메이터
    /// </summary>
    protected Animator anim;
    /// <summary>
    /// 히트한 오브젝트를 저장할 변수
    /// </summary>
    protected GameObject hitobj = null;
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();            //애니메이터 지정
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))                              //만약 플레이어면
        {
            hitobj= collision.gameObject;                                           //오브젝트에 저장
            PlayerHit();                                                            //히트 함수 실행
        }
       
    }
    /// <summary>
    /// 플레이어가 해당 총알을 맞았을때 실행되는 함수
    /// </summary>
    protected virtual void PlayerHit()
    {
        if (hitobj.activeSelf == true)                                              //해당 객체가 없어지지 않았다면
        {
            PlayerHealth player=hitobj.GetComponent<PlayerHealth>();                //플레이어 체력 받아오기
            player.HP = 1;                                                          //데미지 주기
            HitEffect();                                                            //이펙트 사용
        }
    }
    /// <summary>
    /// 총알이 플레이어에 맞아 없어질때 나오는 이펙트 기본 비활성화
    /// </summary>
    protected virtual void HitEffect()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 총알의 생존시간
    /// </summary>
    /// <param name="time">생존할 시간</param>
    protected void DisableTimer(float time)
    {
        StartCoroutine(DisableTimerCoroutin(time));         //코루틴 실행
    }
    /// <summary>
    /// 생존시간 후 객체를 비활성화할 코루틴 함수
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator DisableTimerCoroutin(float time)
    {
        yield return new WaitForSeconds(time);          //시간 기다리기
        gameObject.SetActive(false);                    //객체 비활성화
    }
}

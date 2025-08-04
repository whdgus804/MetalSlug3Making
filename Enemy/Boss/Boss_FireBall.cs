using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FireBall : MonoBehaviour
{
    /// <summary>
    /// 이동속도
    /// </summary>
    [SerializeField] float moveSpeed;
    /// <summary>
    /// 발사 이펙트 오브젝트
    /// </summary>
    [SerializeField] GameObject effect;
    /// <summary>
    /// 폭발이펙트
    /// </summary>
    [SerializeField] GameObject hugeExplosion;
    /// <summary>
    /// 플레이어
    /// </summary>
    PlayerHealth player;
    /// <summary>
    /// 리지드바디
    /// </summary>
    Rigidbody2D rigid;

    private void Awake()
    {
        player = GameManager.Instance.PlayerHealth;
        rigid=GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        
        rigid.AddForce(Vector2.right*moveSpeed);        //총알 발사
        StartCoroutine(FireEffect());                   //발사 이펙트
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Target"))
        {
            StartCoroutine(FireBallFall());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.HP = 1;
            Explosion();
        }else if (collision.gameObject.CompareTag("Ground"))
        {
            Explosion();
        }
    }
    /// <summary>
    /// 폭발 함수 
    /// </summary>
    void Explosion()
    {
        //폭발 애니메이션
        Vector2 vector2 = transform.position;                       //위치 받기
        vector2.y += 1.7f;                                          //위치값 조정
        Instantiate(hugeExplosion, vector2, Quaternion.identity);   //폭발 이펙트
        Destroy(gameObject);                                        //삭제
    }
    /// <summary>
    /// 파이어볼이 떨어지는 코루틴 함수 
    /// </summary>
    /// <returns></returns>
    IEnumerator FireBallFall()
    {
        rigid.drag = 3;                             //저항추가
        yield return new WaitForSeconds(0.15f);     //기다리기
        rigid.gravityScale = 3.0f;                  //밑으로 떨어지기
    }
    /// <summary>
    ///  뒤에 따라다니는 이펙트 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FireEffect()
    {
        WaitForSeconds wait=new WaitForSeconds(0.2f);
        for (int i = 0; i<200; i++)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            yield return wait;
        }
    }
}

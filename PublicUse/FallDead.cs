using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDead : MonoBehaviour
{
    ///// <summary>
    ///// 플레이어가 부활할 위치
    ///// </summary>
    [SerializeField] Transform spawnPos;
    /// <summary>
    /// 플레이어가 물에빠지면 사용할 이펙트
    /// </summary>
    [SerializeField] GameObject waterEffect;

    

    /// <summary>
    /// 물이 위로 튀어오르는듯한 이펙트
    /// </summary>
    [SerializeField] GameObject waterExplosionEffect;
    /// <summary>
    /// 위로 튀어오를  이펙트의 생성 높이
    /// </summary>
    [SerializeField] Transform waterEffectPos;
    BoxCollider2D boxCollider2D;
    PlayerCameraMove playerCamera;
    private void Awake()
    {
        playerCamera = FindAnyObjectByType<PlayerCameraMove>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            Vector2 vec = new Vector2(collision.transform.position.x, -4.63f);
            PlayerHealth health = collision.gameObject.GetComponent<PlayerHealth>();    //위치지정을 위해 스크립트 받기
            if (health.HP > 0)
            {
                GameObject water=Instantiate(waterEffect,vec,Quaternion.identity);
                PlayerAnimation anim=collision.gameObject.GetComponent<PlayerAnimation>();
                Rigidbody2D rigid=collision.gameObject.GetComponent<Rigidbody2D>();
                health.spanwPosition = spawnPos;                          //위치 전달
                anim.Dead_Water();
                health.HP = 100;
                playerCamera.CameraStayHere();
                boxCollider2D.enabled = false;
                StartCoroutine(ColliderDisabler(rigid,water));
            }
            else
            {
                Instantiate(waterExplosionEffect, new Vector2(collision.transform.position.x, waterEffectPos.position.y), Quaternion.identity);
            }
        }
        else
        {
            collision.gameObject.SetActive(false);  
            Instantiate(waterExplosionEffect, new Vector2(collision.transform.position.x, waterEffectPos.position.y), Quaternion.identity);
        }
    }
    IEnumerator ColliderDisabler(Rigidbody2D rigid,GameObject water)
    {
        rigid.AddForce(Vector2.up * 200.0f);
        yield return new WaitForSeconds(0.7f);
        Animator anim=water.GetComponent<Animator>();
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("Sink");
        boxCollider2D.enabled=true;
    }
}

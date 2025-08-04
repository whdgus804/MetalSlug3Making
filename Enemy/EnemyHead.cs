using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{

    //플레이어가 적의 머리를 밟으면 튀어오르기


    /// <summary>
    /// 적의 머리 충돌콜라이더
    /// </summary>
    BoxCollider2D col;

    EnemyHP enemyHP;
    private void Awake()
    {
        col= GetComponent<BoxCollider2D>();     //콜라이더 받기
        enemyHP=GetComponentInParent<EnemyHP>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GroundSencer"))                                                 //플레이어면
        {
            PlayerHealth player=collision.GetComponentInParent<PlayerHealth>();
            if(player != null && enemyHP.HP >0)
            {

                //GroundSencer groundSencer= collision.GetComponent<GroundSencer>();    //플레이어가 점프중인지 알 수 있게 그라운드 센서 받기
                col.enabled = false;                                                        //연속 충돌 방지를 위한 콜라이더 비활성화
                Rigidbody2D rigid=player.GetComponent<Rigidbody2D>();                    //addforce를 위한 플레이어의 리지드 바디 받기
                rigid.constraints |= RigidbodyConstraints2D.FreezePositionY;
                float distance = transform.position.x - collision.transform.position.x;     //플레이어가 오른쪽에 있는지 왼쪽에 있는지 판단
                float xForce = 2;                                                           //튀어오를 힘


                rigid.constraints &=~RigidbodyConstraints2D.FreezePositionY;
                if (distance > 0)                                                           //플레이어가 왼쪽에 더 가까우면
                {
                    rigid.AddForce(new Vector2(-xForce, 4), ForceMode2D.Impulse);           //왼쪽으로 튀어오르게하기
                }
                else
                {
                    rigid.AddForce(new Vector2(xForce, 4), ForceMode2D.Impulse);            //오른쪽으로 튀어오르게 하기
                }
                StartCoroutine(TriggerDelay());                                             //다시 콜라이더를 활성화하는 코루틴 함수 

            }
        }
    }
    /// <summary>
    /// 콜라이더를 다시 활성화시키는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator TriggerDelay()
    {
        yield return new WaitForSeconds(0.1f);  //0.1초 기다리기
        col.enabled = true;                     //콜라이더 활성화
    }
}

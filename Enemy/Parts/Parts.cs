using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parts : MonoBehaviour
{
    /// <summary>
    /// 최소 위로 튈 힘
    /// </summary>
    [SerializeField] float minUpAddForce;
    /// <summary>
    /// 최소 옆으로 튈힘
    /// </summary>
    [SerializeField] float minSideAddForce;
    /// <summary>
    /// 위로 튀어오를 힘
    /// </summary>
    [SerializeField] float upAddForce;
    /// <summary>
    /// 옆으로 튕겨질힘
    /// </summary>
    [SerializeField] float sideAddForce;
    /// <summary>
    /// 애니메이터 컴포넌트
    /// </summary>
    Animator anim;
    /// <summary>
    /// 해당 객체의 리지드바디
    /// </summary>
    Rigidbody2D rigid;
    
    /// <summary>
    /// 객체가 땅에닿으면 다음 애니메이션이 실행되게 할때 전달할 변수
    /// </summary>
    int ground_to_Hash = Animator.StringToHash("Ground");
    private void Awake()
    {
        anim = GetComponent<Animator>();        //컴포넌트 받기
        rigid=GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetTrigger(ground_to_Hash);
        }
    
    }
    private void OnEnable()
    {
        float upforerand=Random.Range(minSideAddForce, upAddForce);
        float sideforceRand=Random.Range(minSideAddForce, sideAddForce);
        rigid.AddForce(new Vector2(sideforceRand * transform.localScale.x, upforerand));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Bone : ItemBase
{
    /// <summary>
    /// 아이템의 생존시간
    /// </summary>
    [SerializeField] float lifeTime;

    /// <summary>
    /// 아이템의 애니메이터
    /// </summary>
    Animator anim;
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();    
    }
    private void OnEnable()
    {
        DisableTime(lifeTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            anim.SetTrigger("Ground");
        }
    }
}

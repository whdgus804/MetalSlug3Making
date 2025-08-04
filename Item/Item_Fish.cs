using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Fish : ItemBase
{
    [SerializeField] Transform waterEffectPos;
    /// <summary>
    /// 아이템의 유지시간
    /// </summary>
    [SerializeField] float disableTime;
    /// <summary>
    /// 튀어오르는 이펙트
    /// </summary>
    [SerializeField] GameObject waterEffect;
    /// <summary>
    /// 위로 튀어오르는 힘
    /// </summary>
    [SerializeField] float upForce;
    /// <summary>
    /// 옆으로 튀는 힘
    /// </summary>
    [SerializeField] float sideForce;
    /// <summary>
    /// 리지드바디
    /// </summary>
    Rigidbody2D rigid;
    protected override void Awake()
    {
        base.Awake(); 
        rigid = GetComponent<Rigidbody2D>();        //리지드 바디 받기
    }
    private void OnEnable()
    {
        if(waterEffect != null)
            Instantiate(waterEffect, waterEffectPos.position, Quaternion.identity);          //이펙트 생성
        rigid.AddForce(new Vector2(sideForce, upForce));                            //튀어오르기
        DisableTime(disableTime);                                                   //일정 후 아이템이 없어지는 함수 실행
    }
}

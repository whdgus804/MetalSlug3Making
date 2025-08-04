using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TideNPC : NPC_Base
{
    /// <summary>
    /// 나무에 묶여있는 타입이면 탈출할때 부숴질 나무오브젝트
    /// </summary>
    [SerializeField] GameObject wood;
    /// <summary>
    /// 매달려있는 타입이면 풀어질 밧줄 오브젝트
    /// </summary>
    [SerializeField] GameObject rope;
    /// <summary>
    /// 플레이어에게 아이템을 줬는지 나타내는 변수 true면 아직 주기전
    /// </summary>
    bool hasItem = true;
    /// <summary>
    /// NPC가 묶여있는 타입
    /// </summary>
    public enum Tide
    {
        Ground, 
        Wood,
        Hang
    }
    /// <summary>
    /// NPC가 묶여있는 타입
    /// </summary>
    public Tide tideTpye;

    /// <summary>
    /// 총알ㅇ을 맞을 콜라이더
    /// </summary>
    [SerializeField] BoxCollider2D tideRopeCollider;



    protected override void OnEnable()
    {

        base.OnEnable();
        switch(tideTpye)                    //타입에따른 애니메이션 변경
        {
            case Tide.Wood:
                anim.SetTrigger("Wood");
                break;
            case Tide.Hang:
                anim.SetTrigger("Hang");
                rigid.gravityScale = 0.0f;      //묵여있는 타입은 떨어지지 않게 중력값 변경
                rope.SetActive(true);           //묶여있을 로프 활성화
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!tide)              //풀려난 상태에서 플레이어에게 아이템을 준다 
        {
            if (collision.CompareTag("Player"))
            {
                if(hasItem)
                {
                    hasItem = false;
                    DropItem();             //플레이어에 닿으면 아이템 전달 함수 실행
                }
            }
            else if(collision.CompareTag("Trun"))       //뒤를 도는 트리거에 닿으면 뒤를 돌아간다
            {
                //로컬 스케일 값에 -1을 곱해준다
                if (hasItem)
                {

                    Vector3 vec = transform.localScale;
                    vec.x *= -1;
                    transform.localScale = vec;
                }
            }
        }
    }
    public override void Hit()
    {
        anim.SetTrigger("UnTide");                                          //풀려나는 애니메이션 재생
        tide = false;                                                       //묶여있는 변수 변경
        switch (tideTpye)                                                   //타입에 따라 풀려나는 상황 변경
        {
            case Tide.Wood:
                wood.transform.parent = null;                               //나무가 재자리에서 무숴지게끔 변경
                wood.SetActive(true);                                       //부숴지는 나무 생성
                break;
            case Tide.Hang:                                                 //매달려있는 상태면
                rope.transform.parent = null;                               //로프 트랜스폼 변경
                GameObject hand = rope.transform.GetChild(0).gameObject;    //로프 자식으로 있는 손이 풀어지는 오브젝트 받기
                hand.SetActive(true);                                       //손이 풀어지는 오브젝트 활성화
                Animator ropeAnim=rope.GetComponent<Animator>();            //로프 애니메이터 받기
                ropeAnim.SetTrigger("Break");                               //로프 애니메이션 재생
                Fly();                                                      //매달려있는 상태에서 풀려남으로서 떨어지는 함수 재생
                break;
        }
        gameObject.layer = 11;                                              //플레이어와 충돌가능하게끔 변경
        tideRopeCollider.isTrigger = true;                                  //트리거 켜기

    }
    protected override void DropItem()
    {
        anim.SetBool("Item", true);
        gameObject.layer = 10;                                      //벽과 땅을 제외하면 부딪히지 않게 변경
        moveSpeed *= 1.5f;                                          //이동속도 2배
        WaitMove(2.7f);                                             //아이템 전달 딜레이
        StartCoroutine(ItemDropCoroutin());                         //아이템 드랍 딜레이
    }
    public override void Fly()
    {
        base.Fly();
        anim.SetBool("OnGround", false);                            //공중 나는 애니메이션 재생
        if (!tide)                                                  //풀려난 상태면 
            rigid.gravityScale = 0.5f;                              //천천히 떨어지기
    }
    public override void Landing()
    {
        base.Landing();
        anim.SetBool("OnGround", true);                             //애니메이션 변경
    }
    /// <summary>
    /// 아이템을 생성하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator ItemDropCoroutin()
    {
        yield return new WaitForSeconds(1.25f);
        Instantiate(dropItem, itemPos.position, Quaternion.identity);//아이템 생성
        transform.localScale = new Vector3(-1, 1, 1);
    }
}

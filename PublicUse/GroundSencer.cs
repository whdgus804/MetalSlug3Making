using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSencer : MonoBehaviour
{
    //적 혹은 플레이어가 땅에 발을 디디고 있는지 아님 떨어지고 있는지 확인할 스크립트 (이하 센서로 호명

    /// <summary>
    /// 플레이어가 공통으로 가질 스크립트 가져오기
    /// </summary>
    PlayerHealth player;
    EnemyHP enemy;
    NPC_Base npc;
    /// <summary>
    /// 햔재 발을 땅에 디디고 있는지를 나타내는 변수true면 땅에 디디고 있음
    /// </summary>
    public bool onGround = false;

    /// <summary>
    /// 땅이 중복으로 트리거에 들어왔을 때 땅에 디디고 있는데 착지하는 것을 방지
    /// 땅에 닿으면 증가 떨어지면 감소
    /// </summary>
    int groundCount = 0;

    public enum Type
    {
        player,
        enemy,
        NPC
    }
    public Type type;

    private void Awake()
    {
        switch (type)
        {
            case Type.player:
                player=GetComponentInParent<PlayerHealth>();
                break;
            case Type.enemy:
                enemy= GetComponentInParent<EnemyHP>();
                break;

            case Type.NPC:
                npc= GetComponentInParent<NPC_Base>();
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))                     //센서가 땅에 닿으면
        {
            onGround = true;                                    //변수 변경
            if (groundCount < 1)                                //만약 공중에 있었으면
            {
                Landing();                                      //착지 함수 실행
            }
            groundCount++;                                      //카운트 증가
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))                     //센서가 땅에 닿으면
        {
            groundCount--;                                      //뱐수 차감
            if(groundCount < 1)                                 //만약 모든 땅에서 떨어지면(공중이면)
            {
                onGround = false;                                   //변수 변경
                groundCount = 0;                                //혹시 모를 버그 방지 
                Fly();                                          //공중 함수 실행
            }
        }
    }
    /// <summary>
    /// 객체가 공중에서 땅에 처음 떨어질때 실행되는 함수
    /// </summary>
    private void Landing()
    {
        switch (type)
        {
            case Type.player:
                player.Landing();
                break;
            case Type.enemy:
                enemy.Landing();
                break;

            case Type.NPC:
                npc.Landing();
                break;
        }
    }
    /// <summary>
    /// 객체가 땅에서 떨어질때 실행되는 함수
    /// </summary>
    private void Fly()
    {
        switch (type)
        {
            case Type.player:
                player.Fly();
                break;
            case Type.enemy:
                enemy.Fly();
                break;

            case Type.NPC:
                npc.Fly();
                break;
        }
    }
}

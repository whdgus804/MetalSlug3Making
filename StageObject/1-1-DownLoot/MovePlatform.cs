using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    /// <summary>
    /// 해당 플랫폼이 나아갈 방향 
    /// </summary>
    public Vector2 movingForce;
    /// <summary>
    /// 플랫폼이 움직일 속도
    /// </summary>
    public float moveSpeed;
    /// <summary>
    /// 해당 플렛폼과 같이 움직임을 구현해줄 스크립트 리스트
    /// </summary>
    List<MovePlatformUse> movePlatformUses;
    /// <summary>
    /// 현재 플레이 중인지 나나태는 변수 true 면 현재 플레이 중
    /// </summary>
    bool onplay = true;
    bool nowMoving=false;
    PlayerHealth player;
    private void Awake()
    {
        movePlatformUses = new List<MovePlatformUse>();
        player=GameManager.Instance.PlayerHealth;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GroundSencer"))                                               //GroundSencer가 닿으면 해당 부모 오브젝트의 MovePlatformUse스크립트를 리스트에 추가
        {
            MovePlatformUse movePlatformUse=collision.GetComponentInParent<MovePlatformUse>();      //스크립트 받기
            movePlatformUses.Add(movePlatformUse);
            if (nowMoving)
            {
                movePlatformUse.MovePlatform(moveSpeed, movingForce);
            }
            //Debug.Log(collision.transform.parent.name);


            //리스트에 추가
            //for(int i = 0; i < movePlatformUses.Count; i++)
            //{
            //    Debug.Log(movePlatformUses[i].gameObject.name);
            //}
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GroundSencer"))                                    //GroundSencer가 닿으면 리스트에서 제거 밑 움직임 정지
        {
            if (onplay)                                                                                 //플레이 중이면 버그방지
            {

                MovePlatformUse movePlatformUse = collision.GetComponentInParent<MovePlatformUse>();    //스크립트 받기
                GroundSencer groundSencer = collision.GetComponent<GroundSencer>();
                if (groundSencer.type == GroundSencer.Type.player)
                {
                    movePlatformUse.StopAllCoroutines();

                }else if(groundSencer.type== GroundSencer.Type.enemy)
                {
                    EnemyHP enemyHP = collision.GetComponentInParent<EnemyHP>();
                    if(enemyHP!=null&&enemyHP.HP>1)
                    {
                        movePlatformUse.StopAllCoroutines();                                                    //움직임 정지

                    }
                }
                movePlatformUses.Remove(movePlatformUse);                                               //리스트에서 제거
                
            }
            //for (int i = 0; i < movePlatformUses.Count; i++)
            //{
            //    Debug.Log(movePlatformUses[i].gameObject.name);
            //}
        }
        
    }

    /// <summary>
    /// 플렛폼을 움직일 함수
    /// </summary>
    /// <param name="moveSpeed"></param>
    void Move(float moveSpeed)
    {
        nowMoving = true;
        for(int i = 0; i < movePlatformUses.Count; i++)
        {
            movePlatformUses[i].MovePlatform(moveSpeed, movingForce);       //현제 리스트에 있는 (해당 플랫폼을 밟고 있는)오브젝트의 움직임을 구현하는 함수 실행
        }
    }
    public void PlatformStart(float move)
    {
        
        StartCoroutine(PlatformStartCoroutine(move));
    }
    IEnumerator PlatformStartCoroutine(float move)
    {
        Move(move);
        while (true)
        {
            yield return new WaitForFixedUpdate();
            transform.Translate(movingForce*move*Time.deltaTime);
        }
    }
    /// <summary>
    /// 움직음을 정지하는 함수
    /// </summary>
    public void MoveStop()
    {
        nowMoving = false;
        for (int i = 0;i < movePlatformUses.Count; i++)
        {
            movePlatformUses[i].StopAllCoroutines();                      //현제 리스트에 있는 (해당 플랫폼을 밟고 있는)오브젝트의 움직임을 구현코루틴 모두 정지
        }
        StopAllCoroutines();
    }

    private void OnApplicationQuit()
    {
        onplay = false;                                             //현재 플레이중임이 아님으로 변수 변경
    }
}

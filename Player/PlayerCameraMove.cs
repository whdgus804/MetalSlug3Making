using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMove : MonoBehaviour
{
    /// <summary>
    /// 시네머신 카메라
    /// </summary>
    [HideInInspector] public CinemachineVirtualCamera cM_Camera;

    BoxCollider2D[] col;
    PlayerHealth player;
    [SerializeField] LayerMask playerLayer;
    /// <summary>
    /// 뒷배경 스크립트
    /// </summary>
    [SerializeField]BackGroundMove backGroundMove;
    /// <summary>
    /// 현재 카메라가 특정 좌표에 고정되어 있는지 나태는 변수
    /// </summary>
    bool nowCameraSet = false;

    /// <summary>
    /// 점수매니저
    /// </summary>
    ScoreManager scoreManager;

    private void Awake()
    {
        player = GameManager.Instance.PlayerHealth;
        cM_Camera=FindAnyObjectByType<CinemachineVirtualCamera>();  //시네머신 카메라 지정
        //col=GetComponent<BoxCollider2D>();
        col=GetComponentsInChildren<BoxCollider2D>();
        scoreManager = GameManager.Instance.ScoreManager;
    }
    
    /// <summary>
    /// 플레이어가 왼쪽갈때 카메라가 되돌아가는 것을 방지 하는 함수
    /// </summary>
    public void CameraStayHere()
    {
        if (!nowCameraSet)
        {
            StopAllCoroutines();
            //cM_Camera.Follow = null;
            cM_Camera.Follow = scoreManager.cameraPos;
        }
    }
    /// <summary>
    /// 카메라를 제자리에서 멈추되 모든 콜라이더를 끄는 함수
    /// </summary>
    public void CameraColOff()
    {
        for (int i = 0; i < col.Length; i++)                         //가지고 있는 모든 콜라이더 비활성화
        {
            col[i].enabled = false;                                  //해당 콜루전이 사라지기 전까지는 카메라 고정(플레이어가 부수고 가야함)

        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(player==null)
            player=GameManager.Instance.PlayerHealth;
        if (collision.CompareTag("Player") && player.HP>0)                     //트리거에 플레이어가 닿으면 
        {
            //transform.position = collision.transform.position;  //플레이어 위치로 이동
            //transform.parent=collision.transform;               //플레이어의 자식으로 들어감(따라가기 위함)
            //cM_Camera.Follow = col[0].transform;
            cM_Camera.Follow = transform;
            StartCoroutine(CamerMove());
        }else if (collision.CompareTag("CameraPoint"))          //카메라 포인트에 닿으면
        {

            //cM_Camera.Follow = collision.transform;             //시네머신 카메라가 따라가는 객체 변경
            //transform.parent = null;                              //추가 이동 방지를 위해 부모 해제
            CameraPositioSet(collision.transform);
        }
    }
    public void CameraPositioSet(Transform trans)
    {
        nowCameraSet = true;
        CameraColOff();
        cM_Camera.Follow = trans;
    }
    
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        transform.position = collision.transform.position;
    //    }
    //}
    /// <summary>
    /// 카메라 포인트에 닿은 후 다시 플레이어를 따라가야할때 실행하는 함수
    /// </summary>
    public void CameraReset()
    {
        nowCameraSet= false;
        StopAllCoroutines();
        if (scoreManager.cameraPos == null)
        {
            cM_Camera.Follow = transform;
            for(int i = 0;i < col.Length; i++)
            {
                col[i].enabled = true;
            }

        }
        else
        {

            cM_Camera.Follow = scoreManager.cameraPos;
        }
        
        StartCoroutine(CamerMove());
        //if (player != null)
        //{
        //    for(int i = 0;i < col.Length;i++)
        //    {

        //        col[i].enabled = true;     //트리거 다시 활성화 
        //    }
        //    transform.position = player.transform.position;
        //    transform.parent = player.transform;               //플레이어의 자식으로 들어감(따라가기 위함)
        //    cM_Camera.Follow = col[0].transform;
        //}
    }
    
    IEnumerator CamerMove()
    {
        if (backGroundMove != null)
        {
            while (true)
            {
                backGroundMove.OnMove();
                cameraPosition.x = player.transform.position.x;
                transform.position = cameraPosition;
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (true)
            {
                cameraPosition.x = player.transform.position.x;
                transform.position = cameraPosition;
                yield return new WaitForFixedUpdate();
            }
        }
    }

   public Vector2 cameraPosition=new Vector2(0,1.07f);
}

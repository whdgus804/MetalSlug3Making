using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ship : MonoBehaviour
{
    /// <summary>
    /// 카메라를 고정시킬 위치
    /// </summary>
    [SerializeField] Transform cameraPoint; 
    /// <summary>
    /// 카메라 스크립트
    /// </summary>
    PlayerCameraMove playerCamera;
    /// <summary>
    /// 플랫폼의 움직임을 수행하는 발판
    /// </summary>
    MovePlatform movePlatform;
    /// <summary>
    /// 현제 움직임을 시작했는지 나타내는 변수
    /// </summary>
    bool onMove = false;
    /// <summary>
    /// 배밑의 물 애니메이터
    /// </summary>
    [SerializeField]Animator waterAnim;
    /// <summary>
    /// 처음 부딪힐 적의 배
    /// </summary>
    [SerializeField] Enemy_Ship enemyShip;

    /// <summary>
    /// 플레이어가 죽으면 부활 할 장소
    /// </summary>
    [SerializeField] Transform spawnPos;

    /// <summary>
    /// 점수 매니저
    /// </summary>
    ScoreManager scoreManager;
    /// <summary>
    /// 애니메이터
    /// </summary>
    Animator anim;
    [SerializeField] Vector2 wateranimPos;

    public BackGroundMove backGroundMove;

    
    private void Awake()
    {
        anim= GetComponent<Animator>();
        playerCamera=FindAnyObjectByType<PlayerCameraMove>();
        movePlatform=GetComponent<MovePlatform>();
        scoreManager = GameManager.Instance.ScoreManager;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ship"))
        {
            backGroundMove.StopAllCoroutines();
            enemyShip.MoveStart();                                      //적 배 움직임 시작
            //StartCoroutine(MoveBackStop());
            movePlatform.MoveStop();                                    //움직임 정지
            movePlatform.moveSpeed *= -1;                               //이동방향지정
            movePlatform.PlatformStart(movePlatform.moveSpeed);         //뒤로 움직이기
            StartCoroutine(MoveBackStop());                             //뒤로 그만 움직이는 코루틴 시작
            playerCamera.cM_Camera.Follow = null;                       //카메라는 움직임 멈춤
        } else if (collision.CompareTag("Target"))
        {
            //배가 움직임을 멈추고 부숴질 위치
            Broken();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&&!onMove)
        {
            backGroundMove.moveSpeed = 0.7f;
            onMove = true;                  //변수 변경
            StartCoroutine(Move());         //움직임 코루틴 실행
            CameraSet();                    //카메라 고정
            PlayerHealth playerHealth=collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.spanwPosition = spawnPos;
        }
    }

    /// <summary>
    /// 움직임을 수행하는 함수를 시작하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Move()
    {

        yield return new WaitForSeconds(1.0f);                              //기다림
        backGroundMove.OnMoveAuto();
        movePlatform.PlatformStart(movePlatform.moveSpeed);                 //움직임
        waterAnim.SetBool("Move", true);                                    //애니메이션 값전달
        
    }
    /// <summary>
    /// 카메라를 배위치에 고정시키는 함수
    /// </summary>
    void CameraSet()
    {
        playerCamera.CameraPositioSet(cameraPoint);
        scoreManager.CameraPos(cameraPoint);
    }
    /// <summary>
    /// 배가 앞으로 나아가는 함수
    /// </summary>
    public void MoveFront()
    {
        backGroundMove.OnMoveAuto();
        if(movePlatform.moveSpeed < 0)                                      //속도가 음수이면
        {
            movePlatform.moveSpeed*=-1;                                     //양수으로 변경
            playerCamera.cM_Camera.Follow = cameraPoint;                    //카메라 따라오게 하기
        }
        movePlatform.PlatformStart(movePlatform.moveSpeed);                 //움직이기
    }
    /// <summary>
    /// 뒤로 움직이는 것을 멈추는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveBackStop()
    {
        yield return new WaitForSeconds(3.3f);  //기다리기
        movePlatform.MoveStop();                //정지
        enemyShip.MoveStop();                   //적 배 움직임 정지
        waterAnim.SetBool("Move",false);        //애니메이션 값전달
    }

    void Broken()
    {
        backGroundMove.StopAllCoroutines();
        waterAnim.transform.parent= null;
        cameraPoint.parent= null;
        waterAnim.SetBool("Move", false);
        waterAnim.SetTrigger("Broken");
        waterAnim.transform.position = wateranimPos;
        anim.SetTrigger("Broken");
        StartCoroutine(BrokenAgain());
    }
    IEnumerator BrokenAgain()
    {
        yield return new WaitForSeconds(1.0f);
        anim.SetTrigger("Broken");
        waterAnim.SetTrigger("Broken");
        yield return new WaitForSeconds(2.0f);
        anim.SetTrigger("Broken");
        waterAnim.SetTrigger("Broken");
        

    }

}

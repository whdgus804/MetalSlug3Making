using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class LucostCenter : MonoBehaviour
{
    GameObject chenamachaine;
    /// <summary>
    /// 중심이 이동될 위치
    /// </summary>
    Transform centerPosL;
    /// <summary>
    /// 중심으로 이동될 오른쪽 위치
    /// </summary>
    Transform centerPosR;
    /// <summary>
    /// 이동속도
    /// </summary>
    [SerializeField] float moveSpeed;
    Enemy_Lucost lucost;
    /// <summary>
    /// 플레이어가 들어오면 도망칠범위
    /// </summary>
    [SerializeField] float runawayRange;
    PlayerHealth player;
    Transform lucostTransform;

    /// <summary>
    /// 좌우를 분간해줄 변수 true면 왼쪽
    /// </summary>
    bool targetSign=true;

    
    private void Awake()
    {
        //if (chenamachaine == null)
        //{
        //    Debug.Log("Point==null");
        //}
        //else
        //{
        //    centerPosL = chenamachaine.transform.GetChild(2);
        //    Debug.Log(centerPosL.name);
        //    centerPosR = chenamachaine.transform.GetChild(1);
        //    Debug.Log(centerPosR.name);
        //}
        lucostTransform = transform.GetChild(0);
        CinemachineVirtualCamera camera = FindAnyObjectByType<CinemachineVirtualCamera>();          //움직이는 카메라 받아오기
        chenamachaine =camera.gameObject;                                                           //받은 카메라 오브젝트로 받기
        centerPosL=chenamachaine.transform.GetChild(2);                                             //해당 오브젝트의 자식의 위치 받기
        centerPosR=chenamachaine.transform.GetChild(1);
        lucost=GetComponentInChildren<Enemy_Lucost>();                                               
        player=GameManager.Instance.PlayerHealth;
    }
    private void OnEnable()
    {
        //ChangeMove();                       //샌터 이동
        //lucost.Left(targetSign);
        StartCoroutine(MoveLeft()); 
        StartCoroutine(RunRange());
        //StartCoroutine(RePosition());
    }
    //IEnumerator RePosition()
    //{
    //    yield return new WaitForSeconds(0.3f);
    //    centerPosL = chenamachaine.transform.GetChild(2);
    //    centerPosR = chenamachaine.transform.GetChild(1);
    //    StartCoroutine(RePosition());
    //}
    /// <summary>
    /// 연속 이동을 방지하는 코루틴 함수 
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveCoolTime()
    {
        yield return new WaitForSeconds(1.5f);              //일정 시간 기다리기
        StartCoroutine(RunRange());                         //플레이어와의 거리재기
        lucost.Runaway(false);                              //도망끝 함수 실행
    }
    /// <summary>
    /// 반대쪽 샌터로 이동하는 함수
    /// </summary>
     void ChangeMove()
    {
        StopAllCoroutines();                //모든 코루틴 정지
        //bool sign = false;
        //float distanc = player.transform.position.x - transform.position.x;
        //if (distanc < 0)
        //{
        //    //플레이어가 왼쪽
        //    StartCoroutine(MoveRight());
        //    sign = false;
        //    if(targetSign != sign)
        //    {
        //        lucost.LookPlayerStart();
        //    }
        //}
        //else
        //{
        //    StartCoroutine(MoveLeft());
        //    sign = true;
        //    if (targetSign != sign)
        //    {
        //        lucost.LookPlayerStart();
        //    }
        //}
        //targetSign = sign;
        if (targetSign)
        {
            StartCoroutine(MoveRight());
            targetSign = false;
        }
        else
        {
            StartCoroutine (MoveLeft());
            targetSign=true;
        }
        lucost.LookPlayerStart();

        //StopAllCoroutines();                //이동정지
        //targetSign = !targetSign;           //변수조정
        
        //lucost.LookPlayerStart();
        //if (targetSign)                      //변수에 따른 코루틴 실행구별
        //{
        //    StartCoroutine(MoveLeft());     //true면 왼쪽으로 이동
        //}
        //else
        //{
        //    StartCoroutine (MoveRight());   //false면 오른쪽으로 이동
        //}
        //StartCoroutine(RePosition());
    }


    /// <summary>
    /// 왼쪽을 적의 중심으로 삼는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveLeft()
    {
        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, centerPosL.position, moveSpeed * Time.deltaTime);      //위치를 왼쪽으로 따라가게 하기 
            yield return null;
        }
    }
    /// <summary>
    /// 오른쪽을 적의 중심으로 삼는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveRight()
    {
        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, centerPosR.position, moveSpeed * Time.deltaTime);      //위치를 오른쪽으로 따라가게 하기
            yield return null;
        }
    }
    /// <summary>
    /// 중심으로 이동하는 것을 멈추는 함수
    /// </summary>
    /// <param name="removeTime">다시 움직이기 가지 걸릴 시간</param>
    public void StopMoveCenter(float removeTime)
    {
        StopAllCoroutines();                            //모든 코루틴 정지
        StartCoroutine(ReStartMove(removeTime));        //일정 시간 뒤 다시 움직 이게 하기
    }
    /// <summary>
    /// 멈춤 후 다시 이동하게할 코루틴 함수
    /// </summary>
    /// <param name="time">기다릴 시간</param>
    /// <returns></returns>
    IEnumerator ReStartMove(float time)
    {
        yield return new WaitForSeconds(time);          //시간 기다리기
        
        StartCoroutine(MoveCoolTime());
        if (targetSign)                                 //변수에 따른 센터 위치 정하기
        {
            StartCoroutine(MoveLeft());                 //true면 왼쪽을 중심으로 이동
        }
        else                                            //false면 오른쪽을 중심으로 이동
        {
            StartCoroutine(MoveRight());
        }
    }
    /// <summary>
    /// 플레이어와의 거리를 계산하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator RunRange()
    {
        while (true)
        {
            float distanc = (player.transform.position-transform.position).sqrMagnitude;        //플레이어와의 거리 계산
            if (distanc < runawayRange * runawayRange)                                          //만약 플레이어가 가까우면
            {
                //lucostTransform.position = transform.position;
                StartCoroutine (MoveCoolTime());                                                //쿨타임 시작
                lucost.Runaway(true);                                                           //도망시작을 알림
                ChangeMove();                                                                   //다음 지점으로 도망가기
                //StopAllCoroutines();
            }
            yield return null;
        }
    }
    public void Attacking(bool nowAttack)
    {
        StopAllCoroutines();
        if(!nowAttack)
        {
            StartCoroutine(RunRange());
        }

    }
    public void OnDead()
    {
        StopAllCoroutines();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Misson1UI : MonoBehaviour
{
    //이미지를 받고 신호를 받으면 저장된 위치로 차례로 이동 간경 0.3초 정도
    //앞의 이미지보다 x축 더한 후 해당 위치로 이동 
    //start , complete 의문자를 따로 받고 y축이 다른 다른 위치 변수를 받고 해당 위치에 위 규칙으로 이동하게하기
    //모두 이동했으면 일정 시간 후 흩어지기
    //가운대의 위치를 받고 방향을 구한 후 반대 방향으로 이동하게 하기

    /// <summary>
    /// MISSION 1 이미지
    /// </summary>
    [SerializeField] Image[] missionImage;
    /// <summary>
    /// START 혹은 COMLETE 이미지
    /// </summary>
    [SerializeField] Image[] startOrComplete;
    /// <summary>
    /// 맨처음 위치
    /// </summary>
    [SerializeField] Vector2 firstPos;
    /// <summary>
    /// Start 혹은 COMPLETE 시작 위치
    /// </summary>
    [SerializeField] Vector2 secPos;
    //문자 간의 거리
    [SerializeField] float distance;

    [SerializeField] GameObject clearMenu;
    
    private void OnEnable()
    {
        
        StartCoroutine(MoveToMisson1UI());
    }
    /// <summary>
    /// UI움직임 수행 코루틴 함수 
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveToMisson1UI()
    {
        //float moveSpeed = 100.0f;
        for (int i = 0; i < missionImage.Length; i++)
        {
            MoveUI moveUI = missionImage[i].gameObject.GetComponent<MoveUI>();  //컴포넌트 받기
            Vector2 vec = firstPos;                 //위치 저장 
            vec.x += distance*(i+1);                //거리 값 저장
            if (i == 7)                             //만약 1 차례면 좀 멀리 떨어지게 하기
                vec.x += distance;
            moveUI.tartgetPos = vec;                //목표위치 지정
            moveUI.sideMoveSpeed = 0.0f;            //옆 움직임 정지
            moveUI.moveSpeed = 300;                 //이동속도 지정
            missionImage[i].enabled = true;         //이미지 활성화
            //missionImage[i].rectTransform.anchoredPosition = vec;
            yield return new WaitForSeconds(0.3f);  //기다리기
        }
            StartCoroutine(CompleteOrStartUI());        //다음 코루틴 실행

    }
    /// <summary>
    /// complete 혹은 start ui 생성 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator CompleteOrStartUI()
    {
        for(int i=0; i<startOrComplete.Length; i++)
        {
            MoveUI moveUI = startOrComplete[i].gameObject.GetComponent<MoveUI>();       //스크립트 받기
            moveUI.sideMoveSpeed = 70.0f;               //이동속도 지정
        }
        for(int i = 0;i < startOrComplete.Length; i++)
        {
            MoveUI moveUI = startOrComplete[i].gameObject.GetComponent<MoveUI>();       //스크립트 받기
            Vector2 vec = secPos;           //목표 지정
            vec.x += distance * (i + 1);        //거리 지정
            moveUI.tartgetPos = vec;            //목표 전달
            moveUI.sideMoveSpeed = 0.0f;        //이동 정지
            moveUI.moveSpeed = 300;             //목표지점으로 이동 시작
            startOrComplete[i].enabled = true;     //활성화
            yield return new WaitForSeconds(0.3f);      //기다리기
        }
        yield return new WaitForSeconds(2.0f);      //기다리기
        for (int i = 0; i < missionImage.Length; i++)   
        {
            MoveUI moveUI = missionImage[i].gameObject.GetComponent<MoveUI>(); //스크립트 받기
            moveUI.MoveFar();               //멀어지는 함수 실행
            moveUI.DisableStart();
        }
        for (int i = 0; i < startOrComplete.Length; i++)
        {
            MoveUI moveUI = startOrComplete[i].gameObject.GetComponent<MoveUI>();       //스크립트 받기
            moveUI.MoveFar();           //멀어지는 함수 실행ㄴ
            moveUI.DisableStart();

        }
        yield return new WaitForSeconds(2.0f);  //기다리기
        //Time.timeScale = 0.0f;
        if (clearMenu != null)
            clearMenu.SetActive(true);
    }
}

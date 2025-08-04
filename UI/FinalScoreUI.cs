using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class FinalScoreUI : MonoBehaviour
{
    /// <summary>
    /// 숫자 스프라이트
    /// </summary>
    [SerializeField]
    Sprite[] numbers;
    /// <summary>
    /// 점수 이미즈 
    /// </summary>
    [SerializeField]
    Image[] scoreImages;
    /// <summary>
    /// 포로 해방 카운트를 나타낼 이미지
    /// </summary>
    [SerializeField]
    Image[] prisonCountImages;
    ScoreManager scoreManager;
    [SerializeField] GameObject missionComplete;


    private void Awake()
    {
        scoreManager=GameManager.Instance.ScoreManager;
    }
    private void OnEnable()
    {
        //점수 받기
        int score = scoreManager.score;
        //string 타입으로 변환
        string numString = score.ToString();
        //Debug.Log($"원본{score}");
        //Debug.Log($"변환{numString}");
        for (int i = 0; i < numString.Length; i++)
        {
            char digitChart = numString[i]; //앞 문자 하나씩 차례로 저장
            int num = (int)char.GetNumericValue(digitChart);        //int 형으로 변환
            //Debug.Log($"자릿수{i + 1}:{num}");
            scoreImages[numString.Length - i - 1].sprite = numbers[num];        //이미지에 맞게 숫자 변경
        }
        StartCoroutine(PrisonerCountCoroutine());
    }
    /// <summary>
    /// 해방한 포로들의 카운트와 획득 점수를 나타내는 UI 변경  코루틴 함수
    /// </summary>
    /// <returns></returns>

    IEnumerator PrisonerCountCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        //yield return new WaitForSeconds(1.0f);
        int prisonCountTeen = 0;        //십의 자리 ui
        int prisonCount = 0;            //점수 계산한 해방 포로 수를 저장할 변수 
        for (int i = 0;i<scoreManager.prisonerCount; i++)
        {
            scoreManager.score += 10000;    //점수 추가
            prisonCount++;                  //해방 포로 카운트 증가
            yield return new WaitForSeconds(0.5f);  //기다리기

            if (prisonCount >9)     
            {
                prisonCountTeen++;
                prisonCountImages[0].sprite = numbers[prisonCountTeen];
                prisonCount = 0;
            }
            //Debug.Log(prisonCount);
            prisonCountImages[1].sprite=numbers[prisonCount];
            
            int score = scoreManager.score;
            //string 타입으로 변환
            string numString = score.ToString();
            for (int j = 0; j < numString.Length; j++)
            {
                char digitChart = numString[j]; //앞 문자 하나씩 차례로 저장
                int num = (int)char.GetNumericValue(digitChart);        //int 형으로 변환
                                                                        //Debug.Log($"자릿수{i + 1}:{num}");
                scoreImages[numString.Length - j - 1].sprite = numbers[num];        //이미지에 맞게 숫자 변경
            }
        }
        yield return new WaitForSeconds(2.0f);

        missionComplete.SetActive(true);
        GameObject obj=transform.parent.gameObject;
        obj.SetActive(false);
    }

}

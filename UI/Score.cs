using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    /// <summary>
    /// 숫자 스프라이트
    /// </summary>
    [SerializeField] Sprite[] numbers;

    /// <summary>
    /// 숫자 UI 이미지 
    /// </summary>
    [SerializeField] Image[] scoreImage;

    ScoreManager scoreManager;
    private void Awake()
    {
        scoreManager = GameManager.Instance.ScoreManager;
    }

    /// <summary>
    /// 스코어를 얻는 함수 -1이면 점수 초기화
    /// </summary>
    /// <param name="score"></param>
    public void GetScore(int score)
    {
        if (score > 999999)
        {
            for (int i = 0; i < scoreImage.Length; i++)
            {
                scoreImage[i].sprite = numbers[9];
            }
        }
        else
        {
            string numString = score.ToString();
            //Debug.Log($"원본{score}");
            //Debug.Log($"변환{numString}");
            for (int i = 0; i < numString.Length; i++)
            {
                char digitChart = numString[i];
                int num = (int)char.GetNumericValue(digitChart);
                //Debug.Log($"자릿수{i + 1}:{num}");
                scoreImage[numString.Length - i-1].sprite = numbers[num];
            }
        }
        //ScoreCheck(0.1f, score);
        //if (score >0)
        //{
        //    if (score > 999999)
        //    {
        //        score = 999999;
        //    }else if (score > 99999)
        //    {
        //        //점수가 10만 보다 크면
        //    }else if (score > 9999)
        //    {
        //        //점수가 10000자릿 수면
        //    }else if(score > 999)
        //    {
        //        //점수가 1000자릿 수면
        //    }else if(score > 99){
        //        //점수가 100자릿수면
        //    }else if (score > 9)
        //    {
        //        //점수가 10자릿수면
        //        float checkNum = score;
        //        checkNum *= 0.1f;
        //        int setSocre=Mathf.FloorToInt(checkNum);
        //        scoreImage[1].sprite = numbers[setSocre];
        //        setSocre *= 10;
        //        score -= setSocre;
        //        scoreImage[0].sprite = numbers[score];
        //    }
        //    else
        //    {
        //        scoreImage[0].sprite = numbers[score];
        //    }
        //}
    }

    //void ScoreCheck(int score)
    //{



    //    //float muiltynum = muityNumSize;
        
    //    //int sizeCount = 0;      //자릿수
    //    //for(sizeCount =0; muiltynum<=0; sizeCount++) //자리수 구하기
    //    //{
    //    //    muiltynum *= 10;
    //    //}
    //    //for(int i=0; i<sizeCount+1; i++)
    //    //{
    //    //    float checkNum = score; //10
    //    //    checkNum *= muityNumSize;
    //    //    int firstNum = Mathf.FloorToInt(checkNum);
    //    //}
    //    //checkNum *= muityNumSize; //1
    //    //int firstNum = Mathf.FloorToInt(checkNum);  //첫 자리

    //    //int countCheck = 0;     //0
    //    //for (countCheck = 0; numuityNumSize <= 0; countCheck++)
    //    //{
    //    //    numuityNumSize *= 10;
    //    //}
    //    //int setSocre = Mathf.FloorToInt(checkNum);
    //    //for(int i=0; i<countCheck+1; i++)
    //    //{
    //    //    setSocre *= 10;
    //    //}
    //    //int firstCount = score- setSocre;
    //    //Debug.Log($"십의자리{setSocre}일의자리{firstCount}");
    //}
}

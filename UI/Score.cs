using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    /// <summary>
    /// ���� ��������Ʈ
    /// </summary>
    [SerializeField] Sprite[] numbers;

    /// <summary>
    /// ���� UI �̹��� 
    /// </summary>
    [SerializeField] Image[] scoreImage;

    ScoreManager scoreManager;
    private void Awake()
    {
        scoreManager = GameManager.Instance.ScoreManager;
    }

    /// <summary>
    /// ���ھ ��� �Լ� -1�̸� ���� �ʱ�ȭ
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
            //Debug.Log($"����{score}");
            //Debug.Log($"��ȯ{numString}");
            for (int i = 0; i < numString.Length; i++)
            {
                char digitChart = numString[i];
                int num = (int)char.GetNumericValue(digitChart);
                //Debug.Log($"�ڸ���{i + 1}:{num}");
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
        //        //������ 10�� ���� ũ��
        //    }else if (score > 9999)
        //    {
        //        //������ 10000�ڸ� ����
        //    }else if(score > 999)
        //    {
        //        //������ 1000�ڸ� ����
        //    }else if(score > 99){
        //        //������ 100�ڸ�����
        //    }else if (score > 9)
        //    {
        //        //������ 10�ڸ�����
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
        
    //    //int sizeCount = 0;      //�ڸ���
    //    //for(sizeCount =0; muiltynum<=0; sizeCount++) //�ڸ��� ���ϱ�
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
    //    //int firstNum = Mathf.FloorToInt(checkNum);  //ù �ڸ�

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
    //    //Debug.Log($"�����ڸ�{setSocre}�����ڸ�{firstCount}");
    //}
}

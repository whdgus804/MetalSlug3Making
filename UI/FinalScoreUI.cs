using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class FinalScoreUI : MonoBehaviour
{
    /// <summary>
    /// ���� ��������Ʈ
    /// </summary>
    [SerializeField]
    Sprite[] numbers;
    /// <summary>
    /// ���� �̹��� 
    /// </summary>
    [SerializeField]
    Image[] scoreImages;
    /// <summary>
    /// ���� �ع� ī��Ʈ�� ��Ÿ�� �̹���
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
        //���� �ޱ�
        int score = scoreManager.score;
        //string Ÿ������ ��ȯ
        string numString = score.ToString();
        //Debug.Log($"����{score}");
        //Debug.Log($"��ȯ{numString}");
        for (int i = 0; i < numString.Length; i++)
        {
            char digitChart = numString[i]; //�� ���� �ϳ��� ���ʷ� ����
            int num = (int)char.GetNumericValue(digitChart);        //int ������ ��ȯ
            //Debug.Log($"�ڸ���{i + 1}:{num}");
            scoreImages[numString.Length - i - 1].sprite = numbers[num];        //�̹����� �°� ���� ����
        }
        StartCoroutine(PrisonerCountCoroutine());
    }
    /// <summary>
    /// �ع��� ���ε��� ī��Ʈ�� ȹ�� ������ ��Ÿ���� UI ����  �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>

    IEnumerator PrisonerCountCoroutine()
    {
        yield return new WaitForSeconds(3.0f);
        //yield return new WaitForSeconds(1.0f);
        int prisonCountTeen = 0;        //���� �ڸ� ui
        int prisonCount = 0;            //���� ����� �ع� ���� ���� ������ ���� 
        for (int i = 0;i<scoreManager.prisonerCount; i++)
        {
            scoreManager.score += 10000;    //���� �߰�
            prisonCount++;                  //�ع� ���� ī��Ʈ ����
            yield return new WaitForSeconds(0.5f);  //��ٸ���

            if (prisonCount >9)     
            {
                prisonCountTeen++;
                prisonCountImages[0].sprite = numbers[prisonCountTeen];
                prisonCount = 0;
            }
            //Debug.Log(prisonCount);
            prisonCountImages[1].sprite=numbers[prisonCount];
            
            int score = scoreManager.score;
            //string Ÿ������ ��ȯ
            string numString = score.ToString();
            for (int j = 0; j < numString.Length; j++)
            {
                char digitChart = numString[j]; //�� ���� �ϳ��� ���ʷ� ����
                int num = (int)char.GetNumericValue(digitChart);        //int ������ ��ȯ
                                                                        //Debug.Log($"�ڸ���{i + 1}:{num}");
                scoreImages[numString.Length - j - 1].sprite = numbers[num];        //�̹����� �°� ���� ����
            }
        }
        yield return new WaitForSeconds(2.0f);

        missionComplete.SetActive(true);
        GameObject obj=transform.parent.gameObject;
        obj.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    /// <summary>
    /// 일의 자리 숫자를 나타낼 변수
    /// </summary>
    int time = 0;
    /// <summary>
    /// 십의 자리 숫자를 나타낼 변수
    /// </summary>
    int teen=6;
    /// <summary>
    /// 십의 자리 숫자 이미지
    /// </summary>
    [SerializeField]Image teenCount;
    /// <summary>
    /// 일의 자리 숫자 이미지
    /// </summary>
    [SerializeField]Image countImage;
    /// <summary>
    /// 숫자 스프라이트 배열
    /// </summary>
    [SerializeField]Sprite[] numbers;
    /// <summary>
    /// 1초
    /// </summary>
    WaitForSeconds waitTime = new WaitForSeconds(3.0f);

    
    private void OnEnable()
    {
        time = 0;
        teen = 6;
        StartCoroutine(Time(waitTime));
    }
    /// <summary>
    /// 타이머 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Time(WaitForSeconds wait)
    {
        yield return wait;
        time--;
        if (time <0)
        {
            TeenCount();
        }
        else
        {
            countImage.sprite = numbers[time];
            StartCoroutine(Time(waitTime));
        }
    }
    /// <summary>
    /// 십의자리 숫자를 계산하는 함수
    /// </summary>
    void TeenCount()
    {
        teen--;
        if (teen < 0)
        {
            Debug.Log("GameOver");
        }
        else
        {
            time = 10;
            teenCount.sprite = numbers[teen];
            WaitForSeconds wait = new WaitForSeconds(0.0f);
            StartCoroutine(Time(wait));
        }
    }
    public void OnRespawn()
    {
        StopAllCoroutines();
        teen = 6;
        time = 0;
        StartCoroutine(Time(waitTime));
    }
}

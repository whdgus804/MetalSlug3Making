using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    /// <summary>
    /// ���� �ڸ� ���ڸ� ��Ÿ�� ����
    /// </summary>
    int time = 0;
    /// <summary>
    /// ���� �ڸ� ���ڸ� ��Ÿ�� ����
    /// </summary>
    int teen=6;
    /// <summary>
    /// ���� �ڸ� ���� �̹���
    /// </summary>
    [SerializeField]Image teenCount;
    /// <summary>
    /// ���� �ڸ� ���� �̹���
    /// </summary>
    [SerializeField]Image countImage;
    /// <summary>
    /// ���� ��������Ʈ �迭
    /// </summary>
    [SerializeField]Sprite[] numbers;
    /// <summary>
    /// 1��
    /// </summary>
    WaitForSeconds waitTime = new WaitForSeconds(3.0f);

    
    private void OnEnable()
    {
        time = 0;
        teen = 6;
        StartCoroutine(Time(waitTime));
    }
    /// <summary>
    /// Ÿ�̸� �ڷ�ƾ �Լ�
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
    /// �����ڸ� ���ڸ� ����ϴ� �Լ�
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

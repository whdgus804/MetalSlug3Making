using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleObject : MonoBehaviour
{
    public Action onDisable = null;


    protected virtual void OnDisable()
    {
        //transform.localPosition = Vector3.zero;
        Reset();
        StopAllCoroutines();
        onDisable?.Invoke();
    }
    /// <summary>
    /// 베이스는 트랜스폼을 전부 초기화 시킨다
    /// </summary>
    protected virtual void Reset()
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
    }
    /// <summary>
    /// 총알의 생존시간
    /// </summary>
    /// <param name="time"></param>
    protected virtual void DisableTime(float time = 0.0f)
    {
        StartCoroutine(DisableTimeCoroutine(time));
    }
    IEnumerator DisableTimeCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
    
}

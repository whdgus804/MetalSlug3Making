using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    /// <summary>
    /// 획득시 얻을 점수
    /// </summary>
    [SerializeField] int itemScore;

    /// <summary>
    /// 점수를 등록할 스코어 매니저스크립트
    /// </summary>
    protected ScoreManager scoreManager;
    /// <summary>
    /// 아이템의 스프라이트
    /// </summary>
    SpriteRenderer sprite;
    protected virtual void Awake()
    {
        scoreManager=GameManager.Instance.ScoreManager;         //스코어 매니저 받기
        sprite=GetComponent<SpriteRenderer>();  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))             //틀리거가 플레이어랑 부딪히면 점수 추가 함수 실행 후 없어지기
        {
            GetItem(collision.gameObject);                                  //함수 실행
        }
    }
    /// <summary>
    /// 플레이어가 아이템을 먹을때 실행되는 함수
    /// </summary>
    protected virtual void GetItem(GameObject player)
    {
        scoreManager.Score += itemScore;
        gameObject.SetActive(false);                //오브젝트 비활성화

    }
    /// <summary>
    /// 아이템이 일정 시간 이상 지난 후 사라져야 할때 실행되는 함수
    /// </summary>
    /// <param name="time">유지할 시간</param>
    protected void DisableTime(float time)
    {
        StartCoroutine(DisableCoroutine(time));
    } 
    /// <summary>
    /// 이정 시간 후 아이템을 없애는 코루틴
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator DisableCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        for(int i = 0; i < 10; i++)
        {
            sprite.color = Vector4.zero;
            yield return new WaitForSeconds(0.05f);
            sprite.color = Vector4.one;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject);
    }
}

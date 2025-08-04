using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTimeDisable : MonoBehaviour
{
    /// <summary>
    /// 아이템이 사라질 시간
    /// </summary>
    [SerializeField] float disabelTime;
    /// <summary>
    /// 땅에 닿은 후에 사라질지 나타내는 변수 true면 땅에 닿은 후에 타이머 시작
    /// </summary>
    [Tooltip("땅에 닿은 후 타이머 시작")]
    [SerializeField] bool groundDisable;
    /// <summary>
    /// 객체를 삭제할지 비활성화 할지 나타내는 변수 true면 비활성화 한다
    /// </summary>
    [Tooltip("꺼져있을 경우 삭제")]
    [SerializeField] bool disable;

    AnimationDisabler animDisabler;
    private void Awake()
    {
        animDisabler=GetComponent<AnimationDisabler>();   
    }
    private void OnEnable()
    {
        if(!groundDisable)                              //땅에 닿은 후 삭젤하는게 아니라면 타이머 시작
            StartCoroutine(DisableTime());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (groundDisable)                                      //땅에 닿은 후 삭제 되게끔 변수가 활성 되있고
        {
            if (collision.gameObject.CompareTag("Ground"))      //땅에 닿았으면
            {   
                StartCoroutine(DisableTime());                  //타이머 시작
            }
        }
    }
    /// <summary>
    /// 일정 시간 뒤 아이템을 삭제하는 코루틴 함수 
    /// </summary>
    /// <returns></returns>
    IEnumerator DisableTime()
    {
        yield return new WaitForSeconds(disabelTime);   //기다리기
        if (disable)                                    //만약 비활성화 변수가 켜져있다면
        {
            animDisabler.Twinkle();                     //반짝인 후 비활성화 하기
        }
        else                                            //아니면
        {
            animDisabler.Destoyer();                    //반짝인 후 삭제하기
        }
    }
}

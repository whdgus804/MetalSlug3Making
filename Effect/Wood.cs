using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    /// <summary>
    /// 오브젝트의 생존시간
    /// </summary>
    [SerializeField] float disableTime;
    /// <summary>
    /// 위로 튈힘
    /// </summary>
    [SerializeField] float upForce;
    /// <summary>
    /// 옆으로 튈힘
    /// </summary>
    [SerializeField] float sideForce;
    /// <summary>
    /// 리지드바디
    /// </summary>
    Rigidbody2D rigid;
    private void Awake()
    {
        rigid= GetComponent<Rigidbody2D>();                 //리지드바디 받기
    }

    private void OnEnable()
    {
        float up=Random.Range(upForce*0.8f, upForce);       //힘을 랜덥값으로 받기
        float side=Random.Range(-sideForce, sideForce);     //힘을 랜덤값으로 받기
        rigid.AddForce(new Vector2(side, up));              //받은 랜덤값으로 튀기
        StartCoroutine(DisableTimer());
    }
    /// <summary>
    /// 오브젝트의 생존 시간 후 제거할 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator DisableTimer()
    {
        yield return new WaitForSeconds(disableTime);   //기다리기
        gameObject.SetActive(false);                    //오브젝트 비활성화
        //Destroy(gameObject);
    }
}

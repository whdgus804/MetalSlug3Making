using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    /// <summary>
    /// 이동속도
    /// </summary>
    public float moveSpeed;
    /// <summary>
    /// 현재 적용된 이동소곧
    /// </summary>
    float nowMove;
    /// <summary>
    /// 이동 방향
    /// </summary>
    Vector2 moveForce = new Vector2(1, 0);

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Camera"))
    //    {
    //        OnMove(moveSpeed);

    //    }
    //}

    //private void FixedUpdate()
    //{
    //    transform.Translate(moveForce*nowMove*Time.deltaTime);
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Camera"))
    //    {
    //        OnMove(0.0f);
    //    }
    //}

    public void OnMove()
    {
        transform.Translate(moveForce*moveSpeed*Time.deltaTime);
    }

    public void OnMoveAuto()
    {
        StartCoroutine(OnMoveCoroutine());
    }
    IEnumerator OnMoveCoroutine()
    {
        while (true)
        {
            transform.Translate(moveForce * moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
    ///// <summary>
    ///// 이동속도를 변경하는 함수 
    ///// </summary>
    ///// <param name="moveSpeed">변경할 이동속도</param>
    //void OnMove(float moveSpeed)
    //{
    //    nowMove= moveSpeed; //이동속도 변경
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    /// <summary>
    /// �̵��ӵ�
    /// </summary>
    public float moveSpeed;
    /// <summary>
    /// ���� ����� �̵��Ұ�
    /// </summary>
    float nowMove;
    /// <summary>
    /// �̵� ����
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
    ///// �̵��ӵ��� �����ϴ� �Լ� 
    ///// </summary>
    ///// <param name="moveSpeed">������ �̵��ӵ�</param>
    //void OnMove(float moveSpeed)
    //{
    //    nowMove= moveSpeed; //�̵��ӵ� ����
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : BulletBase
{
     Vector3 moveValue = Vector3.zero;
     protected override void OnEnable()
    {
        
        base.OnEnable();
        //if (transform.localScale.x < 0)
        //{
        //    moveValue = Vector3.left;
        //}
        //else
        //{
        //    moveValue= Vector3.right;
        //    //if(transform.eulerAngles.z>-1.0f&& transform.eulerAngles.z < 1.0f)
        //    //{

        //    //}
        //}


        //if (transform.eulerAngles.z > -1.0f && transform.eulerAngles.z < 1.0f)  //ȸ������ ������
        //{
        //    //�� Ȥ�� ��

        //    if (transform.localScale.x > 0.0f) //���ý����� ���� 1�̸�
        //    {
        //        moveValue = Vector3.right;  //���������� �̵�
        //    }
        //    else                            //���ý����� ���� -1�̸�
        //    {
        //        moveValue = Vector3.left;   //�������� �̵�
        //    }
        //}else
        //{
        //    moveValue = Vector3.right;
        //}
        moveValue = Vector3.right;
        if(transform.eulerAngles.z > -1.0f && transform.eulerAngles.z < 1.0f && transform.localScale.x < 0)
        {
            moveValue = Vector3.left;
        }
    }
    private void FixedUpdate()
    {
        transform.Translate(moveValue * bulletMoveSpeed * Time.deltaTime);
    }


}

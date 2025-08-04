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


        //if (transform.eulerAngles.z > -1.0f && transform.eulerAngles.z < 1.0f)  //회전값이 없으면
        //{
        //    //앞 혹은 뒤

        //    if (transform.localScale.x > 0.0f) //로컬스케일 값이 1이면
        //    {
        //        moveValue = Vector3.right;  //오른쪽으로 이동
        //    }
        //    else                            //로컬스케일 값이 -1이면
        //    {
        //        moveValue = Vector3.left;   //왼쪽으로 이동
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

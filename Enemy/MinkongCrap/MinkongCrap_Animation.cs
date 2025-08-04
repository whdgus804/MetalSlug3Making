using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinkongCrap_Animation : EnemyAnimation
{
    int shoot_To_Hash = Animator.StringToHash("Shoot");
    int fall_To_Hash = Animator.StringToHash("Fall");
    int eat_To_Hash = Animator.StringToHash("Eat");
    int trun_To_Hash = Animator.StringToHash("Trun");

    /// <summary>
    /// 총알 발사 함수
    /// </summary>
    public void Shoot()
    {
        if(animator ==null)
            animator = GetComponent<Animator>();
        animator.SetTrigger(shoot_To_Hash);
    }
    /// <summary>
    /// 공중을 다니는 함수 
    /// </summary>
    public void Fly()
    {
        animator.SetBool(fall_To_Hash,true);
    }
    /// <summary>
    /// 착지 애니메이션함수
    /// </summary>
    public void Landing()
    {
        animator.SetBool(fall_To_Hash, false);
    }
    /// <summary>
    /// 먹는 애니메이션 함수
    /// </summary>
    /// <param name="eatting"></param>
    public void Eat(bool eatting)
    {
        animator.SetBool(eat_To_Hash ,eatting);
    }
    public void Trun()
    {
        animator.SetTrigger(trun_To_Hash);
    }
}

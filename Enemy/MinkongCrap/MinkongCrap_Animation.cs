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
    /// �Ѿ� �߻� �Լ�
    /// </summary>
    public void Shoot()
    {
        if(animator ==null)
            animator = GetComponent<Animator>();
        animator.SetTrigger(shoot_To_Hash);
    }
    /// <summary>
    /// ������ �ٴϴ� �Լ� 
    /// </summary>
    public void Fly()
    {
        animator.SetBool(fall_To_Hash,true);
    }
    /// <summary>
    /// ���� �ִϸ��̼��Լ�
    /// </summary>
    public void Landing()
    {
        animator.SetBool(fall_To_Hash, false);
    }
    /// <summary>
    /// �Դ� �ִϸ��̼� �Լ�
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

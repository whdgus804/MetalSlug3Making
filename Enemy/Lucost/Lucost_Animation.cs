using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucost_Animation : EnemyAnimation
{
    [SerializeField] GameObject head;
    public override void AttackAnim()
    {
        base.AttackAnim();
    }
    public void ExcutionAnim()
    {
        animator.SetTrigger("Excution");
        head.SetActive(true);
    }
    public override void DieAnim()
    {
        base.DieAnim();
        head.SetActive(false);
    }
    public void OnGround()
    {
        animator.SetBool("Fall", true);
    }
    public void Trun()
    {
        animator.SetTrigger("Turn");
    }

    public void TrunObject()
    {
        Vector3 vec = transform.localScale;
        vec.x *= -1;
        transform.localScale = vec;
    }
}

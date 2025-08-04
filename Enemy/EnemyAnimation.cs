
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    /// <summary>
    /// 애니메이터
    /// </summary>
    protected Animator animator;
    /// <summary>
    /// 이동애니메이션 해시
    /// </summary>
    readonly int move_To_Hash = Animator.StringToHash("Move"); 
    /// <summary>
    /// 공격 애니메이션 해시
    /// </summary>
    readonly int attack_To_Hash = Animator.StringToHash("Attack");
    /// <summary>
    /// 떨어짐 애니메이션 해시
    /// </summary>
    readonly int fall_To_Hash = Animator.StringToHash("Fall");
    /// <summary>
    /// 사망에니메이션 해시
    /// </summary>
    readonly int die_To_Hash = Animator.StringToHash("Dead");
    private void Awake()
    {
        animator = GetComponent<Animator>();            //애니메이터 지정
    }
    /// <summary>
    /// 움직임 애니메이션
    /// </summary>
    /// <param name="move">이동값</param>
    public virtual void MoveAnim(float move)
    {
        animator.SetFloat(move_To_Hash, move);          //애니메이터에 해시값 전달
    }
    /// <summary>
    /// 공격 애니메이션
    /// </summary>
    public virtual void AttackAnim()
    {
        animator.SetTrigger(attack_To_Hash);        //애니메이터에 해시값 전달
    }
    /// <summary>
    /// 떨어지는 애니메이션 
    /// </summary>
    /// <param name="fall">true면 떨어지는 중</param>

    public virtual void Fall(bool fall)
    {
        animator.SetBool(fall_To_Hash, fall);           //애니메이터에 해시값 전달
    }
    /// <summary>
    /// 사망애니메이션
    /// </summary>
    public virtual void DieAnim()
    {
        animator.SetTrigger(die_To_Hash);           //애니메이터에 해시값 전달
    }
}

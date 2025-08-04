
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    /// <summary>
    /// �ִϸ�����
    /// </summary>
    protected Animator animator;
    /// <summary>
    /// �̵��ִϸ��̼� �ؽ�
    /// </summary>
    readonly int move_To_Hash = Animator.StringToHash("Move"); 
    /// <summary>
    /// ���� �ִϸ��̼� �ؽ�
    /// </summary>
    readonly int attack_To_Hash = Animator.StringToHash("Attack");
    /// <summary>
    /// ������ �ִϸ��̼� �ؽ�
    /// </summary>
    readonly int fall_To_Hash = Animator.StringToHash("Fall");
    /// <summary>
    /// ������ϸ��̼� �ؽ�
    /// </summary>
    readonly int die_To_Hash = Animator.StringToHash("Dead");
    private void Awake()
    {
        animator = GetComponent<Animator>();            //�ִϸ����� ����
    }
    /// <summary>
    /// ������ �ִϸ��̼�
    /// </summary>
    /// <param name="move">�̵���</param>
    public virtual void MoveAnim(float move)
    {
        animator.SetFloat(move_To_Hash, move);          //�ִϸ����Ϳ� �ؽð� ����
    }
    /// <summary>
    /// ���� �ִϸ��̼�
    /// </summary>
    public virtual void AttackAnim()
    {
        animator.SetTrigger(attack_To_Hash);        //�ִϸ����Ϳ� �ؽð� ����
    }
    /// <summary>
    /// �������� �ִϸ��̼� 
    /// </summary>
    /// <param name="fall">true�� �������� ��</param>

    public virtual void Fall(bool fall)
    {
        animator.SetBool(fall_To_Hash, fall);           //�ִϸ����Ϳ� �ؽð� ����
    }
    /// <summary>
    /// ����ִϸ��̼�
    /// </summary>
    public virtual void DieAnim()
    {
        animator.SetTrigger(die_To_Hash);           //�ִϸ����Ϳ� �ؽð� ����
    }
}

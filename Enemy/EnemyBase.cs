using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : EnemyHP
{
    /// <summary>
    /// ���� �̵��ӵ�
    /// </summary>
    [SerializeField] protected float moveSpeed;

    /// <summary>
    /// ���� �������� ����
    /// </summary>
    [SerializeField] protected Transform attackPos;
    /// <summary>
    /// ���� ���� ���� ����
    /// </summary>
    [SerializeField] protected Vector2 attackRange;
    /// <summary>
    /// ���� ���� ������
    /// </summary>
    [SerializeField] int attackDamage;
    /// <summary>
    /// ��ü�� ���ݰ����� ���̾�
    /// </summary>
    [SerializeField] protected LayerMask attackLayer;
    /// <summary>
    /// �÷��̾� ��������
    /// </summary>
    protected PlayerHealth player;

    /// <summary>
    /// ���� �̵��� ����, ����ó���� �Ҷ� ������ ����
    /// </summary>
    protected float secMoveSpeed=1.0f;
    protected virtual void Awake()
    {
        player = GameManager.Instance.PlayerHealth;
    }


    /// <summary>
    /// ���� �������� ���⶧ ������ �Լ�
    /// </summary>
    /// <param name="waitTime">��ٸ� �ð�</param>
    /// <param name="setSpeed">��ٸ� �� �̵��� ���� +Ȥ�� - ��ȣ�� 1�� �޴´�</param>
    public void StopMove(float waitTime,float setSpeed)
    {
        StartCoroutine(MoveStopper(waitTime,setSpeed));        //���޹ٵ� �ð���ŭ ��ٸ��� �ڷ�ƾ ����
    }
    /// <summary>
    /// �������� ������ �ڷ�ƾ �Լ�
    /// </summary>
    /// <param name="waitTime"></param>
    /// <param name="setSpeed"></param>
    /// <returns></returns>
    IEnumerator MoveStopper(float waitTime,float setSpeed)
    {
        secMoveSpeed=0.0f;                                  //�̵�����
        //StopMovedAnim();
        yield return new WaitForSeconds(waitTime);          //���޹��� ����ŭ ��ٸ���
        secMoveSpeed = setSpeed;                            //���� ���� �������� �̵��ϰԲ� ���� ����
        //StartMoveAgain();

    }
    ///// <summary>
    ///// �����ִϸ��̼� ������ ���� �Լ�
    ///// </summary>
    //protected virtual void StopMovedAnim()
    //{

    //}
    ///// <summary>
    ///// �������� ���� �� �ٽ� �������� ���۵ɶ� ����Ǵ� �Լ�
    ///// </summary>
    //protected virtual void StartMoveAgain()
    //{

    //}
    /// <summary>
    /// �����ϱ��� �����̸� ���� �ϴ� �Լ� 
    /// </summary>
    /// <param name="Delay"></param>
    protected virtual void OnAttack(float Delay)
    {

        StartCoroutine(AttackDelay(Delay));           //�ð��� ��ٸ� �ڷ�ƾ ���� ����
    }
    /// <summary>
    /// ���� �ð� �� ������ ������ �ڷ�ƾ �Լ�
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    IEnumerator AttackDelay(float waitTime)
    {
        //StopMove(waitTime, 1.0f);                        //�����Ҷ��� �̵�����
        //Debug.Log($"Attack_Delay:{waitTime}");              
        yield return new WaitForSeconds(waitTime);          //���޹��� ����ŭ ��ٸ���
        OnPlayer();                                         //�÷��̾� ����
        //Debug.Log("Attack");
    }
    /// <summary>
    /// ���������� �� �÷��̾ �ִ��� Ȯ���ϴ� �Լ� ������ �������� �ش�
    /// </summary>
    /// <returns></returns>
     void OnPlayer()
     {
        Collider2D collider;                                                                 //�浹ü ����
        collider = Physics2D.OverlapBox(attackPos.position, attackRange, 0, attackLayer);  //�÷��̾�ִ��� ���� ��ġ ���� ���� ��ŭ �÷��̾� ã��
        if (collider != null)                                                                //�÷��̾ ������
        {
            PlayerHealth player=collider.GetComponent<PlayerHealth>();          //��� Ÿ���� �÷��̾ ������ �ִ� HP��ũ��Ʈ ����
            if(player==null)
                player=GameManager.Instance.PlayerHealth;
            player.HP = attackDamage;                                           //���� ������ ��ŭ ������ �ֱ�
            //Debug.Log($"{collider.name}_attack({gameObject.name})");              //�÷��̾ ���� 
        }
        StartCoroutine(WaitPlayerRespawn());

     }
    IEnumerator WaitPlayerRespawn()
    {
        yield return new WaitUntil(() => player.HP > 0);
        LookPlayer();
    }
    /// <summary>
    /// ���� �÷��̾ �ٶ󺸴� �Լ�
    /// </summary>
    protected virtual void LookPlayer()
    {
        float sight = player.transform.position.x - transform.position.x;               //�÷��̾ �ش� ��ü ���� ���� Ȥ�� �����ʿ� �ִ��� ���
        Vector2 vector2 = Vector2.one;                                                  //���ý����Ͽ� ������ ���Ͱ�
        if(sight < 0)                                                                   //�÷��̾ ���ʿ�������
        {
            //�÷��̾ �ش� ��ü ���� ����
            vector2.x = -1;                                                             //���Ͱ� ����
            
        }
        int vectorx = (int)vector2.x;
        int scaleX= (int)transform.localScale.x;
        if (vectorx!=scaleX)
        {
            TrunAnim();
        }
        StartCoroutine(WaitTrunAnim(vector2));
    }
    IEnumerator WaitTrunAnim(Vector2 vector)
    {
        yield return new WaitForSeconds(0.15f);
        transform.localScale = vector;                                                 //���Ͱ��� ���ý����Ͽ� ����
    }
    /// <summary>
    /// �ٸ� ���� �ٶ󺸴� �÷��̾ �ٶ󺼶� ����Ǵ� ���Լ�
    /// </summary>
    protected virtual void TrunAnim()
    {

    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        if(attackPos != null)
        {
            Gizmos.color = Color.yellow;
            Vector2 leftUp = new Vector2(attackPos.position.x - attackRange.x * 0.5f, attackPos.position.y + attackRange.y * 0.5f);      //������
            Vector2 rightUp = new Vector2(leftUp.x + attackRange.x, leftUp.y);
            Vector2 leftDown = new Vector2(leftUp.x, leftUp.y - attackRange.y);
            Vector2 rightDown = new Vector2(rightUp.x, rightUp.y - attackRange.y);

            Gizmos.DrawLine(leftUp, rightUp);                                                                                           //����� �׸���
            Gizmos.DrawLine(leftUp, leftDown);
            Gizmos.DrawLine(rightUp, rightDown);
            Gizmos.DrawLine(leftDown, rightDown);
        }
    }
#endif
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBase : EnemyHP
{
    /// <summary>
    /// �Ѿ��� �ִϸ�����
    /// </summary>
    protected Animator anim;
    /// <summary>
    /// ��Ʈ�� ������Ʈ�� ������ ����
    /// </summary>
    protected GameObject hitobj = null;
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();            //�ִϸ����� ����
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))                              //���� �÷��̾��
        {
            hitobj= collision.gameObject;                                           //������Ʈ�� ����
            PlayerHit();                                                            //��Ʈ �Լ� ����
        }
       
    }
    /// <summary>
    /// �÷��̾ �ش� �Ѿ��� �¾����� ����Ǵ� �Լ�
    /// </summary>
    protected virtual void PlayerHit()
    {
        if (hitobj.activeSelf == true)                                              //�ش� ��ü�� �������� �ʾҴٸ�
        {
            PlayerHealth player=hitobj.GetComponent<PlayerHealth>();                //�÷��̾� ü�� �޾ƿ���
            player.HP = 1;                                                          //������ �ֱ�
            HitEffect();                                                            //����Ʈ ���
        }
    }
    /// <summary>
    /// �Ѿ��� �÷��̾ �¾� �������� ������ ����Ʈ �⺻ ��Ȱ��ȭ
    /// </summary>
    protected virtual void HitEffect()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// �Ѿ��� �����ð�
    /// </summary>
    /// <param name="time">������ �ð�</param>
    protected void DisableTimer(float time)
    {
        StartCoroutine(DisableTimerCoroutin(time));         //�ڷ�ƾ ����
    }
    /// <summary>
    /// �����ð� �� ��ü�� ��Ȱ��ȭ�� �ڷ�ƾ �Լ�
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator DisableTimerCoroutin(float time)
    {
        yield return new WaitForSeconds(time);          //�ð� ��ٸ���
        gameObject.SetActive(false);                    //��ü ��Ȱ��ȭ
    }
}

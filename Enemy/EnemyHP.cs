using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    /// <summary>
    /// �� Ȥ�� �ν����� ��ü�� ����� ��ũ��Ʈ
    /// </summary>
    public int hp;

    public int HP
    {
        get => hp;
        set
        {
            hp -= value;
            if (hp < 1)
            {
                OnDie();
            }
            else
            {
                OnHit();
            }
        }
    }
    /// <summary>
    /// ����  �ǰݵǾ��� �� ���� �Ǵ� �Լ�
    /// </summary>
    protected virtual void OnHit()
    {

    }
    /// <summary>
    /// ���� ����ź�� �ǰݵǾ����� ����Ǿ����� ����Ǵ� �Լ�
    /// </summary>
    public  void GrenadeHit(int damage)
    {
        int sethp = hp-damage;                          //hp�� �������� �����ϱ� ���� ���� ������ ���
        if (sethp < 1)
        {
            gameObject.layer = 10;
            Explosioned();                              //������ ������ �Լ� ���� 
            HP = damage;
            //gameObject.SetActive(false);
        }
        else
        {
            HP = damage;                                //�ƴϸ� ������ ����
        }
        
    }
    /// <summary>
    /// ���� ����ź�� �°� ������ ����Ǵ� �Լ�
    /// </summary>
    protected virtual void Explosioned()
    {

    }
    /// <summary>
    /// ���� ���� �� ����Ǵ� �Լ�
    /// </summary>
    protected virtual void OnDie()
    {

    }
    /// <summary>
    /// ���� ���� �����Ҷ� ����Ǵ� �Լ�
    /// </summary>
    public virtual void Landing()
    {

    }
    /// <summary>
    /// ���� ���߿� ������ ����Ǵ� �Լ�
    /// </summary>
    public virtual void Fly()
    {

    }
}

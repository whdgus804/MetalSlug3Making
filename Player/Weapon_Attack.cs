using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon_Attack : MonoBehaviour
{
    //�÷��̾��� �������� �� ���� �߻� ��ũ��Ʈ

    /// <summary>
    /// ���ݰ����� ���̾�
    /// </summary>
    [SerializeField] LayerMask canHitLayer;
    /// <summary>
    /// �÷��̾��� �������� ��ġ
    /// </summary>
    [SerializeField] Transform attackPos;
    /// <summary>
    /// �÷��̾��� �������� ����
    /// </summary>
    [SerializeField] Vector2 attackRange;

    /// <summary>
    /// ���⽺ũ��Ʈ �� ������Ʈ�� ����Ǿ��ִ� ������Ʈ
    /// </summary>
    [SerializeField] GameObject weapon;

    /// <summary>
    /// ���� �߻���ġ
    /// </summary>
    [Space(20.0f)]
    [SerializeField] Transform frontFirePos;
    /// <summary>
    /// �� �߻���ġ
    /// </summary>
    [SerializeField] Transform upFirePos;
    /// <summary>
    /// �ɾ� �߻� ��ġ
    /// </summary>
    [SerializeField] Transform sitFirePos;
    /// <summary>
    /// �� �߻���ġ
    /// </summary>
    [SerializeField] Transform downFirePos;


    /// <summary>
    /// ����ź�� ���� ��ġ
    /// </summary>
    [Space(20.0f)]
    [SerializeField] Transform throwPosition;

    /// <summary>
    /// ���� �� �̻� ȭ�鿡 ���̸� �ȵǴ� �Ѿ� �� ����ź�� ����Ҷ� ����� �� �ִ� �������� ��Ÿ���� ��ũ��Ʈ ����
    /// </summary>
    AmmoCounter ammoCounter;

    /// <summary>
    /// ���ѽ�ũ��Ʈ
    /// </summary>
    Pistol pistol;

    /// <summary>
    /// ����� ��ũ��Ʈ
    /// </summary>
    Machain_Gun machain_Gun;
    ///// <summary>
    ///// �÷��̾ ���� �ٶ󺸴��� ��Ÿ���� bool���� true�� ���� ������
    ///// </summary>
    //bool isLookUp;
    /// <summary>
    /// �÷��̾��� ���� Ÿ��
    /// </summary>
    public enum WeaponType
    {
        Pistol,                  //�⺻ ����
        Machain_Gun              //�����
    }
    public WeaponType weaponType;     //�÷��̾��� ����Ÿ��

    /// <summary>
    /// �÷��̾� ��ǲ
    /// </summary>
    BasicPlayerInput input;

    /// <summary>
    /// �Ѿ��� ȸ������ ������ ���Ͱ�
    /// </summary>
    Vector3 aimVector = Vector3.zero;

    PlayerAnimation anim;

    /// <summary>
    /// ���������� �������� ��Ÿ���� ����
    /// </summary>
    bool readToAtack=true;

    public int haveGrenade = 10;
    private void Awake()
    {
        input = new BasicPlayerInput();                         //��ǲ ����
        pistol= weapon.GetComponentInChildren<Pistol>();        //���� ��ũ��Ʈ ����
        machain_Gun= weapon.GetComponentInChildren<Machain_Gun>();        //���� ��ũ��Ʈ ����
        ammoCounter=GameManager.Instance.AmmoCounter;
        anim=GetComponent<PlayerAnimation>();
    }

    /// <summary>
    /// �÷��̾��� �����Լ�
    /// </summary>
    /// <param name="sign">0-���� 1-�� 2-�ɱ� 3-��</param>
    public void OnAttack(int sign, Vector3 localScale)
    {
        bool sit=false;
        if(sign==2)
            sit=true;
           
        bool inEnemy = EnemySencer(sit);                            //���� �߻��ϱ��� �������� ��Ÿ��� �� , �����ִ� NPC �Ǵ� �μ� �� �ִ� ��ü�� �ִ��� Ȯ��
        if (!inEnemy)                                            //���� ������ �ѹ߻�
        {
            anim.OnFire(sit);
            Transform aimPos = FirePosition(sign);
            switch (weaponType)                                  //���� ���⿡ ���� �߻��Լ� ����
            {
                case WeaponType.Pistol:                          //���Ⱑ �����̸�
                    pistol.OnFire(aimPos,aimVector,localScale,sign);             //���ѹ߻� �Լ� ����
                    break;
                case WeaponType.Machain_Gun:
                    machain_Gun.OnFire(aimPos,aimVector,localScale, sign);
                    //machain_Gun.FireAnim(anim);
                    break;
            }
        }

    }

    /// <summary>
    /// ���� �������� �����ȿ� �ִ��� ��Ÿ���� bool���� �Լ�
    /// </summary>
    /// <returns></returns>
    private bool EnemySencer(bool sit)
    {
        bool inEnemy = false;                                                                   //��ȯ�� ����
        Collider2D[] collider;                                                                  //���� ������ �迭
        collider = Physics2D.OverlapBoxAll(attackPos.position, attackRange, 0, canHitLayer);     //�������� ��ġ�� �������� ������ŭ 0�� ������ ���ݰ����� ���̾ ���� ��� �ݶ��̴��� �迭�� ����
        if (collider.Length == 0)                                                               //���� ������ false��ȯ
        {
            inEnemy = false;
        }
        else
        {                                                                                       //���� ������
            if (readToAtack)
            {
                EnemyHP enemyHP;                                                                    //��ũ��Ʈ ����
                for (int i = 0; i < collider.Length; i++)                                           //�ݶ��̴� �迭��ŭ �ݺ�
                {
                    if (!collider[i].CompareTag("NPC"))                                             //NPC�� �ƴ϶� �� Ȥ�� �μ� �� �ִ� ��ü��
                    {
                        enemyHP = collider[i].GetComponent<EnemyHP>();                              //��ũ��Ʈ ���� 
                        enemyHP.HP = 1;                                                             //������ 1 �ֱ�
                    }
                    else                                                                            //NPC��
                    {
                        //NPC���� Ǯ��
                        NPC_Base npc = collider[i].GetComponent<NPC_Base>();
                        npc.Hit();
                    }
                }
                StartCoroutine(AttackDelay());
                anim.OnAttack(sit);
            }
            readToAtack = false;
            inEnemy= true;                                                                      //��ȯ�� ����
            

        }
        return inEnemy;                                                                         //�� ��ȯ
    }
    /// <summary>
    /// �������� �����̸� ����ϴ� �ڷ�ƾ �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.5f);
        readToAtack = true;
    }
    /// <summary>
    /// ���� �÷��̾��� ���¿� ���� �Ѿ��� ������ġ�� �������ִ� �Լ�
    /// </summary>
    /// <param name="sign"></param>
    /// <returns></returns>
    private Transform FirePosition(int sign)
    {
        Transform aimpos=null;                      //��ȯ�� �����
        if (sign <= 1)                              //sign�� 0�Ǵ�1�̸�
        {
            if (sign == 0)                          //���̸�
            { 
                aimpos = frontFirePos;              //�߻���ġ�� ����
                aimVector = Vector3.zero;           //�Ѿ˿� �� ȸ������ 0
            }
            else                                    //���̸�
            {
                aimpos = upFirePos;                 //�߻���ġ�� ����
                aimVector.z = 90.0f;                //ȸ������ 90
            }
        }
        else                                        //sign�� 2�Ǵ� 3�̸�
        {
            if(sign == 2)                           //���� �����̸�
            {
                aimpos = sitFirePos;                //���� ������ �߻� ��ġ
                aimVector = Vector3.zero;           //ȸ������ 0
            }
            else                                    //���߿��� ���� �ٶ󺸴� �����̸�
            {
                aimpos = downFirePos;               //���� �߻���ġ
                aimVector.z = -90.0f;               //ȸ������ -90 (270)
            }
        }
        return aimpos;
    }
    /// <summary>
    /// ����ź �߻��Լ�
    /// </summary>
    public void OnGrenade()
    {
        if(ammoCounter.grenadeCount > 0 && haveGrenade >0)
        {
            PoolFactory.Instance.GetBoamStick(throwPosition.position, transform.localScale);
            ammoCounter.grenadeCount--;
            haveGrenade--;
            ammoCounter.OnGrenade(haveGrenade);
        }
    }
    /// <summary>
    /// �÷��̾ �ൿ���� �������� �����Ҷ� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="sign"></param>
    public void AimMoved(int sign)
    {
        //switch (weaponType)
        //{
        //    case (WeaponType.Pistol):
        //        break;
        //    case(WeaponType.Machain_Gun):
        //        break;
        //}
        if (weaponType == WeaponType.Machain_Gun)
        {
            machain_Gun.CrossFire(sign,transform.localScale);
        }
    }

    public void ChangeWeapon(WeaponType weaponType)
    {
        anim.ChangeWeapon(weaponType);
    }
    public void OnRespawn()
    {
        haveGrenade = 10;
        weaponType=WeaponType.Pistol;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //���� ������ ���� ������ �����Ͽ� ����� �߰�


        //Gizmos.color = Color.red;
        //Gizmos.DrawCube(attackPos.position, attackRang);
        Gizmos.color = Color.yellow;

        
        Vector2 leftUp=new Vector2(attackPos.position.x - attackRange.x * 0.5f, attackPos.position.y + attackRange.y * 0.5f);      //������
        Vector2 rightUp=new Vector2( leftUp.x+attackRange.x,leftUp.y);
        Vector2 leftDown = new Vector2(leftUp.x,leftUp.y-attackRange.y);
        Vector2 rightDown = new Vector2(rightUp.x, rightUp.y - attackRange.y);

        Gizmos.DrawLine(leftUp, rightUp);                                                                                           //����� �׸���
        Gizmos.DrawLine(leftUp,leftDown);
        Gizmos.DrawLine (rightUp,rightDown);
        Gizmos.DrawLine(leftDown, rightDown);
        //Gizmos.DrawIcon(new Vector2(leftUpX, leftUpY),"Left");
    }

#endif
}

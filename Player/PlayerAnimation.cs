using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    /// <summary>
    /// ��ü ��������Ʈ
    /// </summary>
    SpriteRenderer uppersprite;
    /// <summary>
    /// ��ü ��������Ʈ
    /// </summary>
    SpriteRenderer lowerSprite;
    /// <summary>
    /// ��ü �ִϸ�����
    /// </summary>
    Animator upperAnim;
    /// <summary>
    /// ��ü �ִϸ�����
    /// </summary>
    Animator lowerAnim;
    /// <summary>
    /// �������� ���� ������ �Ͽ����� ��Ÿ���� ���� true�� �̹� ������ ���� ������ 
    /// </summary>
    bool axeAttack = true;
    /// <summary>
    /// ���� ȸ���ϴ� �ִϸ��̼�
    /// </summary>
    int trun_To_Hash = Animator.StringToHash("Trun");
    /// <summary>
    /// �̵��ϴ� �ִϸ��̼�
    /// </summary>
    int move_To_Hash = Animator.StringToHash("Move");
    /// <summary>
    /// �������� �ִϸ��̼�
    /// </summary>
    int fall_To_Hash = Animator.StringToHash("Fall");
    /// <summary>
    /// �߻��ϴ� �ִϸ��̼�
    /// </summary>
    int fire_To_Hash = Animator.StringToHash("Fire");
    /// <summary>
    /// ���� ���� �ִϸ��̼�
    /// </summary>
    int lookUp_To_Hash = Animator.StringToHash("LookUp");
    /// <summary>
    /// �ɰų� ���߿��� �ϴ��� ���� �ִϸ��̼�
    /// </summary>
    int down_To_Hash = Animator.StringToHash("LookDown_Sit");
    /// <summary>
    /// ���� �ٴ� �ִϸ��̼�
    /// </summary>
    int jump_To_Hash = Animator.StringToHash("Jump");
    /// <summary>
    /// �������� �ִϸ��̼�
    /// </summary>
    int attack_To_Hash = Animator.StringToHash("Attack");
    /// <summary>
    /// ������ �����ϴ� �ִϸ��̼�
    /// </summary>
    int Axe_To_Hash = Animator.StringToHash("Axe");
    /// <summary>
    /// ������� �߻������� ��Ÿ���� �ִϸ��̼�
    /// </summary>
    int firing_To_Hash = Animator.StringToHash("Firing");
    /// <summary>
    /// ��� �ִϸ��̼�
    /// </summary>
    int die_To_Hash = Animator.StringToHash("Die");
    /// <summary>
    /// ��Ȱ �ִϸ��̼�
    /// </summary>
    int respawn_To_Hash = Animator.StringToHash("Respawn");


    /// <summary>
    /// ��û ��ü AC
    /// </summary>
    [Space(20)]
    [SerializeField] RuntimeAnimatorController pistolUpperAC;
    /// <summary>
    /// ���� ��ü AC
    /// </summary>
    [SerializeField] RuntimeAnimatorController pistolLowerAC;
    /// <summary>
    /// ����� ��ü AC
    /// </summary>
    [Space(20)]
    [SerializeField] AnimatorOverrideController hmUpperAC;
    /// <summary>
    /// ����� ��ü AC
    /// </summary>
    [SerializeField] AnimatorOverrideController hmLowerAC;


    /// <summary>
    /// ���ⱳü�� ����߸� ������Ʈ
    /// </summary>
    [SerializeField] GameObject dropGun;
    /// <summary>
    /// ���� ���� ��ũ��Ʈ
    /// </summary>
    Weapon_Attack weapon_Attack;
    private void Awake()
    {
        uppersprite = GetComponent<SpriteRenderer>();           
        upperAnim = GetComponent<Animator>();
        Transform tran = transform.GetChild(0);
        lowerSprite = tran.GetComponent<SpriteRenderer>();
        lowerAnim = tran.GetComponent<Animator>();
        weapon_Attack=GetComponent<Weapon_Attack>();
    }
    private void OnEnable()
    {
        hmLowerAC.runtimeAnimatorController = lowerAnim.runtimeAnimatorController;
        hmUpperAC.runtimeAnimatorController = upperAnim.runtimeAnimatorController;
    }
    /// <summary>
    /// �÷��̾� �̵� �ִϸ��̼�
    /// </summary>
    /// <param name="moveValue"></param>
    public void OnMove(Vector2 moveValue)
    {
        upperAnim.SetFloat(move_To_Hash, moveValue.x);      //�����ӿ� ���� ����
        lowerAnim.SetFloat(move_To_Hash, moveValue.x);      //�����ӿ� ���� ����
    }
    /// <summary>
    /// �ڸ� ���ƺ��� �ִϸ��̼�
    /// </summary>
    public void OnTrun()
    {
        lowerAnim.SetTrigger(trun_To_Hash);     //��ü �ִϸ��̼ǿ� ������
    }
    /// <summary>
    /// �������� ����
    /// </summary>
    /// <param name="fall"></param>
    public void OnFall(bool fall)
    {
        upperAnim.SetBool(fall_To_Hash, fall);      //�������� ���� ����
        lowerAnim.SetBool(fall_To_Hash, fall);      //�������� ���� ����

    }
    /// <summary>
    /// ���� �߻� �Լ�
    /// </summary>
    /// <param name="sit">���� �ɾ� �ִ��� ��Ÿ���� ����</param>
    public void OnFire(bool sit)
    {
        if (!sit)
        {
            upperAnim.SetTrigger(fire_To_Hash);  //�������� ��ü �߻�      

        }
        else
        {
            lowerAnim.SetTrigger(fire_To_Hash);     //�ɾ������� ��ü �߻� 

        }
    }
    /// <summary>
    /// �÷��̾� ���� ���׸� ��Ÿ���� �Լ�
    /// </summary>
    /// <param name="sign"></param>
    public void ChangeLookPoint(int sign)
    {
        /// 0-����
        /// 1-��
        /// 2-�ɱ�
        /// 3-��
        bool up = false;        //���� ����
        bool down = false;      //���� ���� 
        switch (sign)
        {
            case 1:
                up = true;
                break;
            case 2:
                down = true;
                break;
            case 3:
                down = true;
                break;
        }
        upperAnim.SetBool(lookUp_To_Hash, up);
        upperAnim.SetBool(down_To_Hash, down);

        lowerAnim.SetBool(lookUp_To_Hash, up);
        lowerAnim.SetBool(down_To_Hash, down);
    }
    /// <summary>
    /// ���� �پ������ �Լ�
    /// </summary>
    public void OnJump()
    {
        lowerAnim.SetTrigger(jump_To_Hash);     //�����ִϸ��̼� ������
    }
    /// <summary>
    /// �ٹ����� �Լ�
    /// </summary>
    /// <param name="sit">���� �ɾ� �ִ��� ��Ÿ���� ����</param>
    public void OnAttack(bool sit)
    {
        if (!sit)
        {
            upperAnim.SetBool(Axe_To_Hash, axeAttack);      //���� ������
            upperAnim.SetTrigger(attack_To_Hash);           //��ü ������

        }
        else
        {
            lowerAnim.SetTrigger(attack_To_Hash);   //��ü ������
        }
        axeAttack = !axeAttack;     //���� ����
        
    }
    public void OnFiring(bool nowfire)
    {
        upperAnim.SetBool(firing_To_Hash, nowfire); 
        
    }
    public void OnDie()
    {
        lowerAnim.SetTrigger(die_To_Hash);
    }
    public void OnDie_Lucost()
    {
        uppersprite.enabled = false;
        lowerAnim.SetTrigger("Lucost");
    }
    public void OnReapwn()
    {
        lowerAnim.SetTrigger(respawn_To_Hash);
    }
    /// <summary>
    /// �׾��� �� �ִϸ��̼� ������ �ʱ�ȭ�ϴ� �Լ�
    /// </summary>
    public void OnDeadResetAnim()
    {
        upperAnim.SetBool(lookUp_To_Hash, false);       //��ü �ִϸ��̼� �� �ʱ�ȭ
        lowerAnim.SetBool(lookUp_To_Hash, false);       
        upperAnim.SetBool(down_To_Hash, false);         //��ü �ִϸ��̼� �� �ʱ�ȭ
        lowerAnim.SetBool(down_To_Hash, false);
    }
    /// <summary>
    /// ���� ���� �״� �ִϸ��̼� 
    /// </summary>
    public void Dead_Water()
    {
        lowerAnim.SetTrigger("Water");              //�ִϸ��̼� ������
        
    }
    /// <summary>
    /// ���ⱳü �Լ�
    /// </summary>
    /// <param name="weaponType">��ü�� ���� </param>
    public void ChangeWeapon(Weapon_Attack.WeaponType weaponType)
    {
        switch (weaponType)
        {
            case Weapon_Attack.WeaponType.Pistol:       //�����̸�
                upperAnim.SetTrigger("ChangeWeapon");       //��ü �ִϸ��̼� ���
                upperAnim.runtimeAnimatorController = pistolUpperAC;    //�ִϸ����� ����
                lowerAnim.runtimeAnimatorController= pistolLowerAC;     //��ü �ִϸ����� ����
                dropGun.transform.position = transform.position;        //������ġ ����
                dropGun.SetActive(true);                                //���� Ȱ��ȭ
                break;
            case Weapon_Attack.WeaponType.Machain_Gun:                  //������̸�

                if (weapon_Attack.weaponType != Weapon_Attack.WeaponType.Machain_Gun)
                {
                         
                    upperAnim.runtimeAnimatorController = hmUpperAC;        //�ִϸ����� ��Ʈ�ѷ� ����
                    
                    lowerAnim.runtimeAnimatorController = hmLowerAC;        //�ִϸ����� ��Ʈ�ѷ�
                }
                else
                {

                }
                break;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : ItemBase
{
    /// <summary>
    /// �ƹ��� �������� Ÿ��
    /// </summary>
    public enum ItemType
    {
        Ammo,
        HM
    }
    /// <summary>
    /// ���� �������� Ÿ��
    /// </summary>
    public ItemType Type;



    protected override void GetItem(GameObject player)
    {
        base.GetItem(player);   
        Weapon_Attack weapon_Attack=player.GetComponent<Weapon_Attack>();                       //�÷��̾� ���� ��ũ��Ʈ �ޱ�
        switch (Type)                                                                           //���� Ÿ�Կ� ���� ������ ����� �÷��̾� ���� ����
        {
            case ItemType.Ammo:                                                                 //���� ���� Ÿ���� �ƴ� �Ѿ� �������̸� �������� ������ �Ѿ� ����
                switch (weapon_Attack.weaponType)                                               //�÷��̾��� ���� ���� Ÿ��
                {
                    case Weapon_Attack.WeaponType.Machain_Gun:                                  //������̸�
                        Machain_Gun machain=player.GetComponentInChildren<Machain_Gun>();       //��ũ��Ʈ �ޱ�
                        machain.GetAmmoBox();                                                   //�Ѿ� ���� �Լ�����
                        break;
                }
                break;
            case ItemType.HM:                                                                   //���� �������� Ÿ���� ������̸�
                Machain_Gun machain_Gun=player.GetComponentInChildren<Machain_Gun>();           //��ũ��Ʈ �ޱ�
                machain_Gun.ammo = 300;                                                         //ȹ�� �Ѿ˷� �Ѿ� ����
                weapon_Attack.ChangeWeapon(Weapon_Attack.WeaponType.Machain_Gun);
                weapon_Attack.weaponType = Weapon_Attack.WeaponType.Machain_Gun;                //���� ����
                scoreManager.ChangeWeapon();
                break;
        }
    }
}

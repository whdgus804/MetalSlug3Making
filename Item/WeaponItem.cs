using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : ItemBase
{
    /// <summary>
    /// 아무기 아이템의 타입
    /// </summary>
    public enum ItemType
    {
        Ammo,
        HM
    }
    /// <summary>
    /// 무기 아이템의 타입
    /// </summary>
    public ItemType Type;



    protected override void GetItem(GameObject player)
    {
        base.GetItem(player);   
        Weapon_Attack weapon_Attack=player.GetComponent<Weapon_Attack>();                       //플레이어 무기 스크립트 받기
        switch (Type)                                                                           //무기 타입에 따라 지정된 무기로 플레이어 무기 변경
        {
            case ItemType.Ammo:                                                                 //무기 변경 타입이 아닌 총알 아이템이면 장착중인 무기의 총알 증가
                switch (weapon_Attack.weaponType)                                               //플레이어의 현재 무기 타입
                {
                    case Weapon_Attack.WeaponType.Machain_Gun:                                  //기관총이면
                        Machain_Gun machain=player.GetComponentInChildren<Machain_Gun>();       //스크립트 받기
                        machain.GetAmmoBox();                                                   //총알 증가 함수실행
                        break;
                }
                break;
            case ItemType.HM:                                                                   //무기 아이템의 타입이 기관총이면
                Machain_Gun machain_Gun=player.GetComponentInChildren<Machain_Gun>();           //스크립트 받기
                machain_Gun.ammo = 300;                                                         //획득 총알로 총알 변경
                weapon_Attack.ChangeWeapon(Weapon_Attack.WeaponType.Machain_Gun);
                weapon_Attack.weaponType = Weapon_Attack.WeaponType.Machain_Gun;                //무기 변경
                scoreManager.ChangeWeapon();
                break;
        }
    }
}

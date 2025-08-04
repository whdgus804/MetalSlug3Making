using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    protected bool readyToFire = true;
    [SerializeField]protected float fireDelay;
    [SerializeField]
    [Range(0f, 1f)]protected float sitDelay;
    
    /// <summary>
    /// 총의 최대 총알 수
    /// </summary>
    [SerializeField] int maxAmmo;
    /// <summary>
    /// 무기의 기본 총알 수
    /// </summary>
    public int ammo;
    /// <summary>
    /// 아이템을 먹었을때 증가될 총알의 수
    /// </summary>
    [SerializeField] int addAmmo;

   [SerializeField] Weapon_Attack weapon_Attack;




    /// <summary>
    /// 무기발사 함수
    /// </summary>
    /// <param name="aimPositon">발사 위치</param>
    /// <param name="sign">0.앞,1.위2.앉기,3밑</param>
    public virtual void OnFire(Transform aimPositon,Vector3 aimVector,Vector3 localScale, int sign)
    {
        Debug.Log($"{gameObject.name}_Fire");
    }

    protected void StartRaedyToFireDelay(float time)
    {
        StartCoroutine(ReadyToFireCoroutin(time));
    }
    IEnumerator ReadyToFireCoroutin(float time)
    {
        yield return new WaitForSeconds(time);
        readyToFire= true;
    }
    /// <summary>
    /// 총알을 수를 깍고 총알을 모두 사용하였는지 계산하는 함수
    /// </summary>
    protected virtual  void AmmoDiscount()
    {
        ammo--;
        if (ammo < 1)
        {
            EmptyAmmo();
        }
    }
    protected virtual void EmptyAmmo()
    {
        StopAllCoroutines();
        
        weapon_Attack.ChangeWeapon(Weapon_Attack.WeaponType.Pistol);
        weapon_Attack.weaponType = Weapon_Attack.WeaponType.Pistol;
        
        readyToFire= false;
    }
    
    /// <summary>
    /// 총알 아이템을 습득하였을 때 실행함수
    /// </summary>
    public virtual void GetAmmoBox()
    {
        int setammo = ammo + addAmmo;           //총알을 습득할때 최대 총알을 넘는지 확인을 위해 변수 생성 및 기존총알과 추가될 총알 더하기
        if (setammo > maxAmmo)                  //얻을 총알과 기존총알의 합이 최대 총알 수 를 넘었다면 
        {
            setammo= maxAmmo;                   //바뀔 총알 수는 최대 총알 수로 변경
        }
        ammo = setammo;                         //총알 변경
    }
    
}

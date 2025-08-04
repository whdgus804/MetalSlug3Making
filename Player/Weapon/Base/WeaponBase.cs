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
    /// ���� �ִ� �Ѿ� ��
    /// </summary>
    [SerializeField] int maxAmmo;
    /// <summary>
    /// ������ �⺻ �Ѿ� ��
    /// </summary>
    public int ammo;
    /// <summary>
    /// �������� �Ծ����� ������ �Ѿ��� ��
    /// </summary>
    [SerializeField] int addAmmo;

   [SerializeField] Weapon_Attack weapon_Attack;




    /// <summary>
    /// ����߻� �Լ�
    /// </summary>
    /// <param name="aimPositon">�߻� ��ġ</param>
    /// <param name="sign">0.��,1.��2.�ɱ�,3��</param>
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
    /// �Ѿ��� ���� ��� �Ѿ��� ��� ����Ͽ����� ����ϴ� �Լ�
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
    /// �Ѿ� �������� �����Ͽ��� �� �����Լ�
    /// </summary>
    public virtual void GetAmmoBox()
    {
        int setammo = ammo + addAmmo;           //�Ѿ��� �����Ҷ� �ִ� �Ѿ��� �Ѵ��� Ȯ���� ���� ���� ���� �� �����Ѿ˰� �߰��� �Ѿ� ���ϱ�
        if (setammo > maxAmmo)                  //���� �Ѿ˰� �����Ѿ��� ���� �ִ� �Ѿ� �� �� �Ѿ��ٸ� 
        {
            setammo= maxAmmo;                   //�ٲ� �Ѿ� ���� �ִ� �Ѿ� ���� ����
        }
        ammo = setammo;                         //�Ѿ� ����
    }
    
}

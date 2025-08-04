using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageMove : MonoBehaviour
{
    AmmoCounter ammoCounter;
    [SerializeField] Weapon_Attack weapon_Attack;
    [SerializeField] Machain_Gun machain_Gun;
    
    private void Awake()
    {
        ammoCounter = GameManager.Instance.AmmoCounter;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(weapon_Attack.weaponType == Weapon_Attack.WeaponType.Machain_Gun)
            {
                ammoCounter.saveAmmo = machain_Gun.ammo;
            }
            ammoCounter.saveGrenade = weapon_Attack.haveGrenade;
            SceneManager.LoadScene(3);
        }
    }
}

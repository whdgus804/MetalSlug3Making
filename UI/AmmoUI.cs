using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    [SerializeField]Image[] ammoImages;
    [SerializeField]Image[] grenadeImages;
    [SerializeField] Image eternalAmmo;
    [SerializeField] Sprite[] numbers;
    //[SerializeField] Machain_Gun machain_Gun;

    private void Start()
    {
        PistolAmmoUI(true);
    }
    public void PistolAmmoUI(bool set)
    {
        eternalAmmo.enabled = set;
        for(int i = 0; i < ammoImages.Length; i++)
        {
            ammoImages[i].enabled = !set;
        }
    }
    public void MachainGunAmmoUI()
    {
        eternalAmmo.enabled=false;
        for (int i = 0; i < ammoImages.Length; i++)
        {
            ammoImages[i].enabled = true;
            
        }
        ammoImages[0].sprite=numbers[0];
        ammoImages[1].sprite=numbers[0];
        ammoImages[2].sprite=numbers[3];
    }
    public void AmmoDiscount(int ammo)
    {

        string stringnum=ammo.ToString();
        for (int i = 0; i < stringnum.Length; i++)
        {
            char countAmmo = stringnum[i];
            int num=(int)char.GetNumericValue(countAmmo);
            ammoImages[stringnum.Length-i-1].sprite = numbers[num];
        }
    }
}

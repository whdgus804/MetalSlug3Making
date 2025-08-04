using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCounter : MonoBehaviour
{
    public int saveAmmo = 0;
    public int saveGrenade = 10;
    /// <summary>
    /// ¼ö·ùÅº °³¼ö¸¦ »ø º¯¼ö
    /// </summary>
    public int grenadeCount =2;
    GrenadeCountUI grenadeCountUI;
    private void Awake()
    {
        grenadeCountUI=GameManager.Instance.GrenadeCountUI;
    }
    private void OnEnable()
    {
        grenadeCount = 2;
    }
    public void OnGrenade(int hasGrenade)
    {
        grenadeCountUI.OnGrenade(hasGrenade);
    }
}

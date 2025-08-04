using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeCountUI : MonoBehaviour
{
    [SerializeField] Sprite[] numbersprite;
    [SerializeField] Image[] countImage;

    AmmoCounter ammoCounter;
    private void Awake()
    {
        //ammoCounter = FindAnyObjectByType<AmmoCounter>();   
        ammoCounter = GameManager.Instance.AmmoCounter;
    }

    private void OnEnable()
    {
        OnGrenade(ammoCounter.saveGrenade);
    }
    /// <summary>
    /// ���� ����ź�� ���� ui�� ��Ÿ���� �Լ� 
    /// </summary>
    /// <param name="hasGrenade"></param>
    public void OnGrenade(int hasGrenade)
    {
        Debug.Log(hasGrenade);
        if (hasGrenade > 9)
        {
            countImage[1].sprite = numbersprite[1];
            countImage[0].sprite = numbersprite[0];
        }
        else
        {
            countImage[1].sprite = numbersprite[0];
            countImage[0].sprite = numbersprite[hasGrenade];

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditUI : MonoBehaviour
{
    [SerializeField] Sprite[] numerSprite;
    [SerializeField] Image creditImage;
    ScoreManager scoreManager;
    private void Awake()
    {
        scoreManager=GameManager.Instance.ScoreManager;
    }
    private void OnEnable()
    {
        Debug.Log(scoreManager.Coin);
        CreditUse(scoreManager.Coin);
    }
    public void CreditUse(int haveCredit)
    {
        creditImage.sprite = numerSprite[haveCredit];
    }
}

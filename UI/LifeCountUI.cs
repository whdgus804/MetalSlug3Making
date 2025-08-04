using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeCountUI : MonoBehaviour
{
    /// <summary>
    /// 카운트 이미지 0,1,2
    /// </summary>
    [SerializeField] Sprite[] number;
    [SerializeField] Image countimage;

    ScoreManager scoreManager;
    private void Awake()
    {
        scoreManager=GameManager.Instance.ScoreManager;
    }
    private void OnEnable()
    {
        Count(scoreManager.lifeScore);
    }
    public void Count(int count)
    {
        countimage.sprite = number[count];
    }
}

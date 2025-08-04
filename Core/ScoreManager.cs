using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// 사망 플레이어
    /// </summary>
    GameObject deadPlayer;
    /// <summary>
    /// 점수 ui
    /// </summary>
    Score scoreUI;
    /// <summary>
    /// 타이머 스크립트
    /// </summary>
    Timer timer;
    /// <summary>
    /// 점수
    /// </summary>
    public int score = 0;
    /// <summary>
    /// 카매라 위치
    /// </summary>
    [HideInInspector] public Transform cameraPos=null;
    /// <summary>
    /// 남은 생명을 보일 ui
    /// </summary>
    LifeCountUI lifeCountUI;
    /// <summary>
    /// 남은 크래딧으 보일 ui
    /// </summary>
    CreditUI creditsUI;
    /// <summary>
    /// 남은 총알을 보일 ui
    /// </summary>
    AmmoUI ammoUI;

    public int prisonerCount = 0;
    private void Awake()
    {
        scoreUI = FindAnyObjectByType<Score>();
        lifeCountUI= FindAnyObjectByType<LifeCountUI>();
        ammoUI = FindAnyObjectByType<AmmoUI>();
        timer = FindAnyObjectByType<Timer>();
        creditsUI = FindAnyObjectByType<CreditUI>();
    }
    public int Score
    {
        get=>score;
        set
        {
            score = value;
            scoreUI.GetScore(score);
            //Debug.Log(score);
        }
    }

    public int lifeScore = 2;
    public int LifeScore
    {
        get => lifeScore;
        set
        {
            lifeScore --;
            if(lifeScore < 0)
            {
                InsertCoin();
            }
            lifeCountUI.Count(lifeScore);
            //Debug.Log($"Life={lifeScore},Coin={coin}");
        }
    }

    [SerializeField] int coin = 100;
    public int Coin
    {
        get => coin;
        set
        {
            if (coin != 0)
            {
                coin--;
                creditsUI.CreditUse(coin);
                lifeScore = 3;
            }
        }
    }
    /// <summary>
    /// 플레이어가 죽고다시 리스폰 될때 실행되는 함수
    /// </summary>
    public void ReSpawn(GameObject player)
    {
        prisonerCount = 0;
        if(player != null)
            deadPlayer = player;
        LifeScore--;
        if(player != null)
        {
            PlayerHealth playerHP= deadPlayer.GetComponent<PlayerHealth>();
            playerHP.ReadyToResapwn();
        }
        timer.OnRespawn();
    }
    /// <summary>
    /// 플레이어가 라이프 만큼 죽으면 코인을 하나 감소하고 라이프를 3개 채워 주는 함수
    /// </summary>
     void InsertCoin()
    {
        Coin--;
        
        ReSpawn(null);
    }
    /// <summary>
    /// 카메라의 위치를 받는함수
    /// </summary>
    /// <param name="transform"></param>
    public void CameraPos(Transform transform)
    {
        cameraPos = transform;
    }

    public void ChangeWeapon()
    {
        ammoUI.MachainGunAmmoUI();
    }


}
